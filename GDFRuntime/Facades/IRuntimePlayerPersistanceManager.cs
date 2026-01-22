using System;
using System.Collections.Generic;
using GDFFoundation;

namespace GDFRuntime
{
    public interface IRuntimePlayerPersistanceManager
    {
        public List<byte> GetGameSaves(IJobHandler handler);
        public bool Exists(byte gameSave);

        public void Load(IJobHandler handler, byte gameSave, List<GDFPlayerDataStorage> data);
        public void Save(IJobHandler handler, byte gameSave, List<GDFPlayerDataStorage> data);
        public void Save(IJobHandler handler, List<GDFPlayerDataStorage> data);
        public void Copy(IJobHandler handler, byte srcGameSave, byte dstGameSave);
        
        public void Purge(IJobHandler handler);
        public void Purge(IJobHandler handler, byte gameSave);

        public void LoadDataToSync(IJobHandler handler, List<GDFPlayerDataStorage> dataToSync);
        public void SaveDataToSync(IJobHandler handler, List<GDFPlayerDataStorage> dataToSync);
        public void RemoveDataToSync(IJobHandler handler, List<GDFPlayerDataStorage> dataDataToSync);

        public PlayerStorageInformation LoadInformation(IJobHandler handler);
        public void SaveInformation(IJobHandler handler, PlayerStorageInformation information);

        public void MigrateOnline(IJobHandler handler);
        public void MigrateOffline(IJobHandler handler);
    }
}