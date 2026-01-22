using System;
using System.Collections.Generic;
using System.Linq;
using GDFFoundation;
using GDFRuntime;
using Newtonsoft.Json;

namespace GDFUnity
{
    public class PlayerDataGameSave
    {
        private class Cache
        {
            private readonly object _lock = new object();

            internal Dictionary<string, GDFPlayerData> data = new Dictionary<string, GDFPlayerData>();
            private Dictionary<string, GDFPlayerDataStorage> _storage = new Dictionary<string, GDFPlayerDataStorage> ();

            public int Count => _storage.Count;
            public IEnumerable<string> KeysUnsafe => _storage.Keys;
            public IEnumerable<GDFPlayerDataStorage> StoragesUnsafe => _storage.Values;

            public bool Contains(string reference)
            {
                lock(_lock)
                {
                    return _storage.ContainsKey(reference);
                }
            }

            public void Add(string reference, GDFPlayerDataStorage storage, GDFPlayerData data)
            {
                lock(_lock)
                {
                    _storage.Add(reference, storage);
                    this.data.Add(reference, data);
                }
            }
            
            public void Add(string reference, GDFPlayerData data)
            {
                lock(_lock)
                {
                    this.data.Add(reference, data);
                }
            }
            
            public void Add(string reference, GDFPlayerDataStorage storage)
            {
                lock(_lock)
                {
                    _storage.Add(reference, storage);
                }
            }

            public GDFPlayerData Get(RuntimePlayerDataManager manager, string reference)
            {
                GDFPlayerData data;
                lock(_lock)
                {
                    if (this.data.TryGetValue(reference, out data))
                    {
                        return data;
                    }

                    if (_storage.TryGetValue(reference, out GDFPlayerDataStorage storage))
                    {
                        data = Deserialize(manager, storage);
                        this.data.Add(reference, data);
                    }
                    return data;
                }
            }

            public bool TryGetStorage(string reference, out GDFPlayerDataStorage storage)
            {
                storage = null;
                lock(_lock)
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
                lock(_lock)
                {
                    _storage.TryGetValue(reference, out storage);
                    return storage;
                }
            }

            public GDFPlayerData Update(RuntimePlayerDataManager manager, GDFPlayerDataStorage storage)
            {
                if (!Contains(storage.Reference))
                {
                    Add(storage.Reference, storage);
                    return null;
                }

                GDFPlayerData data;
                lock(_lock)
                {
                    if (this.data.TryGetValue(storage.Reference, out data))
                    {
                        JsonConvert.PopulateObject(storage.Json, data);
                        return data;
                    }
                    data = Deserialize(manager, storage);
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

                lock(_lock)
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
                lock(_lock)
                {
                    data.Remove(reference);
                    return _storage.Remove(reference);
                }
            }

            public void Clear()
            {
                lock(_lock)
                {
                    _storage.Clear();
                    data.Clear();
                }
            }
            
            private GDFPlayerData Deserialize(RuntimePlayerDataManager manager, GDFPlayerDataStorage storage)
            {
                GDFPlayerData data = JsonConvert.DeserializeObject(storage.Json, manager._engine.TypeManager.GetType(storage.ClassName)) as GDFPlayerData;
                data.ProtectedFill(storage);
                data.Channels = storage.Channels;
                data.Trashed = storage.Trashed;
                return data;
            }
        }

        private class SaveQueue : IEnumerable<GDFPlayerDataStorage>
        {
            private Dictionary<string, GDFPlayerDataStorage> _storages = new Dictionary<string, GDFPlayerDataStorage>();
            public int Count => _storages.Count;

            public GDFPlayerDataStorage Find(string reference)
            {
                if (_storages.TryGetValue(reference, out GDFPlayerDataStorage value))
                {
                    return value;
                }
                return null;
            }

