using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GDFFoundation;
using GDFRuntime;
using Newtonsoft.Json;

namespace GDFUnity
{
    [Dependency(typeof(IRuntimeConfigurationEngine), typeof(IRuntimeAccountManager))]
    [JobLockers(typeof(IRuntimeAccountManager), typeof(IRuntimePlayerPersistanceManager))]
    public class RuntimePlayerDataManager : APIManager, IRuntimePlayerDataManager
    {
        static private class Exceptions
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
        }

        private class Lock : IDisposable
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

        private class Cache
        {
            internal Dictionary<string, GDFPlayerData> data;
            private RuntimePlayerDataManager _manager;
            private Dictionary<string, GDFPlayerDataStorage> _storage;

            public int Count => _storage.Count;
            public IEnumerable<string> KeysUnsafe => _storage.Keys;

            private Lock _Lock => _manager._lock;
            private IRuntimeEngine _Engine => _manager._engine;


            public Cache(RuntimePlayerDataManager manager)
            {
                _manager = manager;
                _storage = new Dictionary<string, GDFPlayerDataStorage> ();
                data = new Dictionary<string, GDFPlayerData>();
            }

            public bool Contains(string reference)
            {
                using(_Lock.Use(_Engine))
                {
                    return _storage.ContainsKey(reference);
                }
            }

            public void Add(string reference, GDFPlayerDataStorage storage, GDFPlayerData data)
            {
                using(_Lock.Use(_Engine))
                {
                    _storage.Add(reference, storage);
                    this.data.Add(reference, data);
                }
            }
            
            public void Add(string reference, GDFPlayerData data)
            {
                using(_Lock.Use(_Engine))
                {
                    this.data.Add(reference, data);
                }
            }
            
            public void Add(string reference, GDFPlayerDataStorage storage)
            {
                using(_Lock.Use(_Engine))
                {
                    _storage.Add(reference, storage);
                }
            }

            public GDFPlayerData Get(string reference)
            {
                GDFPlayerData data;
                using (_Lock.Use(_Engine))
                {
                    if (this.data.TryGetValue(reference, out data))
                    {
                        return data;
                    }

                    if (_storage.TryGetValue(reference, out GDFPlayerDataStorage storage))
                    {
                        data = Deserialize(storage);
                        this.data.Add(reference, data);
                    }
                    return data;
                }
            }

            public bool TryGetStorage(string reference, out GDFPlayerDataStorage storage)
            {
                storage = null;
                using (_Lock.Use(_Engine))
                {
                    return _storage.TryGetValue(reference, out storage);
                }
            }
            
            public List<GDFPlayerDataStorage> GetStorages()
            {
                return _storage.Values.ToList();
            }
            
            public GDFPlayerDataStorage GetStorage(string reference)
            {
                GDFPlayerDataStorage storage;
                using (_Lock.Use(_Engine))
                {
                    _storage.TryGetValue(reference, out storage);
                    return storage;
                }
            }

            public GDFPlayerData Update(GDFPlayerDataStorage storage)
            {
                if (!Contains(storage.Reference))
                {
                    Add(storage.Reference, storage);
                    return null;
                }

                GDFPlayerData data;
                using (_Lock.Use(_Engine))
                {
                    if (this.data.TryGetValue(storage.Reference, out data))
                    {
                        JsonConvert.PopulateObject(storage.Json, data);
                        return data;
                    }
                    data = Deserialize(storage);
                    this.data.Add(storage.Reference, data);
                    return data;
                }
            }

            public void Update(GDFPlayerData data)
            {
                if (!Contains(data.Reference))
                {
                    return;
                }

                using (_Lock.Use(_Engine))
                {
                    if (this.data.ContainsKey(data.Reference))
                    {
                        this.data[data.Reference] = data;
                    }
                    else
                    {
                        this.data.Add(data.Reference, data);
                    }
                }
            }

            public bool Remove(string reference)
            {
                using (_Lock.Use(_Engine))
                {
                    data.Remove(reference);
                    return _storage.Remove(reference);
                }
            }

            public void Clear()
            {
                using(_Lock.Use(_Engine))
                {
                    _storage.Clear();
                    data.Clear();
                }
            }
            
            private GDFPlayerData Deserialize(GDFPlayerDataStorage storage)
            {
                GDFPlayerData data = JsonConvert.DeserializeObject(storage.Json, _Engine.TypeManager.GetType(storage.ClassName)) as GDFPlayerData;
                data.ProtectedFill(storage);
                data.Channels = storage.Channels;
                data.Trashed = storage.Trashed;
                return data;
            }
        }

        private struct Unicity
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

