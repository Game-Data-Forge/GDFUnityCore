using System.Collections.Generic;

namespace GDFEditor
{
    /// <summary>
    /// Represents the payload for synchronizing metadata.
    /// </summary>
    public class GDFUpPayloadSyncMetaData : GDFUpPayloadSyncEditor
    {
        /// <summary>
        /// Represents a list of metadata information for a project.
        /// </summary>
        public List<GDFMetaData> MetaDataList { set; get; } = new List<GDFMetaData>();

        /// <summary>
        /// Represents the ID of a project.
        /// </summary>
        public long ProjectReference { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the track associated with the synchronization payload.
        /// </summary>
        public int TrackId { set; get; }
    }
}