            public void Add(GDFPlayerDataStorage storage)
            {
                if (_storages.ContainsKey(storage.Reference))
                {
                    return;
                }
                
                _storages.Add(storage.Reference, storage);
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
                _storages.Remove(storage.Reference);
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

        private byte _gameSave;
        private RuntimePlayerDataManager _manager;
        private Cache _cache = new Cache ();
        private SaveQueue _saveQueue = new SaveQueue();
        private List<GDFPlayerDataStorage> _loadBuffer = new List<GDFPlayerDataStorage>();
        
        public byte GameSave => _gameSave;
        internal bool HasDataToSave => _saveQueue.Count != 0;
        internal int DataToSaveAmount => _saveQueue.Count;

        public PlayerDataGameSave(RuntimePlayerDataManager manager, byte gameSave)
        {
            _manager = manager;
            _gameSave = gameSave;
        }

        public void Add(string reference, GDFPlayerData data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            if (data.Trashed)
            {
                throw RuntimePlayerDataManager.Exceptions.CannotAddTrashed;
            }

            if (data.Reference != null)
            {
                throw RuntimePlayerDataManager.Exceptions.DataAlreadyExists;
            }

            if (data.Channels == 0)
            {
                data.Channels = _manager._engine.Configuration.Channel;
            }

            if (string.IsNullOrWhiteSpace(reference))
            {
                reference = GenerateReference(data.GetType(), data.Channels);
            }

            reference = reference.Trim();
            if (_cache.Contains(reference))
            {
                throw RuntimePlayerDataManager.Exceptions.DataAlreadyExists;
            }
            
            string className = _manager._engine.TypeManager.GetClassName(data.GetType());
            GDFPlayerDataStorage storage = new GDFPlayerDataStorage();
            storage.Reference = reference;
            storage.Creation = GDFDatetime.Now;
            storage.Modification = storage.Creation;
            storage.GameSave = _gameSave;
            storage.Project = _manager._engine.Configuration.Reference;
            storage.Account = _manager._engine.AccountManager.Reference;
            storage.Range = _manager._engine.AccountManager.Range;
            storage.ClassName = _manager._engine.TypeManager.GetClassName(data.GetType());
            storage.Channels = data.Channels;
            storage.Trashed = data.Trashed;
            
            data.ProtectedFill(storage);

            string json = Serialize(data);
            storage.Json = json;

            _cache.Add(data.Reference, storage, data);
            
            _saveQueue.Add(storage);
        }

        public GDFPlayerData Get(string reference)
        {
            return _cache.Get(_manager, reference);
        }
        public GDFPlayerData Get(Type type, string reference)
        {
            if (!_manager._engine.TypeManager.Is(type, typeof(GDFPlayerData)))
            {
                throw RuntimePlayerDataManager.Exceptions.InvalidTypeForReference;
            }

            GDFPlayerData data = _cache.Get(_manager, reference);
            if (data == null)
            {
                return null;
            }

            if (!_manager._engine.TypeManager.Is(data.GetType(), type))
            {
                return null;
            }

            return data;
        }

        public T Get<T>(string reference) where T : GDFPlayerData
        {
            return _cache.Get(_manager, reference) as T;
        }
        public List<GDFPlayerData> Get(Type type, bool includeInherited = true)
        {
            if (!_manager._engine.TypeManager.Is(type, typeof(GDFPlayerData)))
            {
                throw RuntimePlayerDataManager.Exceptions.InvalidTypeForReference;
            }
            List<GDFPlayerDataStorage> storages = Iterate(type, includeInherited);
            List<GDFPlayerData> list = new List<GDFPlayerData>();
            foreach (GDFPlayerDataStorage storage in storages)
            {
                GDFPlayerData item = _cache.Get(_manager, storage.Reference);
                if (item == null)
                {
                    continue;
                }
                list.Add(item);
            }
            return list;
        }
        public List<T> Get<T>(bool includeInherited = true) where T : GDFPlayerData
        {
            List<GDFPlayerDataStorage> storages = Iterate(typeof(T), includeInherited);
            List<T> list = new List<T>();
            foreach (GDFPlayerDataStorage storage in storages)
            {
                T item = _cache.Get(_manager, storage.Reference) as T;
                if (item == null)
                {
                    continue;
                }
                list.Add(item);
            }
            return list;
        }

        public DataState GetDataState(GDFPlayerData data)
        {
            DataState info = new DataState();

            if (data.Reference == null)
            {
                return info;
            }
            
            if (_cache.GetStorage(data.Reference) != null)
            {
                info.state = DataState.State.Cached;
            }
            
            GDFPlayerDataStorage storage = _saveQueue.Find(data.Reference);
            if (storage != null)
            {
                info.state |= DataState.State.Savable;
                info.saveModificationDate = storage.Modification;
            }

            storage = _manager._syncQueue.Find(data);
            if (storage != null)
            {
                info.state |= DataState.State.Syncable;
                info.syncModificationDate = storage.Modification;
            }

            return info;
        }

        public void AddToSaveQueue(GDFPlayerData data)
        {
            if (string.IsNullOrWhiteSpace(data.Reference))
            {
                throw RuntimePlayerDataManager.Exceptions.InvalidReference;
            }
            
            GDFPlayerDataStorage storage = FromStorageCache(data.Reference);
            if (storage == null)
            {
                throw RuntimePlayerDataManager.Exceptions.NotFoundInCache;
            }

            storage.Trashed = data.Trashed;
            storage.Modification = GDFDatetime.Now;

            if (data.Trashed)
            {
                TrashData(storage, data);
            }
            else
            {
                FillData(data, storage);
                storage.ClassName = _manager._engine.TypeManager.GetClassName(data.GetType());
                storage.Json = Serialize(data);
            }
            
            _saveQueue.Add(storage);
        }

        public void Delete(GDFPlayerData data)
        {
            data.Trashed = true;

            AddToSaveQueue(data);
        }

        public void SyncStorage(GDFPlayerDataStorage storage)
        {
            _saveQueue.Remove(storage);
            if (storage.Trashed)
            {
                _cache?.Remove(storage.Reference);
            }
            else
            {
                _cache?.Update(_manager, storage);
            }
        }

        public void Load(IJobHandler handler)
        {
            handler.StepAmount = 2;

            _cache.Clear();
            _saveQueue.Clear();

            _loadBuffer.Clear();
            _manager._engine.PersistanceManager.Load(handler.Split(), _gameSave, _loadBuffer);
            UpdateStorage(handler.Split());
        }

        public List<GDFPlayerDataStorage> Save(IJobHandler handler)
        {
            List<GDFPlayerDataStorage> storages = _saveQueue.ToList();

            _manager._engine.PersistanceManager.Save(handler, _gameSave, storages);
            _saveQueue.Clear();

            return storages;
        }

        public void SaveTo(IJobHandler handler, byte gameSave)
        {
            List<GDFPlayerDataStorage> storages = _saveQueue.ToList();
            _manager._engine.PersistanceManager.Save(handler, gameSave, storages);
        }

        public void CopyFrom(IJobHandler handler, PlayerDataGameSave source, List<GDFPlayerDataStorage> dataToSync, bool fullSync)
        {
            List<GDFPlayerDataStorage> storages = new List<GDFPlayerDataStorage>();
            foreach (GDFPlayerDataStorage storage in source.DuplicateStorages())
            {
                storages.Add(storage);
            }
            CopyFrom(handler, storages, dataToSync, fullSync);
        }
        
        public void CopyFrom(IJobHandler handler, List<GDFPlayerDataStorage> storages, List<GDFPlayerDataStorage> dataToSync, bool fullSync)
        {
            _cache.Clear();
            _cache = new Cache();
            foreach (GDFPlayerDataStorage storage in storages)
            {
                _cache.Add(storage.Reference, storage);
                dataToSync.Add(storage);
            }

            if (fullSync)
            {
                _loadBuffer.Clear();
                _manager._engine.PersistanceManager.Load(handler, _gameSave, _loadBuffer);
                foreach (GDFPlayerDataStorage storage in _loadBuffer)
                {
                    if (_cache.Contains(storage.Reference)) continue;
                    dataToSync.Add(storage);
                }
            }
            else
            {
                handler.Step();
            }
        }

        public void ExtractStorages(List<GDFPlayerDataStorage> dataToSync)
        {
            foreach (GDFPlayerDataStorage storage in DuplicateStorages())
            {
                dataToSync.Add(storage);
            }
        }

        private IEnumerable<GDFPlayerDataStorage> DuplicateStorages()
        {
            foreach(GDFPlayerDataStorage cache in _cache.StoragesUnsafe)
            {
                GDFPlayerDataStorage storage = new GDFPlayerDataStorage();
                storage.CopyFrom(cache);
                yield return storage;
            }
        }
        
        private void UpdateStorage(IJobHandler handler)
        {
            handler.StepAmount = _loadBuffer.Count;
            foreach (GDFPlayerDataStorage storage in _loadBuffer)
            {
                storage.Initialize(_manager._engine, _gameSave);
                _cache.Add(storage.Reference, storage);
                handler.Step();
            }
        }

        private string GenerateReference(Type type, int channel)
        {
            string sha = GDFSecurityTools.HashMd5(_manager._engine.TypeManager.GetClassName(type));
            string cha = channel.ToString("X8");
            string reference;
            do
            {
                reference = $"{sha}-{cha}-{_manager._rng.Next():X8}";
            }
            while(_cache.Contains(reference));

            return reference;
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

        private List<GDFPlayerDataStorage> Iterate(Type type, bool includeInherited)
        {
            using(_manager._lock.Use(_manager._engine))
            {
                List<GDFPlayerDataStorage> list = new List<GDFPlayerDataStorage>();
                if (includeInherited)
                {
                    FindByTypeInCache(type, list);
                    return list;
                }
                
                string className = _manager._engine.TypeManager.GetClassName(type);
                FindByExactTypeInCache(className, list);
                return list;
            }
        }

        private bool FindByTypeInCache(Type type, List<GDFPlayerDataStorage> storages)
        {
            foreach (GDFPlayerDataStorage storage in _cache.GetStorages())
            {
                if (_manager._engine.TypeManager.Is(_manager._engine.TypeManager.GetType(storage.ClassName), type))
                {
                    storages.Add(storage);
                }
            }

            return storages.Count != 0;
        }

        private bool FindByExactTypeInCache(string className, List<GDFPlayerDataStorage> storages)
        {
            foreach (GDFPlayerDataStorage storage in _cache.GetStorages())
            {
                if (className == storage.ClassName)
                {
                    storages.Add(storage);
                }
            }

            return storages.Count != 0;
        }
        
        private GDFPlayerDataStorage FromStorageCache(string reference)
        {
            using(_manager._lock.Use(_manager._engine))
            {
                _cache.TryGetStorage(reference, out GDFPlayerDataStorage cache);
                return cache;
            }
        }

        private void TrashData(GDFPlayerDataStorage storage, GDFPlayerData data)
        {
            FillData(data, storage);
            storage.ClassName = "";
            storage.Json = "";
            _cache.Remove(storage.Reference);

            if (data == null)
            {
                return;
            }

            if (storage.GameSave == 0)
            {
                return;
            }

            GDFPlayerData defaultData = _cache.Get(_manager, storage.Reference);
            if (defaultData == null) return;
            
            JsonConvert.PopulateObject(JsonConvert.SerializeObject(defaultData), data);
            data.CopyFrom(defaultData);
            _cache.Update(data);
        }

        private void FillData(GDFPlayerData data, GDFPlayerDataStorage storage)
        {
            data.ProtectedFill(storage);
            data.Channels = storage.Channels;
            data.Trashed = storage.Trashed;
        }

    }
}