        private class Queue : IEnumerable<GDFPlayerDataStorage>
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
                GDFPlayerDataStorage value;
                if (_storages.TryGetValue(unicity, out value))
                {
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

        private readonly Lock _lock = new Lock();
        private byte _gameSave = 0;
        private IRuntimeEngine _engine;
        private Dictionary<string, PlayerReferenceStorage> _references = new Dictionary<string, PlayerReferenceStorage>();
        private Cache _commonCache;
        private Cache _gameSaveCache;
        private List<GDFPlayerDataStorage> _loadDataBuffer = new List<GDFPlayerDataStorage>();
        private List<GDFPlayerDataStorage> _loadDataToSyncBuffer = new List<GDFPlayerDataStorage>();
        private List<PlayerReferenceStorage> _referenceSaveQueue = new List<PlayerReferenceStorage>();
        private Queue _saveQueue = new Queue();
        private Queue _syncQueue = new Queue();
        private Random _rng = new Random();
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        private PlayerStorageInformation _information = new PlayerStorageInformation();
        private Notification _syncing;
        private Notification _synced;
        private Notification _loading;
        private Notification _loaded;
        private Notification _saving;
        private Notification _saved;

        public byte GameSave => _gameSave;
        public bool HasDataToSave => _saveQueue.Count != 0;
        public bool HasDataToSync => _syncQueue.Count != 0;
        public Notification Syncing => _syncing;
        public Notification Synced => _synced;
        public Notification Loading => _loading;
        public Notification Loaded => _loaded;
        public Notification Saving => _saving;
        public Notification Saved => _saved;

        protected override Type JobLokerType => typeof(IRuntimePlayerDataManager);

        public RuntimePlayerDataManager(IRuntimeEngine engine)
        {
            _engine = engine;

            _syncing = new Notification(engine.ThreadManager);
            _synced = new Notification(engine.ThreadManager);
            _loading = new Notification(engine.ThreadManager);
            _loaded = new Notification(engine.ThreadManager);
            _saving = new Notification(engine.ThreadManager);
            _saved = new Notification(engine.ThreadManager);

            _engine.AccountManager.AccountChanging.onBackgroundThread += OnAccountChanging;
            _engine.AccountManager.AccountChanged.onBackgroundThread += OnAccountChanged;
            _engine.AccountManager.AccountDeleting.onBackgroundThread += PurgeRunner;

            _gameSaveCache = new Cache(this);
            _commonCache = new Cache(this);
        }

        ~RuntimePlayerDataManager()
        {
            _engine.AccountManager.AccountDeleting.onBackgroundThread -= PurgeRunner;
            _engine.AccountManager.AccountChanged.onBackgroundThread -= OnAccountChanged;
            _engine.AccountManager.AccountChanging.onBackgroundThread -= OnAccountChanging;
        }

        public Job LoadCommonGameSave()
        {
            return LoadGameSave(0);
        }

        public Job LoadGameSave(byte gameSave)
        {
            if (gameSave > 99)
            {
                throw Exceptions.InvalidGameSave;
            }

            return JobLocker(() => LoadJob(gameSave));
        }

        public bool GameSaveExists(byte gameSave)
        {
            return _information.GameSaves.Contains(gameSave);
        }

        public void Add(GDFPlayerData data, bool defaultGameSave)
        {
            Add(null, data, defaultGameSave);
        }

        public void Add(string reference, GDFPlayerData data, bool defaultGameSave)
        {
            using(_lock.Use(_engine))
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                if (data.Trashed)
                {
                    throw Exceptions.CannotAddTrashed;
                }

                if (data.Reference != null)
                {
                    throw Exceptions.DataAlreadyExists;
                }

                if (data.Channels == 0)
                {
                    data.Channels = _engine.Configuration.Channel;
                }

                Cache cache = _gameSave == 0 ? _commonCache : _gameSaveCache;
                if (string.IsNullOrWhiteSpace(reference))
                {
                    reference = GenerateReference(cache, data.GetType(), data.Channels);
                }

                reference = reference.Trim();
                if (cache.Contains(reference))
                {
                    throw Exceptions.DataAlreadyExists;
                }
                

                string className = _engine.TypeManager.GetClassName(data.GetType());
                if (_references.TryGetValue(reference, out PlayerReferenceStorage playerReference) && playerReference.Classname != className)
                {
                    throw Exceptions.InvalidTypeForReference;
                }
                
                GDFPlayerDataStorage storage = new GDFPlayerDataStorage();
                storage.Reference = reference;
                storage.Creation = GDFDatetime.Now;
                storage.Modification = storage.Creation;
                storage.GameSave = _gameSave;
                storage.Project = _engine.Configuration.Reference;
                storage.Account = _engine.AccountManager.Reference;
                storage.Range = _engine.AccountManager.Range;
                storage.Channels = data.Channels;
                storage.Trashed = data.Trashed;
                
                data.ProtectedFill(storage);

                string json = Serialize(data);
                storage.ClassName = _engine.TypeManager.GetClassName(data.GetType());
                storage.Json = json;

                RegisterReference(data.Reference, storage.GameSave, storage.ClassName);

                cache.Add(data.Reference, storage, data);
                
                _saveQueue.Add(storage);
            }
        }

