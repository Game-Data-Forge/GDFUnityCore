#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFUserData.cs create at 2025/04/03 09:04:09
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Base class for user data in the GDF system.
    /// </summary>
    [Serializable]
    public abstract class GDFUserData : IWritableFieldAccount, IGDFWritableSyncableData, IGDFWritableStringReference
    {
        #region Instance fields and properties

        /// <summary>
        ///     Represents a process kind for user data.
        /// </summary>
        public GDFUserDataProcessKind Process { set; get; } = GDFUserDataProcessKind.None;

        #region From interface IGDFWritableAccountData

        /// **Account***
        /// *Namespace:** GDFFoundation
        public long Account { set; get; }

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public DateTime Creation { get; set; }

        public long DataVersion { get; set; }

        public DateTime Modification { get; set; }

        public bool Trashed { get; set; }

        #endregion

        #region From interface IGDFWritableStringReference

        public string Reference { get; set; }

        #endregion

        #region From interface IGDFWritableSyncableData

        /// <summary>
        ///     The channel the data is accessible from.
        /// </summary>
        public int Channels { set; get; } = 0;

        /// <summary>
        ///     Interface to indicate that a class has a SyncCommit property.
        /// </summary>
        public long SyncCommit { set; get; }

        /// <summary>
        ///     Gets or sets the synchronization datetime.
        /// </summary>
        /// <value>The synchronization datetime.</value>
        public DateTime SyncDateTime { get; set; }

        #endregion

        #endregion

        #region Instance methods

        /// <summary>
        ///     Copy the data from another GDFUserData object to this object.
        /// </summary>
        /// <param name="sOther">The GDFUserData object to copy from</param>
        public void CopyFrom(GDFUserData sOther)
        {
            Account = sOther.Account;
            Reference = sOther.Reference;
            Channels = sOther.Channels;
            Process = sOther.Process;
            Creation = sOther.Creation;
            Modification = sOther.Modification;
            SyncDateTime = sOther.SyncDateTime;
            SyncCommit = sOther.SyncCommit;
            DataVersion = sOther.DataVersion;
            Trashed = sOther.Trashed;
        }

        #region From interface IGDFWritableAccountData

        public object GetReference()
        {
            return Reference;
        }

        #endregion

        #endregion
    }
}