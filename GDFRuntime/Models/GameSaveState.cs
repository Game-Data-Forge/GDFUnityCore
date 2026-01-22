namespace GDFRuntime
{
    public enum GameSaveState : byte
    {
        Inexistant = 0,
        Unloaded = 1,
        Loading = 2,
        Loaded = 3,
        Saving = 4,
        Duplicating = 5,
        Syncing = 6,
        Unloading = 7,
        Deleting = 8
    }
}