        public GDFPlayerData Get(string reference, bool defaultGameSave)
        {
            return FromCache(reference, defaultGameSave);
        }

        public GDFPlayerData Get(Type type, string reference, bool defaultGameSave)
        {
            if (!_engine.TypeManager.Is(type, typeof(GDFPlayerData)))
            {
                throw Exceptions.InvalidTypeForReference;
            }

            GDFPlayerData data = FromCache(reference, defaultGameSave);
            if (data == null)
            {
                return null;
            }

            if (!_engine.TypeManager.Is(data.GetType(), type))
            {
                return null;
            }

            return data;
        }

        public T Get<T>(string reference, bool defaultGameSave) where T : GDFPlayerData
        {
            return FromCache(reference, defaultGameSave) as T;
        }

        public List<GDFPlayerData> Get(Type type, bool includeInherited, bool defaultGameSave)
        {
            if (!_engine.TypeManager.Is(type, typeof(GDFPlayerData)))
            {
                throw Exceptions.InvalidTypeForReference;
            }
            List<GDFPlayerDataStorage> storages = Iterate(type, includeInherited, defaultGameSave);
            List<GDFPlayerData> list = new List<GDFPlayerData>();
            foreach (GDFPlayerDataStorage storage in storages)
            {
                GDFPlayerData item = FromCache(storage.Reference, defaultGameSave);
                if (item == null)
                {
                    continue;
                }
                list.Add(item);
            }
            return list;
        }

        public List<T> Get<T>(bool includeInherited, bool defaultGameSave) where T : GDFPlayerData
        {
            List<GDFPlayerDataStorage> storages = Iterate(typeof(T), includeInherited, defaultGameSave);
            List<T> list = new List<T>();
            foreach (GDFPlayerDataStorage storage in storages)
            {
                T item = FromCache(storage.Reference, defaultGameSave) as T;
                if (item == null)
                {
                    continue;
                }
                list.Add(item);
            }
            return list;
        }

        public void AddToSaveQueue(GDFPlayerData data)
        {
            using(_lock.Use(_engine))
            {
                if (data == null)
                {
                    throw new ArgumentNullException(nameof(data));
                }

                if (string.IsNullOrWhiteSpace(data.Reference))
                {
                    throw Exceptions.InvalidReference;
                }
                
                Cache cache = data.GameSave == 0 ? _commonCache : _gameSaveCache;
                GDFPlayerDataStorage storage = FromStorageCache(data.Reference, data.GameSave == 0);
                if (storage == null)
                {
                    throw Exceptions.NotFoundInCache;
                }

                storage.Trashed = data.Trashed;
                storage.Modification = GDFDatetime.Now;

                if (data.Trashed)
                {
                    TrashData(cache, storage, data);
                }
                else
                {
                    FillData(data, storage);
                    storage.ClassName = _engine.TypeManager.GetClassName(data.GetType());
                    storage.Json = Serialize(data);
                }
                
                _saveQueue.Add(storage);
            }
        }

        public Job DeleteGameSave()
        {
            return DeleteGameSave(_gameSave);
        }

        public Job DeleteGameSave(byte gameSave)
        {
            if (gameSave > 99)
            {
                throw Exceptions.InvalidGameSave;
            }

            return JobLocker(() => Job.Run(handler => DeleteGameSaveRunner(handler, gameSave), $"Delete GameSave {gameSave}"));
        }
        
        public DataStateInfo GetState(GDFPlayerData data)
        {
            DataStateInfo info = new DataStateInfo();

            if (data.Reference == null)
            {
                return info;
            }
            
            Cache cache = data.GameSave == 0 ? _commonCache : _gameSaveCache;
            if (cache.GetStorage(data.Reference) != null)
            {
                info.state = DataState.Attached;
            }
            
            GDFPlayerDataStorage storage = _saveQueue.Find(data);
            if (storage != null)
            {
                info.state |= DataState.Savable;
                info.saveModificationDate = storage.Modification;
            }

            storage = _syncQueue.Find(data);
            if (storage != null)
            {
                info.state |= DataState.Syncable;
                info.syncModificationDate = storage.Modification;
            }

            return info;
        }

        public void Delete(GDFPlayerData data)
        {
            using(_lock.Use(_engine))
            {
                data.Trashed = true;

                AddToSaveQueue(data);
            }
        }

        public Job Save()
        {
            return SaveToGameSave(_gameSave);
        }
        
