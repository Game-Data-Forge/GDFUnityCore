

namespace GDFEditor
{
    /// <summary>
    /// Represents a switch lock payload metadata used in the GDFRequestEditor class.
    /// </summary>
    public class GDFUpPayloadMetaDataSwitchLock : GDFUpPayloadEditor
    {
        /// <summary>
        /// Represents the metadata information that can be unlocked.
        /// </summary>
        public GDFMetaData MetaDataToUnlock { set; get; }

        /// <summary>
        /// Represents the reference to the metadata that will be locked.
        /// </summary>
        public long MetaDataToLockReference { set; get; }

        /// <summary>
        /// Represents a payload metadata switch lock used in the GDFUpPayloadMetaDataSwitchLock class.
        /// </summary>
        public string LockerName { set; get; } = GDFUpPayloadMetaDataLock.K_UNKNOWN;

        /// <summary>
        /// Represents the unique identifier for tracking purposes within a <see cref="GDFUpPayloadMetaDataSwitchLock"/>.
        /// </summary>
        public int TrackId { set; get; }

        /// <summary>
        /// Represents a metadata switch lock payload used in the GDFRequestEditor class.
        /// </summary>
        public GDFUpPayloadMetaDataSwitchLock(string sLockerName, GDFMetaData sMetaDataToLock, GDFMetaData sMetaDataToUnlock)
        {
            LockerName = sLockerName;
            if (sMetaDataToLock != null)
            {
                MetaDataToLockReference = sMetaDataToLock.Reference;
            }
            MetaDataToUnlock = sMetaDataToUnlock;
        }
    }
}

