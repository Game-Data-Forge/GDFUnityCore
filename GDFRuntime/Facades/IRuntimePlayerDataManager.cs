using System;
using System.Collections.Generic;
using GDFFoundation;

namespace GDFRuntime
{
    public interface IRuntimePlayerDataManager : IAsyncManager
    {
        public bool HasDataToSync { get; }
        public int DataToSyncAmount { get; }
        public byte ActiveGameSave { get; set; }

        public Notification Syncing { get; }
        public Notification Synced { get; }
        public Notification<byte> Loading { get; }
        public Notification<byte> Loaded { get; }
        public Notification<byte> Unloading { get; }
        public Notification<byte> Unloaded { get; }
        public Notification<byte> Saving { get; }
        public Notification<byte> Saved { get; }
        public Notification<byte> Deleting { get; }
        public Notification<byte> Deleted { get; }
        public Notification<byte, byte> Duplicating { get; }
        public Notification<byte, byte> Duplicated { get; }

        public Job<List<byte>> GetGameSaves();

        public Job LoadGameSave(byte gameSave);
        public Job UnloadGameSave(byte gameSave, bool saveBeforeUnload = false);
        public Job DeleteGameSave(byte gameSave);

        public GameSaveState GetGameSaveState(byte gameSave);

        public void Add(GDFPlayerData data);
        public void Add(byte gameSave, GDFPlayerData data);
        public void Add(string reference, GDFPlayerData data);
        public void Add(byte gameSave, string reference, GDFPlayerData data);

        public GDFPlayerData Get(string reference);
        public GDFPlayerData Get(byte gameSave, string reference);
        public GDFPlayerData Get(Type type, string reference);
        public GDFPlayerData Get(byte gameSave, Type type, string reference);

        public T Get<T>(string reference) where T : GDFPlayerData;
        public T Get<T>(byte gameSave, string reference) where T : GDFPlayerData;
        public List<GDFPlayerData> Get(Type type, bool includeInherited = true);
        public List<GDFPlayerData> Get(byte gameSave, Type type, bool includeInherited = true);
        public List<T> Get<T>(bool includeInherited = true) where T : GDFPlayerData;
        public List<T> Get<T>(byte gameSave, bool includeInherited = true) where T : GDFPlayerData;

        public DataState GetDataState(GDFPlayerData data);
        
        public bool HasDataToSave();
        public bool HasDataToSave(byte gameSave);
        public int DataToSaveAmount();
        public int DataToSaveAmount(byte gameSave);

        public void AddToSaveQueue(GDFPlayerData data);

        public void Delete(GDFPlayerData data);

        public Job Save();
        public Job Save(byte gameSave);
        public Job DuplicateGameSave(byte copyGameSave, byte pasteGameSave);
        public Job Sync();

        public Job Purge();
        
        public Job MigrateOnline();
        public Job MigrateOffline();
    }
}