        public Job SaveToGameSave(byte gameSave)
        {
            if (gameSave > 99)
            {
                throw Exceptions.InvalidGameSave;
            }

            return JobLocker(() => Job.Run(handler => SaveRunner(handler, gameSave), $"Save GameSave {gameSave}"));
        }

        public Job Sync()
        {
            return JobLocker(() => Job.Run(SyncRunner, "Player data sync"));
        }

        public Job Purge()
        {
            return JobLocker(() => Job.Run(PurgeRunner, "Purge data"));
        }

        public Job MigrateLocalData()
        {
            string jobName = "Purge data";
            
            if (_engine.AccountManager.IsLocal)
            {
                return Job.Success(jobName);
            }

            if (!_engine.AccountManager.Authentication.Local.Exists)
            {
                return Job.Success(jobName);
            }

            return JobLocker(() => Job.Run(MigrationRunner, jobName));
        }

        private void TrashData(Cache cache, GDFPlayerDataStorage storage, GDFPlayerData data)
        {
            FillData(data, storage);
            storage.ClassName = "";
            storage.Json = "";
            cache.Remove(storage.Reference);
            UnregisterReference(storage.Reference, storage.GameSave);

            if (data == null)
            {
                return;
            }

            if (storage.GameSave == 0)
            {
                return;
            }

            GDFPlayerData defaultData = _commonCache.Get(storage.Reference);
            if (defaultData == null) return;
            
            JsonConvert.PopulateObject(JsonConvert.SerializeObject(defaultData), data);
            data.CopyFrom(defaultData);
            _commonCache.Update(data);
        }

        private Job LoadJob(byte gameSave)
        {
            return Job.Run(handler => LoadRunner(handler, gameSave), $"Load gamesave {gameSave}");
        }

        private GDFPlayerData FromCache(string reference, bool defaultGameSave)
        {
            using(_lock.Use(_engine))
            {
                GDFPlayerData cache;
                if (!defaultGameSave)
                {
                    cache = _gameSaveCache.Get(reference);
                    if (cache != null)
                    {
                        return cache;
                    }
                }
                
                return _commonCache.Get(reference);
            }
        }
        
        private GDFPlayerDataStorage FromStorageCache(string reference, bool defaultGameSave)
        {
            using(_lock.Use(_engine))
            {
                GDFPlayerDataStorage cache;
                if (!defaultGameSave && _gameSaveCache.TryGetStorage(reference, out cache))
                {
                    return cache;
                }

                _commonCache.TryGetStorage(reference, out cache);
                return cache;
            }
        }

        private string Serialize(GDFPlayerData data)
        {
            try
            {
                return JsonConvert.SerializeObject(data);
            }
            catch
            {
                return null;
            }

        }

        private void FillData(GDFPlayerData data, GDFPlayerDataStorage storage)
        {
            data.ProtectedFill(storage);
            data.Channels = storage.Channels;
            data.Trashed = storage.Trashed;
        }

        private List<GDFPlayerDataStorage> Iterate(Type type, bool includeInherited, bool defaultGameSave)
        {
            using(_lock.Use(_engine))
            {
                List<GDFPlayerDataStorage> list = new List<GDFPlayerDataStorage>();
                if (includeInherited)
                {
                    if (!defaultGameSave)
                    {
                        if (FindByTypeInCache(type, _gameSaveCache, list))
                        {
                            return list;
                        }
                    }
                    
                    FindByTypeInCache(type, _commonCache, list);
                    return list;
                }
                
                string className = _engine.TypeManager.GetClassName(type);
                if (!defaultGameSave)
                {
                    if (FindByExactTypeInCache(className, _gameSaveCache, list))
                    {
                        return list;
                    }
                }
                
                FindByExactTypeInCache(className, _commonCache, list);
                return list;
            }
        }

        private bool FindByTypeInCache(Type type, Cache cache, List<GDFPlayerDataStorage> storages)
        {
            foreach (PlayerReferenceStorage reference in _references.Values)
            {
                if (cache.TryGetStorage(reference.Reference, out GDFPlayerDataStorage storage)
                    && _engine.TypeManager.Is(_engine.TypeManager.GetType(storage.ClassName), type))
                {
                    storages.Add(storage);
                }
            }

            return storages.Count != 0;
        }

        private bool FindByExactTypeInCache(string className, Cache cache, List<GDFPlayerDataStorage> storages)
        {
            foreach (PlayerReferenceStorage reference in _references.Values)
            {
                if (cache.TryGetStorage(reference.Reference, out GDFPlayerDataStorage storage)
                    && className == storage.ClassName)
                {
                    storages.Add(storage);
                }
            }

            return storages.Count != 0;
        }
    
