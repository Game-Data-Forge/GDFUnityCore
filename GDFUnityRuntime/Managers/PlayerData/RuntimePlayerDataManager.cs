using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GDFFoundation;
using GDFRuntime;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace GDFUnity
{
    [Dependency(typeof(IRuntimeConfigurationEngine), typeof(IRuntimeAccountManager))]
    [JobLockers(typeof(IRuntimeAccountManager), typeof(IRuntimePlayerPersistanceManager))]
    public class RuntimePlayerDataManager : APIManager, IRuntimePlayerDataManager
    {
        private enum GameSaveStateFlag : byte
        {
            None = 0,
            Existant = 1,
            Loaded = 2,
        }

        static public class Exceptions
        {
            static public GDFException InvalidGameSave => new GDFException("PLA", 01, "Invalid Game save ! Value must be [0; 99].");
            static public GDFException CannotAddTrashed => new GDFException("PLA", 02, "Cannot add a trashed player data !");
            static public GDFException UserChanged => new GDFException("PLA", 03, "Cannot execute method ! User changed during execution...");
            static public GDFException DataAlreadyExists => new GDFException("PLA", 04, "Player data already exists !");
            static public GDFException InvalidReference => new GDFException("PLA", 05, "Invalid player data reference !");
            static public GDFException NotFoundInCache => new GDFException("PLA", 06, "Player data not found in cache !");
            static public GDFException ReferenceAlreadySet => new GDFException("PLA", 07, "Player data already has a reference !");
            static public GDFException InvalidGDFType => new GDFException("PLA", 08, "Type is not a valid GDFPlayerData type !");
            static public GDFException InvalidTypeForReference => new GDFException("PLA", 09, "Cannot create a data for a reference that already uses another type !");
            static public GDFException GameSaveBusy(byte gameSave, PlayerDataJob.Type state) => new GDFException("PLA", 10, $"The gamesave {gameSave} is {state} !");
            static public GDFException GameSaveNotLoaded(byte gameSave) => new GDFException("PLA", 11, $"The gamesave {gameSave} is not loaded !");
            static public GDFException GameSaveNotExisting(byte gameSave) => new GDFException("PLA", 12, $"The gamesave {gameSave} does not exist !");
            static public GDFException InvalidMigrationAccount => new GDFException("PLA", 13, $"You cannot be connected to a local account when migrating data !");
            static public GDFException NoMigrationLocalDataFound => new GDFException("PLA", 14, $"No local account data found for migration !");
        }

        internal class Lock : IDisposable
        {
            private object _lock = new object();

            public Lock Use(IRuntimeEngine engine)
            {
                long account = engine.AccountManager.IsAuthenticated ? engine.AccountManager.Reference : 0;
                Monitor.Enter(_lock);
                if (account != (engine.AccountManager.IsAuthenticated ? engine.AccountManager.Reference : 0))
                {
                    Dispose();
                    throw Exceptions.UserChanged;
                }
                return this;
            }

            public void Dispose()
            {
                Monitor.Exit(_lock);
            }
        }

        internal struct Unicity
        {
            public string reference;
            public short gameSave;

            public override int GetHashCode()
            {
                return HashCode.Combine(gameSave, reference);
            }

            static public implicit operator Unicity(GDFPlayerDataStorage other)
            {
                return new Unicity
                {
                    reference = other.Reference,
                    gameSave = other.GameSave
                };
            }

            static public implicit operator Unicity(GDFPlayerData other)
            {
                return new Unicity
                {
                    reference = other.Reference,
                    gameSave = other.GameSave
                };
            }
        }

        internal class SyncQueue : IEnumerable<GDFPlayerDataStorage>
        {
            private Dictionary<Unicity, GDFPlayerDataStorage> _storages = new Dictionary<Unicity, GDFPlayerDataStorage>();

            public int Count => _storages.Count;

            public GDFPlayerDataStorage Find(Unicity unicity)
            {
                if (_storages.TryGetValue(unicity, out GDFPlayerDataStorage value))
                {
                    return value;
                }
                return null;
            }

            public void Add(GDFPlayerDataStorage storage)
            {
                Unicity unicity = storage;
                if (_storages.TryGetValue(unicity, out GDFPlayerDataStorage _))
                {
                    return;
                }
                
                _storages.Add(unicity, storage);
            }

            public void AddOrUpdate(GDFPlayerDataStorage storage)
            {
                Unicity unicity = storage;
                if (_storages.ContainsKey(unicity))
                {
                    _storages[unicity] = storage;
                    return;
                }
                
                _storages.Add(unicity, storage);
            }

            public void Remove(IEnumerable<GDFPlayerDataStorage> storages)
            {
                foreach (GDFPlayerDataStorage storage in storages)
                {
                    Remove(storage);
                }
            }

            public void Remove(GDFPlayerDataStorage storage)
            {
                _storages.Remove(storage);
            }

            public void ClearGameSave(byte gameSave)
            {
                Dictionary<Unicity, GDFPlayerDataStorage> storages = new Dictionary<Unicity, GDFPlayerDataStorage>();

                foreach (KeyValuePair<Unicity, GDFPlayerDataStorage> storage in _storages)
                {
                    if (storage.Key.gameSave == gameSave) continue;

                    storages.Add(storage.Key, storage.Value);
                }

                _storages = storages;
            }

            public void Fill(List<GDFPlayerDataStorage> buffer, int count)
            {
                foreach (GDFPlayerDataStorage storage in this)
                {
                    GDFPlayerDataStorage duplicate = new GDFPlayerDataStorage();
                    duplicate.CopyFrom(storage);
                    buffer.Add(duplicate);
                    if (buffer.Count >= count)
                    {
                        return;
                    }
                }
            }

            public List<GDFPlayerDataStorage> ToList()
            {
                List<GDFPlayerDataStorage> storages = new List<GDFPlayerDataStorage> ();

                foreach (GDFPlayerDataStorage storage in this)
                {
                    GDFPlayerDataStorage duplicate = new GDFPlayerDataStorage();
                    duplicate.CopyFrom(storage);
                    storages.Add(duplicate);
                }

                return storages;
            }

            public void Clear()
            {
                _storages.Clear();
            }

            public IEnumerator<GDFPlayerDataStorage> GetEnumerator()
            {
                return _storages.Values.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        internal readonly Lock _lock = new Lock();
        internal IRuntimeEngine _engine;
        internal SyncQueue _syncQueue = new SyncQueue();
        internal PlayerStorageInformation _information = new PlayerStorageInformation();
        internal System.Random _rng = new System.Random();
        private Dictionary<byte, PlayerDataJob> _gameSaveJobs = new Dictionary<byte, PlayerDataJob>();
        private ConcurrentDictionary<byte, PlayerDataGameSave> _gameSaves = new ConcurrentDictionary<byte, PlayerDataGameSave>();
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        private List<GDFPlayerDataStorage> _dataToSyncLoadBuffer = new List<GDFPlayerDataStorage>();
        private byte _activeGameSave = 0;
        private bool _syncDataLoaded = false;
        private Notification _syncing;
        private Notification _synced;
        private Notification<byte> _loading;
        private Notification<byte> _loaded;
        private Notification<byte> _unloading;
        private Notification<byte> _unloaded;
        private Notification<byte> _saving;
        private Notification<byte> _saved;
        private Notification<byte> _deleting;
        private Notification<byte> _deleted;
        private Notification<byte, byte> _duplicating;
        private Notification<byte, byte> _duplicated;

        public bool HasDataToSync => _syncQueue.Count != 0;
        public int DataToSyncAmount => _syncQueue.Count;
        public byte ActiveGameSave
        {
            get => _activeGameSave;
            set
            {
                AssertGameSave(value);
            
                _activeGameSave = value;
            }
        }
        public Notification Syncing => _syncing;
        public Notification Synced => _synced;
        public Notification<byte> Loading => _loading;
        public Notification<byte> Loaded => _loaded;
        public Notification<byte> Unloading => _unloading;
        public Notification<byte> Unloaded => _unloaded;
        public Notification<byte> Saving => _saving;
        public Notification<byte> Saved => _saved;
        public Notification<byte> Deleting => _deleting;
        public Notification<byte> Deleted => _deleted;
        public Notification<byte, byte> Duplicating => _duplicating;
        public Notification<byte, byte> Duplicated => _duplicated;

        protected override Type JobLokerType => typeof(IRuntimePlayerDataManager);

        public RuntimePlayerDataManager(IRuntimeEngine engine)
        {
            _engine = engine;

            _syncing = new Notification(engine.ThreadManager);
            _synced = new Notification(engine.ThreadManager);
            _loading = new Notification<byte>(engine.ThreadManager);
            _loaded = new Notification<byte>(engine.ThreadManager);
            _unloading = new Notification<byte>(engine.ThreadManager);
            _unloaded = new Notification<byte>(engine.ThreadManager);
            _saving = new Notification<byte>(engine.ThreadManager);
            _saved = new Notification<byte>(engine.ThreadManager);
            _deleting = new Notification<byte>(engine.ThreadManager);
            _deleted = new Notification<byte>(engine.ThreadManager);
            _duplicated = new Notification<byte, byte>(engine.ThreadManager);
            _duplicating = new Notification<byte, byte>(engine.ThreadManager);

            _engine.AccountManager.AccountChanging.onBackgroundThread += OnAccountChanging;
            _engine.AccountManager.AccountChanged.onBackgroundThread += OnAccountChanged;
            _engine.AccountManager.AccountDeleting.onBackgroundThread += PurgeRunner;
        }

        ~RuntimePlayerDataManager()
        {
            _engine.AccountManager.AccountDeleting.onBackgroundThread -= PurgeRunner;
            _engine.AccountManager.AccountChanged.onBackgroundThread -= OnAccountChanged;
            _engine.AccountManager.AccountChanging.onBackgroundThread -= OnAccountChanging;
        }

        public Job<List<byte>> GetGameSaves()
        {
            return JobLocker(() => GetGameSavesJob());
        }

        public Job LoadGameSave(byte gameSave)
        {
            AssertGameSave(gameSave);
            
            return GameSaveJobLocker(gameSave, PlayerDataJob.Type.Loading, () => LoadGameSaveJob(gameSave));
        }

        public Job UnloadGameSave(byte gameSave, bool saveBeforeUnload = false)
        {
            AssertGameSave(gameSave);
            
            return GameSaveJobLocker(gameSave, PlayerDataJob.Type.Unloading, () => UnloadGameSaveJob(gameSave, saveBeforeUnload));
        }

        public Job DeleteGameSave(byte gameSave)
        {
            AssertGameSave(gameSave);
            
            return GameSaveJobLocker(gameSave, PlayerDataJob.Type.Deleting, () => DeleteGameSaveJob(gameSave));
        }

        public GameSaveState GetGameSaveState(byte gameSave)
        {
            AssertGameSave(gameSave);

            // Check if gamesave is loaded / loading / unloading / deleting / saving / syncing
            PlayerDataJob job;
            lock (_gameSaveJobs)
            {
                if (_gameSaveJobs.TryGetValue(gameSave, out job))
                {
                    switch (job.type)
                    {
                        case PlayerDataJob.Type.Loading:
                            return GameSaveState.Loading;
                        case PlayerDataJob.Type.Saving:
                            return GameSaveState.Saving;
                        case PlayerDataJob.Type.Unloading:
                            return GameSaveState.Unloading;
                        case PlayerDataJob.Type.Deleting:
                            return GameSaveState.Deleting;
                        case PlayerDataJob.Type.Syncing:
                            return GameSaveState.Syncing;
                        case PlayerDataJob.Type.Duplicating:
                            return GameSaveState.Duplicating;
                    }
                }
            }
            
            if (_gameSaves.ContainsKey(gameSave))
            {
                return GameSaveState.Loaded;
            }

            // Check if file exists
            if (!_engine.PersistanceManager.Exists(gameSave))
            {
                return GameSaveState.Inexistant;
            }

            return GameSaveState.Unloaded;
        }

        public void Add(GDFPlayerData data)
        {
            Add(ActiveGameSave, data);
        }
        public void Add(byte gameSave, GDFPlayerData data)
        {
            Add(gameSave, null, data);
        }
        public void Add(string reference, GDFPlayerData data)
        {
            Add(ActiveGameSave, reference, data);
        }
        public void Add(byte gameSave, string reference, GDFPlayerData data)
        {
            AssertGameSave(gameSave);
            
            using(_lock.Use(_engine))
            {
                PlayerDataGameSave gs = GetGameSaveCache(gameSave);
                gs.Add(reference, data);
            }
        }

        public GDFPlayerData Get(string reference)
        {
            return Get(ActiveGameSave, reference);
        }
        public GDFPlayerData Get(byte gameSave, string reference)
        {
            AssertGameSave(gameSave);
            
            PlayerDataGameSave gs = GetGameSaveCache(gameSave);
            return gs.Get(reference);
        }
        public GDFPlayerData Get(Type type, string reference)
        {
            return Get(ActiveGameSave, type, reference);
        }
        public GDFPlayerData Get(byte gameSave, Type type, string reference)
        {
            AssertGameSave(gameSave);
            
            PlayerDataGameSave gs = GetGameSaveCache(gameSave);
            return gs.Get(type, reference);
        }
        public T Get<T>(string reference) where T : GDFPlayerData
        {
            return Get<T>(ActiveGameSave, reference);
        }
        public T Get<T>(byte gameSave, string reference) where T : GDFPlayerData
        {
            AssertGameSave(gameSave);
            
            PlayerDataGameSave gs = GetGameSaveCache(gameSave);
            return gs.Get<T>(reference);
        }
        public List<GDFPlayerData> Get(Type type, bool includeInherited = true)
        {
            return Get(ActiveGameSave, type, includeInherited);
        }
        public List<GDFPlayerData> Get(byte gameSave, Type type, bool includeInherited = true)
        {
            AssertGameSave(gameSave);
            
            PlayerDataGameSave gs = GetGameSaveCache(gameSave);
            return gs.Get(type, includeInherited);
        }
        public List<T> Get<T>(bool includeInherited = true) where T : GDFPlayerData
        {
            return Get<T>(ActiveGameSave, includeInherited);
        }
        public List<T> Get<T>(byte gameSave, bool includeInherited = true) where T : GDFPlayerData
        {
            AssertGameSave(gameSave);
            
            PlayerDataGameSave gs = GetGameSaveCache(gameSave);
            return gs.Get<T>(includeInherited);
        }

        public DataState GetDataState(GDFPlayerData data)
        {
            PlayerDataGameSave gs;
            if (!_gameSaves.TryGetValue(data.GameSave, out gs))
            {
                return new DataState
                {
                    state = DataState.State.Unknown
                };
            }

            return gs.GetDataState(data);
        }

        public bool HasDataToSave()
        {
            foreach (PlayerDataGameSave gameSave in _gameSaves.Values)
            {
                if (gameSave.HasDataToSave)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasDataToSave(byte gameSave)
        {
            AssertGameSave(gameSave);

            if(_gameSaves.TryGetValue(gameSave, out PlayerDataGameSave gs))
            {
                return gs.HasDataToSave;
            }

            return false;
        }

        public int DataToSaveAmount()
        {
            int amount = 0;
            foreach (PlayerDataGameSave gameSave in _gameSaves.Values)
            {
                amount += gameSave.DataToSaveAmount;
            }
            return amount;
        }

        public int DataToSaveAmount(byte gameSave)
        {
            AssertGameSave(gameSave);

            if(_gameSaves.TryGetValue(gameSave, out PlayerDataGameSave gs))
            {
                return gs.DataToSaveAmount;
            }
            
            return 0;
        }


        public void AddToSaveQueue(GDFPlayerData data)
        {
            using(_lock.Use(_engine))
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                PlayerDataGameSave gs = GetGameSaveCache(data.GameSave);
                gs.AddToSaveQueue(data);
            }
        }

        public void Delete(GDFPlayerData data)
        {
            using(_lock.Use(_engine))
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                PlayerDataGameSave gs = GetGameSaveCache(data.GameSave);
                gs.Delete(data);
            }
        }

        public Job Save()
        {
            List<PlayerDataGameSave> gameSaves = _gameSaves.Values.ToList();
            
            return GameSaveJobLocker(gameSaves, PlayerDataJob.Type.Saving, () => SaveGameSaveJob(gameSaves));
        }

        public Job Save(byte gameSave)
        {
            AssertGameSave(gameSave);
            
            return GameSaveJobLocker(gameSave, PlayerDataJob.Type.Saving, () => SaveGameSaveJob(gameSave));
        }
        
        public Job Sync()
        {
            List<PlayerDataGameSave> gameSaves = _gameSaves.Values.ToList();
            
            return GameSaveJobLocker(gameSaves, PlayerDataJob.Type.Syncing, () => SyncJob(gameSaves));
        }

        public Job Purge()
        {
            List<PlayerDataGameSave> gameSaves = _gameSaves.Values.ToList();
            
            return GameSaveJobLocker(gameSaves, PlayerDataJob.Type.Deleting, () => Job.Run(PurgeRunner, "Purge data"));
        }

        public Job MigrateOnline()
        {
            string jobName = "Migrate from Local to Online";
            
            if (_engine.AccountManager.IsLocal)
            {
                throw Exceptions.InvalidMigrationAccount;
            }

            if (!_engine.AccountManager.Authentication.Local.Exists)
            {
                throw Exceptions.NoMigrationLocalDataFound;
            }

            return JobLocker(() => Job.Run(OnlineMigrationRunner, jobName));
        }
        
        public Job MigrateOffline()
        {
            string jobName = "Migrate from Online to Local";
            
            if (_engine.AccountManager.IsLocal)
            {
                throw Exceptions.InvalidMigrationAccount;
            }

            return JobLocker(() => Job.Run(OfflineMigrationRunner, jobName));
        }
        
        public Job DuplicateGameSave(byte srcGameSave, byte dstGameSave)
        {
            AssertGameSave(srcGameSave);
            AssertGameSave(dstGameSave);

            return GameSaveJobLocker(new List<byte> {srcGameSave, dstGameSave}, PlayerDataJob.Type.Duplicating, () => DuplicateGameSaveJob(srcGameSave, dstGameSave));
        }

        public T Get<T>(IJobHandler handler, string url)
        {
            _headers.Clear();
            _engine.ServerManager.FillHeaders(_headers, _engine.AccountManager.Bearer);

            return Get<T>(handler.Split(), url, _headers);
        } 

        public void Post(IJobHandler handler, string url, object payload)
        {
            _headers.Clear();
            _engine.ServerManager.FillHeaders(_headers, _engine.AccountManager.Bearer);

            Post(handler.Split(), url, _headers, payload);
        } 

        public void Delete(IJobHandler handler, string url)
        {
            _headers.Clear();
            _engine.ServerManager.FillHeaders(_headers, _engine.AccountManager.Bearer);

            Delete(handler.Split(), url, _headers);
        } 

        private Job<List<byte>> GetGameSavesJob()
        {
            return Job<List<byte>>.Run(handler =>
            {
                using(_lock.Use(_engine))
                {
                    return _engine.PersistanceManager.GetGameSaves(handler);
                }
            }, $"Get gamesaves");
        }

        private Job LoadGameSaveJob(byte gameSave)
        {
            return Job.Run(handler =>
            {
                using(_lock.Use(_engine))
                {
                    LoadRunnerUnsafe(handler, gameSave);
                }
            }, $"Load gamesave {gameSave}");
        }

        private void LoadRunnerUnsafe(IJobHandler handler, byte gameSave)
        {
            handler.StepAmount = 5;

            Loading.Invoke(handler.Split(), gameSave);
            try
            {
                if (_information == null)
                {
                    _information = _engine.PersistanceManager.LoadInformation(handler.Split());
                }
                else
                {
                    handler.Step();
                }

                if (!_syncDataLoaded)
                {
                    _syncQueue.Clear();
                    _dataToSyncLoadBuffer.Clear();
                    _engine.PersistanceManager.LoadDataToSync(handler.Split(), _dataToSyncLoadBuffer);
                    foreach (GDFPlayerDataStorage storage in _dataToSyncLoadBuffer)
                    {
                        storage.Initialize(_engine);
                        _syncQueue.Add(storage);
                    }
                    _syncDataLoaded = true;
                }
                else
                {
                    handler.Step();
                }

                PlayerDataGameSave gs;
                if (!_gameSaves.TryGetValue(gameSave, out gs))
                {
                    gs = new PlayerDataGameSave(this, gameSave);
                    _gameSaves[gameSave] = gs;
                }

                gs.Load(handler.Split());
            }
            finally
            {
                lock (_gameSaveJobs)
                {
                    _gameSaveJobs.Remove(gameSave);
                }
                
                Loaded.Invoke(handler.Split(), gameSave);
            }
        }

        private Job UnloadGameSaveJob(byte gameSave, bool saveBeforeUnload)
        {
            return Job.Run(handler =>
            {
                using(_lock.Use(_engine))
                {
                    UnloadRunnerUnsafe(handler, gameSave, saveBeforeUnload);
                }
            }, $"Unload gamesave {gameSave}");
        }

        private void UnloadRunnerUnsafe(IJobHandler handler, byte gameSave, bool saveBeforeUnload)
        {
            handler.StepAmount = 4;

            Unloading.Invoke(handler.Split(), gameSave);
            try
            {
                PlayerDataGameSave gs;
                if (!_gameSaves.TryGetValue(gameSave, out gs))
                {
                    return;
                }
                
                if (saveBeforeUnload)
                {
                    SaveRunnerUnsafe(handler.Split(), gs);
                }
                
                _gameSaves.Remove(gameSave, out gs);
            }
            finally
            {
                lock (_gameSaveJobs)
                {
                    _gameSaveJobs.Remove(gameSave);
                }
                Unloaded.Invoke(handler.Split(), gameSave);
            }
        }

        private Job DeleteGameSaveJob(byte gameSave)
        {
            return Job.Run(handler =>
            {
                using(_lock.Use(_engine))
                {
                    DeleteRunnerUnsafe(handler, gameSave);
                }
            }, $"Delete gamesave {gameSave}");
        }

        private void DeleteRunnerUnsafe(IJobHandler handler, byte gameSave)
        {
            handler.StepAmount = 5;

            Deleting.Invoke(handler.Split(), gameSave);
            try
            {
                List<GDFPlayerDataStorage> dataToSync = new List<GDFPlayerDataStorage>();
                UnloadRunnerUnsafe(handler.Split(), gameSave, false);
                _engine.PersistanceManager.Load(handler.Split(), gameSave, dataToSync);

                foreach (GDFPlayerDataStorage storage in dataToSync)
                {
                    storage.Initialize(_engine, gameSave);
                    storage.Trashed = true;
                    _syncQueue.AddOrUpdate(storage);
                }

                handler.Step();

                _engine.PersistanceManager.SaveDataToSync(handler.Split(), dataToSync);
                _engine.PersistanceManager.Purge(handler.Split(), gameSave);
            }
            finally
            {
                lock (_gameSaveJobs)
                {
                    _gameSaveJobs.Remove(gameSave);
                }
                Deleted.Invoke(handler.Split(), gameSave);
            }
        }

        private Job SaveGameSaveJob(byte gameSave)
        {
            return Job.Run(handler =>
            {
                using(_lock.Use(_engine))
                {
                    PlayerDataGameSave gs = GetGameSaveCache(gameSave);
                    SaveRunnerUnsafe(handler, gs);
                }

            }, $"Save gamesave {gameSave}");
        }

        private void SaveRunnerUnsafe(IJobHandler handler, PlayerDataGameSave gs)
        {
            handler.StepAmount = 3;
            Saving.Invoke(handler.Split(), gs.GameSave);
            try
            {
                List<GDFPlayerDataStorage> storages = gs.Save(handler.Split());
                foreach (GDFPlayerDataStorage data in storages)
                {
                    _syncQueue.Add(data);
                }
            }
            finally
            {
                lock (_gameSaveJobs)
                {
                    _gameSaveJobs.Remove(gs.GameSave);
                }
                
                Saved.Invoke(handler.Split(), gs.GameSave);
            }
        }

        private Job SaveGameSaveJob(List<PlayerDataGameSave> gameSaves)
        {
            return Job.Run(handler =>
            {
                using(_lock.Use(_engine))
                {
                    SaveRunnerUnsafe(handler, gameSaves);
                }
            }, $"Save gamesave");
        }

        private void SaveRunnerUnsafe(IJobHandler handler, List<PlayerDataGameSave> gameSaves)
        {
            handler.StepAmount = gameSaves.Count + 3;

            foreach (PlayerDataGameSave gameSave in gameSaves)
            {
                Saving.Invoke(handler.Split(), gameSave.GameSave);
            }
            try
            {
                
                foreach (PlayerDataGameSave gameSave in gameSaves)
                {
                    try
                    {
                        SaveRunnerUnsafe(handler.Split(), gameSave);
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }

                _engine.PersistanceManager.SaveDataToSync(handler.Split(), _syncQueue.ToList());
                
            }
            finally
            {
                lock (_gameSaveJobs)
                {
                    _gameSaveJobs.Clear();
                }

                foreach (PlayerDataGameSave gameSave in gameSaves)
                {
                    Saved.Invoke(handler.Split(), gameSave.GameSave);
                }
            }
        }

        private Job SyncJob(List<PlayerDataGameSave> gameSaves)
        {
            return Job.Run(handler =>
            {
                using(_lock.Use(_engine))
                {
                    handler.StepAmount = 4;
                    Syncing.Invoke(handler.Split());
                    try
                    {
                        SaveRunnerUnsafe(handler.Split(), gameSaves);
                        if (_engine.AccountManager.IsLocal)
                        {
                            handler.Step();
                            return;
                        }
                        SyncRunnerUnsafe(handler.Split());
                    }
                    finally
                    {
                        lock (_gameSaveJobs)
                        {
                            _gameSaveJobs.Clear();
                        }
                
                        Synced.Invoke(handler.Split());
                    }
                }
            }, $"Sync player data");
        }

        private void SyncRunnerUnsafe(IJobHandler handler)
        {
            handler.StepAmount = 2;

            SendToServerUnsafe(handler.Split());
            GetFromServerUnsafe(handler.Split());
        }

        private void SendToServerUnsafe(IJobHandler handler)
        {
            PlayerDataExchange exchange = new PlayerDataExchange();

            handler.StepAmount = (_syncQueue.Count / GDFConstants.K_STORAGE_SYNC_LIMIT + 1) * 2;

            while (_syncQueue.Count > 0)
            {
                exchange.Storages.Clear();

                _syncQueue.Fill(exchange.Storages, GDFConstants.K_STORAGE_SYNC_LIMIT);

                try
                {
                    Post(handler.Split(), _engine.ServerManager.BuildSyncURL("/api/v1/player-data"), exchange);
                }
                catch (APIException e)
                {
                    GDFLogger.Error(e);
                    throw;
                }

                _syncQueue.Remove(exchange.Storages);

                _engine.PersistanceManager.RemoveDataToSync(handler.Split(), exchange.Storages);
            }
        }

        private void GetFromServerUnsafe(IJobHandler handler)
        {
            PlayerDataPagination pagination;
            
            IJobHandler rootHandler = handler;
            handler.StepAmount = 2;

            do
            {
                handler = handler.Split();
                handler.StepAmount = 4;

                pagination = Get<PlayerDataPagination>(handler.Split(), _engine.ServerManager.BuildSyncURL("/api/v1/player-data/" + _information.SyncDate.ToISO8601().ToBase64URL()));

                if (pagination.Items.Count == 0)
                {
                    return;
                }

                foreach (GDFPlayerDataStorage playerStorage in pagination.Items)
                {
                    _syncQueue.Remove(playerStorage);
                    if (_gameSaves.TryGetValue(playerStorage.GameSave, out PlayerDataGameSave gs))
                    {
                        gs.SyncStorage(playerStorage);
                    }

                    if (_information.SyncDate < playerStorage.SyncDateTime)
                    {
                        _information.SyncDate = playerStorage.SyncDateTime;
                    }
                }

                _engine.PersistanceManager.Save(handler.Split(), pagination.Items);
                
            } while (pagination.Items.Count != pagination.Total);

            _engine.PersistanceManager.SaveInformation(rootHandler.Split(), _information);
        }

        private void OnlineMigrationRunner(IJobHandler handler)
        {
            handler.StepAmount = _gameSaves.Count + 2;
            using (_lock.Use(_engine))
            {
                _engine.PersistanceManager.MigrateOnline(handler.Split());
                PurgeAPICall(handler.Split());
                foreach (byte gameSave in _gameSaves.Keys)
                {
                    LoadRunnerUnsafe(handler.Split(), gameSave);
                }
            }
        }

        private void OfflineMigrationRunner(IJobHandler handler)
        {
            using (_lock.Use(_engine))
            {
                _engine.PersistanceManager.MigrateOffline(handler);
            }
        }

        private void PurgeRunner(IJobHandler handler)
        {
            using(_lock.Use(_engine))
            {
                try
                {
                    handler.StepAmount = 2;

                    _gameSaves.Clear();
                    _syncQueue.Clear();
                    _information.SyncDate = DateTime.MinValue;

                    _engine.PersistanceManager.Purge(handler.Split());

                    if (_engine.AccountManager.IsLocal)
                    {
                        return;
                    }
                    
                    PurgeAPICall(handler.Split());
                }
                finally
                {
                    lock (_gameSaveJobs)
                    {
                        _gameSaveJobs.Clear();
                    }
                }
            }
        }

        private Job DuplicateGameSaveJob(byte srcGameSave, byte dstGameSave)
        {
            return Job.Run(handler =>
            {
                using(_lock.Use(_engine))
                {
                    DuplicateRunnerUnsafe(handler, srcGameSave, dstGameSave);
                }
            }, $"Duplicate gamesave {srcGameSave} to {dstGameSave}");
        }

        private void DuplicateRunnerUnsafe(IJobHandler handler, byte srcGameSave, byte dstGameSave)
        {
            handler.StepAmount = 4;

            Duplicating.Invoke(handler.Split(), srcGameSave, dstGameSave);
            try
            {
                GameSaveStateFlag srcState = GetGameSaveStateFlag(srcGameSave, out PlayerDataGameSave srcGS);
                GameSaveStateFlag dstState = GetGameSaveStateFlag(dstGameSave, out PlayerDataGameSave dstGS);

                if (srcState == GameSaveStateFlag.None)
                {
                    throw Exceptions.GameSaveNotExisting(srcGameSave);
                }
                
                List<GDFPlayerDataStorage> storages = new List<GDFPlayerDataStorage>();
                _dataToSyncLoadBuffer.Clear();
                
                if (dstState.HasFlag(GameSaveStateFlag.Loaded))
                {
                    // Update dst cache
                    if (srcState.HasFlag(GameSaveStateFlag.Loaded))
                    {
                        // Update from src gamesave cache
                        dstGS.CopyFrom(handler.Split(), srcGS, _dataToSyncLoadBuffer, dstState.HasFlag(GameSaveStateFlag.Existant));
                    }
                    else
                    {
                        // Update from src file.
                        _engine.PersistanceManager.Load(handler.Split(), srcGameSave, storages);
                        dstGS.CopyFrom(handler.Split(), storages, _dataToSyncLoadBuffer, dstState.HasFlag(GameSaveStateFlag.Existant));
                    }
                }
                else if (srcState.HasFlag(GameSaveStateFlag.Loaded))
                {
                    // Duplicate src SaveQueue in the buffer with the right gameSave
                    srcGS.ExtractStorages(_dataToSyncLoadBuffer);
                }
                else
                {
                    // Load the storages and fill the missing information and add it to the buffer
                    _engine.PersistanceManager.Load(handler.Split(), srcGameSave, _dataToSyncLoadBuffer);
                    foreach (GDFPlayerDataStorage storage in _dataToSyncLoadBuffer)
                    {
                        storage.Initialize(_engine, dstGameSave);
                    }
                }
                
                // Update sync queue
                foreach (GDFPlayerDataStorage storage in _dataToSyncLoadBuffer)
                {
                    storage.Initialize(_engine, dstGameSave);
                    _syncQueue.Add(storage);
                }

                if (dstState.HasFlag(GameSaveStateFlag.Existant))
                {
                    // Delete dst save file
                    _engine.PersistanceManager.Purge(handler.Split(), dstGameSave);
                }

                if (dstState.HasFlag(GameSaveStateFlag.Loaded))
                {
                    // Normal dst GameSave Save
                    dstGS.Save(handler.Split());
                }
                else if (srcState.HasFlag(GameSaveStateFlag.Loaded))
                {
                    // Special src gamesave save in dst file
                    // Should not change the saveQueue of src
                    dstGS.SaveTo(handler.Split(), dstGameSave);
                }
                else
                {
                    // Copy src file to dst
                    _engine.PersistanceManager.Copy(handler.Split(), srcGameSave, dstGameSave);
                }

                // Save sync queue (delta only)
                _engine.PersistanceManager.SaveDataToSync(handler.Split(), _dataToSyncLoadBuffer);
            }
            finally
            {
                lock (_gameSaveJobs)
                {
                    _gameSaveJobs.Remove(srcGameSave);
                    _gameSaveJobs.Remove(dstGameSave);
                }
                Duplicated.Invoke(handler.Split(), srcGameSave, dstGameSave);
            }
        }

        private GameSaveStateFlag GetGameSaveStateFlag(byte gameSave, out PlayerDataGameSave gs)
        {
            GameSaveStateFlag state = GameSaveStateFlag.None;
            if (_engine.PersistanceManager.Exists(gameSave))
            {
                state |= GameSaveStateFlag.Existant;
            }

            if (_gameSaves.TryGetValue(gameSave, out gs))
            {
                state |= GameSaveStateFlag.Loaded;
            }
            return state;
        }

        private void PurgeAPICall(IJobHandler handler)
        {
            _headers.Clear();
            _engine.ServerManager.FillHeaders(_headers, _engine.AccountManager.Bearer);
            Delete(handler, _engine.ServerManager.BuildSyncURL("/api/v1/player-data"), _headers);
        }

        private void OnAccountChanging(IJobHandler handler, MemoryJwtToken token)
        {
            using(_lock.Use(_engine))
            {
                if (token == null)
                {
                    return;
                }
                
                _gameSaveJobs.Clear();
                _gameSaves.Clear();
                _syncQueue.Clear();
            }
        }

        private void OnAccountChanged(IJobHandler handler, MemoryJwtToken token)
        {
            using(_lock.Use(_engine))
            {
                if (token == null)
                {
                    return;
                }

                _gameSaveJobs.Clear();
                _gameSaves.Clear();
                _syncQueue.Clear();
                _syncDataLoaded = false;

                ActiveGameSave = 0;
                
                LoadRunnerUnsafe(handler, ActiveGameSave);
            }
        }

        private void AssertGameSave(byte gameSave)
        {
            if (gameSave > 99)
            {
                throw Exceptions.InvalidGameSave;
            }
        }

        private Job GameSaveJobLocker(List<byte> gameSaves, PlayerDataJob.Type type, Func<Job> body)
        {
            return GameSaveJobLocker(gameSaves, x => x, type, body);
        }

        private Job GameSaveJobLocker(byte gameSave, PlayerDataJob.Type type, Func<Job> body)
        {
            lock (_gameSaveJobs)
            {
                if (_gameSaveJobs.TryGetValue(gameSave, out PlayerDataJob job))
                {
                    if (job.type == type)
                    {
                        return job.job;
                    }
                    
                    throw Exceptions.GameSaveBusy(gameSave, job.type);
                }
                
                job = new PlayerDataJob();
                _gameSaveJobs.Add(gameSave, job);
                job.type = type;
                job.job = JobLocker(body);
                
                return job.job;
            }
        }

        private Job GameSaveJobLocker(List<PlayerDataGameSave> gameSaves, PlayerDataJob.Type type, Func<Job> body)
        {
            return GameSaveJobLocker(gameSaves, x => x.GameSave, type, body);
        }

        private Job GameSaveJobLocker<T>(List<T> gameSaves, Func<T, byte> converter, PlayerDataJob.Type type, Func<Job> body)
        {
            PlayerDataJob job;
            lock (_gameSaveJobs)
            {
                int i = gameSaves.Count - 1;
                try
                {
                    byte gameSave;
                    for (i = gameSaves.Count - 1; i >= 0; i--)
                    {
                        gameSave = converter.Invoke(gameSaves[i]);
                        if (_gameSaveJobs.TryGetValue(gameSave, out job))
                        {
                            if (job.type == type)
                            {
                                return job.job;
                            }

                            throw Exceptions.GameSaveBusy(gameSave, job.type);
                        }
                    }

                    job = new PlayerDataJob();
                    job.type = type;
                    for (i = gameSaves.Count - 1; i >= 0; i--)
                    {
                        _gameSaveJobs.Add(converter.Invoke(gameSaves[i]), job);
                    }
                    job.job = JobLocker(body);
                }
                catch
                {
                    for (i = i + 1; i < gameSaves.Count; i++)
                    {
                        _gameSaveJobs.Remove(converter.Invoke(gameSaves[i]));
                    }

                    throw;
                }
            }

            return job.job;
        }

        private PlayerDataGameSave GetGameSaveCache(byte gameSave)
        {
            if (!_gameSaves.TryGetValue(gameSave, out PlayerDataGameSave gs))
            {
                throw Exceptions.GameSaveNotLoaded(gameSave);
            }

            return gs;
        }

    }
}