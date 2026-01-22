using System;

namespace GDFRuntime
{
    public struct DataState
    {
        public enum State
        {
            Unknown = 0,
            Cached = 1,
            Savable = 2,
            Syncable = 4
        }

        public State state;
        public DateTime? saveModificationDate;
        public DateTime? syncModificationDate;
    }
}