        private string GenerateReference(Cache cache, Type type, int channel)
        {
            string sha = GDFSecurityTools.HashMd5(_engine.TypeManager.GetClassName(type));
            string cha = channel.ToString("X8");
            string reference;
            do
            {
                reference = $"{sha}-{cha}-{_rng.Next():X8}";
            }
            while(cache.Contains(reference));

            return reference;
        }

        private void SaveRunner(IJobHandler handler, byte gameSave)
        {
            handler.StepAmount = 3;

            using (_lock.Use(_engine))
            {
                Saving.Invoke(handler.Split());
                try
                {
                    if (gameSave == _gameSave)
                    {
                        SaveRunnerUnsafe(handler.Split());
                    }
                    else if (gameSave == 0)
                    {
                        SaveOverriteCommonRunnerUnsafe(handler.Split());
                    }
                    else
                    {
                        SaveOverriteOtherRunnerUnsafe(handler.Split(), gameSave);
                    }
                }
                finally
                {
                    Saved.Invoke(handler.Split());
                }
            }
        }

        private void SyncRunner(IJobHandler handler)
        {
            handler.StepAmount = 4;
            using (_lock.Use(_engine))
            {
                Syncing.Invoke(handler.Split());
                try
                {
                    SaveRunnerUnsafe(handler.Split());
                    if (_engine.AccountManager.IsLocal)
                    {
                        return;
                    }
                    SyncRunnerUnsafe(handler.Split());
                }
                finally
                {
                    Synced.Invoke(handler.Split());
                }
            }
        }

        private void MigrationRunner(IJobHandler handler)
        {
            handler.StepAmount = 2;
            using (_lock.Use(_engine))
            {
                _engine.PersistanceManager.Migrate(handler.Split());
                LoadRunnerUnsafe(handler.Split(), _gameSave);
            }
        }

        private void DeleteGameSaveRunner(IJobHandler handler, byte gameSave)
        {
            using(_lock.Use(_engine))
            {
                handler.StepAmount = 2;
                DeleteGameSaveRunnerUnsafe(handler.Split(), gameSave);
            }
        }

        private void DeleteGameSaveRunnerUnsafe(IJobHandler handler, byte gameSave)
        {
            if (!GameSaveExists(gameSave)) return;
            if (gameSave == 0)
            {
                DeleteGameSaveRunnerUnsafe(handler, _commonCache, gameSave);
            }
            else if (gameSave == _gameSave)
            {
                DeleteGameSaveRunnerUnsafe(handler);
            }
            else
            {
                DeleteUnloadedGameSaveRunnerUnsafe(handler, gameSave);
            }
        }

        private void DeleteGameSaveRunnerUnsafe(IJobHandler handler)
        {
            handler.StepAmount = 4;

            List<GDFPlayerDataStorage> dataToSync = DeleteGameSaveRunnerUnsafe(handler.Split(), _gameSaveCache, _gameSave);

            foreach (GDFPlayerDataStorage storage in dataToSync)
            {
                if (!_gameSaveCache.data.TryGetValue(storage.Reference, out GDFPlayerData data))
                {
                    continue;
                }

                GDFPlayerData defaultData = _commonCache.Get(storage.Reference);
                if (defaultData == null) return;
                
                JsonConvert.PopulateObject(JsonConvert.SerializeObject(defaultData), data);
                data.CopyFrom(defaultData);
                _commonCache.Update(data);
            }

            handler.Step();

            _information.GameSaves.Remove(_gameSave);
            _engine.PersistanceManager.SaveInformation(handler.Split(), _information);
        }

        private List<GDFPlayerDataStorage> DeleteGameSaveRunnerUnsafe(IJobHandler handler, Cache cache, byte gameSave)
        {
            handler.StepAmount = 5;
            List<GDFPlayerDataStorage> dataToSync = new List<GDFPlayerDataStorage>();
            List<PlayerReferenceStorage> references = new List<PlayerReferenceStorage>();

            foreach (GDFPlayerDataStorage storage in cache.GetStorages())
            {
                storage.Trashed = true;
                storage.ClassName = "";
                storage.Json = "";

                dataToSync.Add(storage);
                _syncQueue.Add(storage);
                references.Add(UnregisterReference(storage.Reference, gameSave, true));
            }

            handler.Step();

            _saveQueue.ClearGameSave(gameSave);
            cache.Clear();

            handler.Step();

            _engine.PersistanceManager.SaveDataToSync(handler.Split(), dataToSync);
            _engine.PersistanceManager.SaveReference(handler.Split(), references);
            _engine.PersistanceManager.Purge(handler.Split(), gameSave);
            
            _information.GameSaves.Remove(gameSave);
            _engine.PersistanceManager.SaveInformation(handler.Split(), _information);

            return dataToSync;
        }

