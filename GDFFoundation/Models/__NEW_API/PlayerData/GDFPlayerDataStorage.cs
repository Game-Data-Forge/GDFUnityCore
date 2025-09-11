#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFPlayerDataStorage.cs create at 2025/04/08 15:04:53
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Class for storing player data.
    /// </summary>
    [Serializable]
    public class GDFPlayerDataStorage : IGDFStorageData, IWritableFieldAccount, IGDFWritableStringReference, IGDFRangedData
    {
        #region Instance fields and properties

        /// <summary>
        ///     Represents a game save for a player.
        /// </summary>
        public byte GameSave { set; get; } = 0;

        /// <summary>
        ///     Class for storing player data.
        /// </summary>
        public GDFPlayerDataProcessKind Process { set; get; } = GDFPlayerDataProcessKind.None;

        #region From interface IGDFRangedData

        public int Range { get; set; }

        #endregion

        #region From interface IGDFStorageData

        public int Channels { get; set; }

        [GDFDbLength(256)]
        public string ClassName { get; set; } = string.Empty;

        [GDFDbAccess(updateAccess: GDFDbColumnAccess.Deny)]
        public DateTime Creation { get; set; }

        public long DataVersion { get; set; }

        public string Json { get; set; } = string.Empty;

        public DateTime Modification { get; set; }
        public long Project { get; set; }

        public long RowId { get; set; }

        /// <summary>
        ///     Represents a player data storage.
        /// </summary>
        public int Storage { get; set; } = 1;

        [Obsolete("Only use SyncDateTime")] public long SyncCommit { get; set; }

        [GDFDbIndex] public DateTime SyncDateTime { get; set; }

        public bool Trashed { get; set; }

        #endregion

        #region From interface IGDFWritableAccountData

        public long Account { get; set; }

        #endregion

        #region From interface IGDFWritableStringReference

        [GDFDbLength(50)] public string Reference { get; set; } = string.Empty;

        #endregion

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents a player data storage entity.
        /// </summary>
        public GDFPlayerDataStorage()
        {
        }

        #endregion

        #region Instance methods

        /// <summary>
        ///     Copies the values of the given GDFPlayerDataStorage object into the current object.
        /// </summary>
        /// <param name="sOther">The GDFPlayerDataStorage object from which to copy the values.</param>
        public void CopyFrom(GDFPlayerDataStorage sOther)
        {
            Project = sOther.Project;
            Reference = sOther.Reference;
            Account = sOther.Account;
            Range = sOther.Range;
            Storage = sOther.Storage;
            GameSave = sOther.GameSave;
            ClassName = sOther.ClassName;
            Json = sOther.Json;
            Process = sOther.Process;
            Creation = sOther.Creation;
            Modification = sOther.Modification;
            Trashed = sOther.Trashed;
            RowId = sOther.RowId;
            DataVersion = sOther.DataVersion;
            SyncDateTime = sOther.SyncDateTime;
            SyncCommit = sOther.SyncCommit;
            Channels = sOther.Channels;
        }

        #region From interface IGDFStorageData

        public object GetReference()
        {
            return Reference;
        }

        #endregion

        #endregion
    }
}