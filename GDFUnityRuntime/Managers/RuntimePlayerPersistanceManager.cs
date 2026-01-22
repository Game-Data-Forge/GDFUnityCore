using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public List<byte> GetGameSaves(IJobHandler handler)
        {
            List<byte> gamesaves = new List<byte>();
            string path = GetFolderPath(GDF.Account.Identity);
            foreach (string file in Directory.EnumerateFiles(path))
            {
                int extension = file.LastIndexOf(".save");
                if (extension != file.Length - 5)
                {
                    continue;
                }
                
                int startIndex = file.LastIndexOf('/') - 1;
                string gamesave = file.Substring(startIndex, extension - startIndex);
                gamesaves.Add(byte.Parse(gamesave));
            }

            return gamesaves;
        }

        public bool Exists(byte gameSave)
        {
            string path = GetFilePath(GDF.Account.Identity, gameSave);
            return File.Exists(path);
        }

        public void Load(IJobHandler handler, byte gameSave, List<GDFPlayerDataStorage> data)
        {
            if (data == null)
            {
                throw new ArgumentNullException(nameof(data));
            }

            handler.StepAmount = 2;
            using (IDBConnection connection = GetConnection(gameSave).Open())
            {
                PlayerDataDAL.Instance.Validate(handler.Split(), connection);
                PlayerDataDAL.Instance.Get(handler.Split(), connection, data);
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

        public void Save(IJobHandler handler, byte gameSave, List<GDFPlayerDataStorage> data)
        {
            if (data == null)
            {
                return;
            }

            handler.StepAmount = 2;
            using (IDBConnection connection = GetConnection(gameSave).Open())
            {
                PlayerDataDAL.Instance.Validate(handler.Split(), connection);
                PlayerDataDAL.Instance.Record(handler.Split(), connection, data);
            }
        }

        public void Copy(IJobHandler handler, byte srcGameSave, byte dstGameSave)
        {
            string srcPath = GetFilePath(GDF.Account.Identity, srcGameSave);
            string dstPath = GetFilePath(GDF.Account.Identity, dstGameSave);

            File.Copy(srcPath, dstPath);
        }

        public void LoadDataToSync(IJobHandler handler, List<GDFPlayerDataStorage> dataToSync)
        {
            handler.StepAmount = 2;
            using (IDBConnection connection = _mainConnection.Open())
            {
                PlayerDataToSyncDAL.Instance.Validate(handler.Split(), connection);
                PlayerDataToSyncDAL.Instance.Get(handler.Split(), connection, dataToSync);
            }
        }
        
        public void SaveDataToSync(IJobHandler handler, List<GDFPlayerDataStorage> dataToSync)
        {
            handler.StepAmount = 2;
            using (IDBConnection connection = _mainConnection.Open())
            {
                PlayerDataToSyncDAL.Instance.Validate(handler.Split(), connection);
                PlayerDataToSyncDAL.Instance.Record(handler.Split(), connection, dataToSync);
            }
        }
        
        public void RemoveDataToSync(IJobHandler handler, List<GDFPlayerDataStorage> dataToSync)
        {
            if (dataToSync == null)
            {
                return;
            }

            using (IDBConnection connection = _mainConnection.Open())
            {
                PlayerDataToSyncDAL.Instance.Validate(handler.Split(), connection);
                PlayerDataToSyncDAL.Instance.DeleteData(handler.Split(), connection, dataToSync);
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
            string path = GetFolderPath(GDF.Account.Identity);
            Directory.Delete(path, true);
            Directory.CreateDirectory(path);
        }

        public void Purge(IJobHandler handler, byte gameSave)
        {
            string path = GetFilePath(GDF.Account.Identity, gameSave);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public void MigrateOnline(IJobHandler handler)
        {
            string localPath = GetFolderPath(GDF.Account.LocalIdentity);
            string onlinePath = GetFolderPath(GDF.Account.Identity);
            
            CopyDirectory(handler, localPath, onlinePath);
        }
        
        public void MigrateOffline(IJobHandler handler)
        {
            string localPath = GetFolderPath(GDF.Account.LocalIdentity);
            string onlinePath = GetFolderPath(GDF.Account.Identity);
            
            CopyDirectory(handler, onlinePath, localPath);
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

        private string GetFolderPath(string identity)
        {
            return Path.Combine(GDFUserSettings.Instance.GenerateContainerName(GDFUserSettings.EnvironmentContainer(_engine)), identity);
        }

        private string GetFilePath(string identity)
        {
            _root = GetFolderPath(identity);
            if (!Directory.Exists(_root))
            {
                Directory.CreateDirectory(_root);
            }

            return Path.Combine(_root, "global.data");
        }

        private string GetFilePath(string identity, byte gameSave)
        {
            _root = GetFolderPath(identity);
            if (!Directory.Exists(_root))
            {
                Directory.CreateDirectory(_root);
            }

            return Path.Combine(_root, gameSave + ".save");
        }

        private void CopyDirectory(IJobHandler handler, string source, string destination)
        {
            string[] files = Directory.GetFiles(source);
            
            handler.StepAmount = files.Length + 3;
            handler.Step();

            Directory.Delete(destination, true);
            handler.Step();
            Directory.CreateDirectory(destination);
            handler.Step();

            foreach(string file in files)
            {
                string fileName = Path.GetFileName(file);
                File.Copy(file, Path.Combine(destination, fileName), true);
                handler.Step();
            }
        }

    }
}