        private void DeleteUnloadedGameSaveRunnerUnsafe(IJobHandler handler, byte gameSave)
        {
            handler.StepAmount = 7;

            List<GDFPlayerDataStorage> dataToSync = new List<GDFPlayerDataStorage>();
            List<PlayerReferenceStorage> references = new List<PlayerReferenceStorage>();

            _engine.PersistanceManager.Load(handler, gameSave, dataToSync, null);

            foreach (GDFPlayerDataStorage storage in dataToSync)
            {
                storage.Trashed = true;
                storage.ClassName = "";
                storage.Json = "";

                _syncQueue.Add(storage);
                references.Add(UnregisterReference(storage.Reference, gameSave, true));
            }

            handler.Step();

            _saveQueue.ClearGameSave(gameSave);

            handler.Step();

            _engine.PersistanceManager.SaveDataToSync(handler.Split(), dataToSync);
            _engine.PersistanceManager.SaveReference(handler.Split(), references);
            _engine.PersistanceManager.Purge(handler.Split(), gameSave);
            
            _information.GameSaves.Remove(gameSave);
            _engine.PersistanceManager.SaveInformation(handler.Split(), _information);
        }

        private void PurgeRunner(IJobHandler handler)
        {
            using(_lock.Use(_engine))
            {
                handler.StepAmount = 2;

                _references.Clear();
                _commonCache.Clear();
                _gameSaveCache.Clear();
                _referenceSaveQueue.Clear();
                _saveQueue.Clear();
                _syncQueue.Clear();
                _information.SyncDate = DateTime.MinValue;

                _engine.PersistanceManager.Purge(handler.Split());

                if (_engine.AccountManager.IsLocal)
                {
                    return;
                }

                _headers.Clear();
                _engine.ServerManager.FillHeaders(_headers, _engine.AccountManager.Bearer);
                Delete<int>(handler.Split(), _engine.ServerManager.BuildSyncURL("/api/v1/player-data"), _headers);
            }
        }

        private void LoadRunner(IJobHandler handler, byte gameSave)
        {
            handler.StepAmount = 3;

            using (_lock.Use(_engine))
            {
                Loading.Invoke(handler.Split());
                try
                {
                    LoadRunnerUnsafe(handler.Split(), gameSave);
                }
                finally
                {
                    Loaded.Invoke(handler.Split());
                }
            }
        }

        private void LoadRunnerUnsafe(IJobHandler handler, byte gameSave)
        {
            handler.StepAmount = 4;
            if (gameSave != 0)
            {
                handler.StepAmount ++;
            }
            
            _references.Clear();
            _commonCache.Clear();
            _gameSaveCache.Clear();
            _saveQueue.Clear();
            List<PlayerReferenceStorage> references = new List<PlayerReferenceStorage>();
            
            _engine.PersistanceManager.LoadReference(handler.Split(), references);
            foreach (PlayerReferenceStorage reference in references)
            {
                _references.Add(reference.Reference, reference);
            }

            _information = _engine.PersistanceManager.LoadInformation(handler.Split());

            LoadRunnerUnsafe(handler.Split(), _commonCache, 0);
            
            if (gameSave != 0)
            {
                LoadRunnerUnsafe(handler.Split(), _gameSaveCache, gameSave);
            }
            else
            {
                handler.Step();
            }
            _gameSave = gameSave;
        }

        private void LoadRunnerUnsafe(IJobHandler handler, Cache cache, byte gameSave)
        {
            handler.StepAmount = 3;

            _loadDataBuffer.Clear();
            _loadDataToSyncBuffer.Clear();
            _engine.PersistanceManager.Load(handler.Split(), gameSave, _loadDataBuffer, _loadDataToSyncBuffer);
            for (int i = _loadDataBuffer.Count - 1; i >= 0; i--)
            {
                _loadDataBuffer[i].GameSave = gameSave;
            }
            UpdateStorage(handler.Split(), cache);
        }

        private void SaveRunnerUnsafe(IJobHandler handler)
        {
            List<GDFPlayerDataStorage> storages = _saveQueue.ToList();

            handler.StepAmount = 4;

            _engine.PersistanceManager.Save(handler.Split(), storages);
            _engine.PersistanceManager.SaveDataToSync(handler.Split(), storages);

            foreach (GDFPlayerDataStorage data in storages)
            {
                _syncQueue.Add(data);
                _information.GameSaves.Add(data.GameSave);
            }
            _engine.PersistanceManager.SaveReference(handler.Split(), _referenceSaveQueue);
            _engine.PersistanceManager.SaveInformation(handler.Split(), _information);
            _saveQueue.Clear();
            _referenceSaveQueue.Clear();
        }

