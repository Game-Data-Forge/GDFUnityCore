using GDFFoundation;

namespace GDFUnity
{
    public class PlayerDataJob
    {
        public enum Type : byte
        {
            Loading = 0,
            Unloading = 1,
            Deleting = 2,
            Saving = 3,
            Syncing = 4,
            Duplicating = 5
        }

        public Type type;
        public Job job;
    }
}