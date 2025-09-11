using System;
using System.Collections.Generic;
using System.IO;
using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    [Dependency(typeof(IRuntimeConfigurationEngine), typeof(IRuntimeAccountManager))]
    [FullLockers(typeof(IRuntimeAccountManager), typeof(IRuntimePlayerDataManager))]
    public class RuntimePlayerPersistanceManager : IRuntimePlayerPersistanceManager
    {
        private string _root;
        private SQLiteDbConnection _mainConnection = null;
        private Dictionary<byte, SQLiteDbConnection> _connectionCache = new Dictionary<byte, SQLiteDbConnection>();
        private IRuntimeEngine _engine;

        public RuntimePlayerPersistanceManager(IRuntimeEngine engine)
        {
            _engine = engine;
            _engine.AccountManager.AccountChanged.onBackgroundThread += OnAccountChanged;
        }
        
        ~RuntimePlayerPersistanceManager()
        {
            _engine.AccountManager.AccountChanged.onBackgroundThread -= OnAccountChanged;
        }

        public void LoadReference(IJobHandler handler, List<PlayerReferenceStorage> references)
        {
            if (references == null)
            {
                throw new ArgumentNullException(nameof(references));
            }

            handler.StepAmount = 2;
            using (IDBConnection connection = _mainConnection.Open())
            {
                PlayerReferenceDAL.Instance.Validate(handler.Split(), connection);
                PlayerReferenceDAL.Instance.Get(handler.Split(), connection, references);
            }
        }
        
        public void SaveReference(IJobHandler handler, List<PlayerReferenceStorage> references)
        {
            if (references == null)
            {
                return;
            }

            handler.StepAmount = 2;
            using (IDBConnection connection = _mainConnection.Open())
            {
                PlayerReferenceDAL.Instance.Validate(handler.Split(), connection);
                PlayerReferenceDAL.Instance.Record(handler.Split(), connection, references);
            }
        }

        public void Load(IJobHandler handler, byte gameSave, List<GDFPlayerDataStorage> data, List<GDFPlayerDataStorage> dataToSync)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            handler.StepAmount = 4;
            using (IDBConnection connection = GetConnection(gameSave).Open())
            {
                PlayerDataDAL.Instance.Validate(handler.Split(), connection);
                PlayerDataDAL.Instance.Get(handler.Split(), connection, data);
            }

            if (dataToSync == null)
            {
                return;
            }

            using (IDBConnection connection = _mainConnection.Open())
            {
                PlayerDataToSyncDAL.Instance.Validate(handler.Split(), connection);
                PlayerDataToSyncDAL.Instance.Get(handler.Split(), connection, dataToSync);
            }
        }

        public void Save(IJobHandler handler, List<GDFPlayerDataStorage> storages)
        {
            if (storages == null)
            {
                return;
            }

            Dictionary<byte, List<GDFPlayerDataStorage>> cache = SplitStoragesByGameSave(storages);

            handler.StepAmount = cache.Count * 2 + 2;
            foreach (KeyValuePair<byte, List<GDFPlayerDataStorage>> pair in cache)
            {
                using (IDBConnection connection = GetConnection(pair.Key).Open())
                {
                    PlayerDataDAL.Instance.Validate(handler.Split(), connection);
                    PlayerDataDAL.Instance.Record(handler.Split(), connection, pair.Value);
                }
            }
        }

        public void SaveDataToSync(IJobHandler handler, List<GDFPlayerDataStorage> storages)
        {
            handler.StepAmount = 2;
            using (IDBConnection connection = _mainConnection.Open())
            {
                PlayerDataToSyncDAL.Instance.Validate(handler.Split(), connection);
                PlayerDataToSyncDAL.Instance.Record(handler.Split(), connection, storages);
            }
        }
        
        public void RemoveDataToSync(IJobHandler handler, List<GDFPlayerDataStorage> storages)
        {
            if (storages == null)
            {
                return;
            }

            using (IDBConnection connection = _mainConnection.Open())
            {
                PlayerDataToSyncDAL.Instance.Validate(handler.Split(), connection);
                PlayerDataToSyncDAL.Instance.DeleteData(handler.Split(), connection, storages);
            }
        }

        public PlayerStorageInformation LoadInformation(IJobHandler handler)
        {
            return LoadInformation(handler, _mainConnection);
        }

        public void SaveInformation(IJobHandler handler, PlayerStorageInformation information)
        {
            handler.StepAmount = 2;
            
            using IDBConnection connection = _mainConnection.Open();
            
            PlayerSyncDateDAL.Instance.Validate(handler.Split(), connection);
            PlayerSyncDateDAL.Instance.Record(handler.Split(), connection, information);
        }

        public void Purge(IJobHandler handler)
        {
            string path;
            PlayerStorageInformation information = LoadInformation(handler);

            foreach (byte gameSave in information.GameSaves)
            {
                path = GetFilePath(GDF.Account.Identity, gameSave);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }

            path = GetFilePath(GDF.Account.Identity);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void Purge(IJobHandler handler, byte gameSave)
        {
            string path = GetFilePath(GDF.Account.Identity, gameSave);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void Migrate(IJobHandler handler)
        {
            string localPath;
            string onlinePath;

            Purge(handler);

            string path = GetFilePath(GDF.Account.LocalIdentity);
            SQLiteDbConnection connection = new SQLiteDbConnection(path);

            PlayerStorageInformation information = LoadInformation(handler, connection);

            foreach (byte gameSave in information.GameSaves)
            {
                localPath = GetFilePath(GDF.Account.LocalIdentity, gameSave);
                onlinePath = GetFilePath(GDF.Account.Identity, gameSave);

                if (File.Exists(localPath))
                {
                    File.Copy(localPath, onlinePath, true);
                }
            }

            localPath = GetFilePath(GDF.Account.LocalIdentity);
            onlinePath = GetFilePath(GDF.Account.Identity);

            if (File.Exists(localPath))
            {
                File.Copy(localPath, onlinePath, true);
            }
        }
        
        private PlayerStorageInformation LoadInformation(IJobHandler handler, SQLiteDbConnection mainConnection)
        {
            handler.StepAmount = 2;

            using (IDBConnection connection = mainConnection.Open())
            {
                PlayerSyncDateDAL.Instance.Validate(handler.Split(), connection);
                return PlayerSyncDateDAL.Instance.Get(handler.Split(), connection);
            }
        }

        private Dictionary<byte, List<GDFPlayerDataStorage>> SplitStoragesByGameSave(IEnumerable<GDFPlayerDataStorage> storages)
        {
            Dictionary<byte, List<GDFPlayerDataStorage>> dictionary = new Dictionary<byte, List<GDFPlayerDataStorage>>();

            foreach (GDFPlayerDataStorage storage in storages)
            {
                List<GDFPlayerDataStorage> list;
                if (!dictionary.TryGetValue(storage.GameSave, out list))
                {
                    list = new List<GDFPlayerDataStorage>();
                    dictionary.Add(storage.GameSave, list);
                }
                list.Add(storage);
            }

            return dictionary;
        }

        private void OnAccountChanged(IJobHandler handler, MemoryJwtToken token)
        {
            if (token == null)
            {
                return;
            }

            handler.StepAmount = 2;

            string path = GetFilePath(GDF.Account.Identity);

            handler.Step();

            _connectionCache.Clear();
            _mainConnection = new SQLiteDbConnection(path);
        }

        private IDBConnection GetConnection(byte gameSave)
        {
            if (_connectionCache.TryGetValue(gameSave, out SQLiteDbConnection connection))
            {
                return connection;
            }
            string path = GetFilePath(GDF.Account.Identity, gameSave);
            connection = new SQLiteDbConnection(path);
            _connectionCache.Add(gameSave, connection);
            return connection;
        }

        private string GetFilePath(string identity)
        {
            _root = Path.Combine(GDFUserSettings.Instance.GenerateContainerName(GDFUserSettings.EnvironmentContainer(_engine)), identity);
            if (!Directory.Exists(_root))
            {
                Directory.CreateDirectory(_root);
            }

            return Path.Combine(_root, "global.data");
        }

        private string GetFilePath(string identity, byte gameSave)
        {
            _root = Path.Combine(GDFUserSettings.Instance.GenerateContainerName(GDFUserSettings.EnvironmentContainer(_engine)), identity);
            if (!Directory.Exists(_root))
            {
                Directory.CreateDirectory(_root);
            }

            return Path.Combine(_root, gameSave + ".save");
        }
    }
}