        private void SaveOverriteCommonRunnerUnsafe(IJobHandler handler)
        {
            handler.StepAmount = 8;

            foreach (GDFPlayerDataStorage storage in _saveQueue)
            {
                if (storage.GameSave == 0) continue;

                UnregisterReference(storage.Reference, storage.GameSave, true);
            }

            foreach (GDFPlayerDataStorage storage in _commonCache.GetStorages())
            {
                if (_saveQueue.Contains(storage)) continue;

                UnregisterReference(storage.Reference, 0, true);

                storage.Trashed = true;
                storage.ClassName = "";
                storage.Json = "";
                _saveQueue.Add(storage);
            }

            handler.Step();

            _saveQueue.ClearGameSave(_gameSave);

            foreach (GDFPlayerDataStorage storage in _gameSaveCache.GetStorages())
            {
                GDFPlayerDataStorage data = new GDFPlayerDataStorage();
                data.CopyFrom(storage);
                data.GameSave = 0;

                _saveQueue.Add(data);
            }

            handler.Step();

            List<GDFPlayerDataStorage> saveData = _saveQueue.ToList();

            _engine.PersistanceManager.Save(handler.Split(), saveData);
            _engine.PersistanceManager.SaveDataToSync(handler.Split(), saveData);

            foreach (GDFPlayerDataStorage storage in saveData)
            {
                if (!storage.Trashed)
                {
                    RegisterReference(storage.Reference, 0, storage.ClassName, true);
                }

                _syncQueue.Add(storage);
            }

            handler.Step();

            _engine.PersistanceManager.SaveReference(handler.Split(), _referenceSaveQueue);
            _engine.PersistanceManager.SaveInformation(handler.Split(), _information);
            _saveQueue.Clear();
            _referenceSaveQueue.Clear();

            _commonCache.Clear();
            foreach (GDFPlayerDataStorage storage in saveData)
            {
                if (storage.Trashed) continue;

                _commonCache.Add(storage.Reference, storage);
            }
            
            handler.Step();
        }

        private void SaveOverriteOtherRunnerUnsafe(IJobHandler handler, byte gameSave)
        {
            handler.StepAmount = 9;

            List<GDFPlayerDataStorage> backup = new List<GDFPlayerDataStorage>();
            foreach (GDFPlayerDataStorage storage in _saveQueue)
            {
                if (storage.GameSave == 0) continue;

                UnregisterReference(storage.Reference, storage.GameSave, true);
                backup.Add(storage);
            }

            _saveQueue.ClearGameSave(_gameSave);

            Cache cache = _gameSave == 0 ? _commonCache : _gameSaveCache;
            if (GameSaveExists(gameSave))
            {
                List<GDFPlayerDataStorage> storages = new List<GDFPlayerDataStorage>();
                _engine.PersistanceManager.Load(handler.Split(), gameSave, storages, null);
                foreach (GDFPlayerDataStorage storage in storages)
                {
                    if (_saveQueue.Contains(storage)) continue;

                    UnregisterReference(storage.Reference, gameSave, true);

                    storage.Trashed = true;
                    storage.ClassName = "";
                    storage.Json = "";
                    _saveQueue.Add(storage);
                }
                _engine.PersistanceManager.Purge(handler.Split(), gameSave);
            }
            else
            {
                _information.GameSaves.Add(gameSave);
                handler.Step();
                handler.Step();
            }

            foreach (GDFPlayerDataStorage storage in cache.GetStorages())
            {
                GDFPlayerDataStorage data = new GDFPlayerDataStorage();
                data.CopyFrom(storage);
                data.GameSave = gameSave;

                _saveQueue.Add(data);
            }

            handler.Step();

            List<GDFPlayerDataStorage> saveData = _saveQueue.ToList();

            _engine.PersistanceManager.Save(handler.Split(), saveData);
            _engine.PersistanceManager.SaveDataToSync(handler.Split(), saveData);

            foreach (GDFPlayerDataStorage storage in saveData)
            {
                if (!storage.Trashed)
                {
                    RegisterReference(storage.Reference, storage.GameSave, storage.ClassName, true);
                }

                if (backup.Any(x => x.Reference == storage.Reference && x.GameSave == storage.GameSave))
                {
                    RegisterReference(storage.Reference, _gameSave, storage.ClassName, true);
                }

                _syncQueue.Add(storage);
            }

            handler.Step();

            _engine.PersistanceManager.SaveReference(handler.Split(), _referenceSaveQueue);
            _engine.PersistanceManager.SaveInformation(handler.Split(), _information);
            _saveQueue.Clear();
            _referenceSaveQueue.Clear();
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
                
                _headers.Clear();
                _engine.ServerManager.FillHeaders(_headers, _engine.AccountManager.Bearer);
                try
                {
                    Post<string>(handler.Split(), _engine.ServerManager.BuildSyncURL("/api/v1/player-data"), _headers, exchange);
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
            List<PlayerReferenceStorage> references = new List<PlayerReferenceStorage>();
            
            IJobHandler rootHandler = handler;
            handler.StepAmount = 2;

            do
            {
                handler = handler.Split();
                handler.StepAmount = 4;

                references.Clear();

                _headers.Clear();
                _engine.ServerManager.FillHeaders(_headers, _engine.AccountManager.Bearer);

                pagination = Get<PlayerDataPagination>(handler.Split(), _engine.ServerManager.BuildSyncURL("/api/v1/player-data/" + _information.SyncDate.ToISO8601().ToBase64URL()), _headers);

                if (pagination.Items.Count == 0)
                {
                    return;
                }

                foreach (GDFPlayerDataStorage playerStorage in pagination.Items)
                {
                    _saveQueue.Remove(playerStorage);
                    _syncQueue.Remove(playerStorage);

                    PlayerReferenceStorage reference;
                    if (playerStorage.Trashed)
                    {
                        reference = UnregisterReference(playerStorage.Reference, playerStorage.GameSave, false);
                    }
                    else
                    {
                        reference = RegisterReference(playerStorage.Reference, playerStorage.GameSave, playerStorage.ClassName, false);
                    }
                    references.Add(reference);
                    _information.GameSaves.Add(playerStorage.GameSave);

                    Cache cache = null;
                    if(playerStorage.GameSave == 0)
                    {
                        cache = _commonCache;
                    }
                    else if (playerStorage.GameSave == _gameSave)
                    {
                        cache = _gameSaveCache;
                    }

                    if (playerStorage.Trashed)
                    {
                        cache?.Remove(playerStorage.Reference);
                    }
                    else
                    {
                        cache?.Update(playerStorage);
                    }

                    if (_information.SyncDate < playerStorage.SyncDateTime)
                    {
                        _information.SyncDate = playerStorage.SyncDateTime;
                    }
                }

                _engine.PersistanceManager.SaveReference(handler.Split(), references);
                _engine.PersistanceManager.Save(handler.Split(), pagination.Items);
                
            } while (pagination.Items.Count != pagination.Total);

            _engine.PersistanceManager.SaveInformation(rootHandler.Split(), _information);
        }

        private void UpdateStorage(IJobHandler handler, Cache cache)
        {
            handler.StepAmount = _loadDataBuffer.Count + _loadDataToSyncBuffer.Count;
            foreach (GDFPlayerDataStorage storage in _loadDataBuffer)
            {
                storage.Account = _engine.AccountManager.Reference;
                storage.Range = _engine.AccountManager.Range;
                storage.Project = _engine.Configuration.Reference;
                storage.ClassName = _references[storage.Reference].Classname;
                cache.Add(storage.Reference, storage);
                handler.Step();
            }

            handler.StepAmount = _loadDataToSyncBuffer.Count;
            foreach (GDFPlayerDataStorage storage in _loadDataToSyncBuffer)
            {
                storage.Account = _engine.AccountManager.Reference;
                storage.Range = _engine.AccountManager.Range;
                storage.Project = _engine.Configuration.Reference;
                _syncQueue.Add(storage);
                handler.Step();
            }
        }

        private PlayerReferenceStorage RegisterReference(string reference, byte gameSave, string className, bool needToBeSaved = true)
        {
            PlayerReferenceStorage playerReference;
            if (!_references.TryGetValue(reference, out playerReference))
            {
                playerReference = new PlayerReferenceStorage(){
                    Reference = reference,
                    Classname = className
                };

                _references.Add(reference, playerReference);
            }
            
            playerReference.GameSaves.Add(gameSave);

            if (needToBeSaved && !_referenceSaveQueue.Contains(playerReference))
            {
                _referenceSaveQueue.Add(playerReference);
            }

            return playerReference;
        }

        private PlayerReferenceStorage UnregisterReference(string reference, byte gameSave, bool needToBeSaved = true)
        {
            PlayerReferenceStorage playerReference = _references[reference];
            playerReference.GameSaves.Remove(gameSave);

            if (playerReference.Count == 0)
            {
                _references.Remove(reference);
            }

            if (needToBeSaved && !_referenceSaveQueue.Contains(playerReference))
            {
                _referenceSaveQueue.Add(playerReference);
            }

            return playerReference;
        }

        private void OnAccountChanging(IJobHandler handler, MemoryJwtToken token)
        {
            using(_lock.Use(_engine))
            {
                if (token == null)
                {
                    return;
                }
                
                _references.Clear();
                _commonCache.Clear();
                _gameSaveCache.Clear();
                _referenceSaveQueue.Clear();
                _saveQueue.Clear();
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

                _references.Clear();
                _gameSaveCache.Clear();
                _saveQueue.Clear();
                _syncQueue.Clear();
                _referenceSaveQueue.Clear();
                
                LoadRunnerUnsafe(handler, 0);
            }
        }

    }
}