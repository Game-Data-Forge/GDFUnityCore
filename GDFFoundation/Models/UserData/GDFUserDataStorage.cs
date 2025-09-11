#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFUserDataStorage.cs create at 2025/04/03 09:04:09
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     GDFUserDataDataStorage class represents a storage model for user data.
    /// </summary>
    [Serializable]
    public class GDFUserDataDataStorage : IGDFStorageData, IWritableFieldAccount, IGDFWritableStringReference
    {
        #region Constants

        //public const int K_UNIT_STORAGE_SIZE = 128;

        public const int K_UNIT_MAX = 32;

        /// <summary>
        ///     Represents the size of the storage head unit in a user data storage.
        /// </summary>
        public const int K_UNIT_STORAGE_HEAD = 128;

        /// <summary>
        ///     Represents the unit storage size for user data.
        /// </summary>
        public const int K_UNIT_STORAGE_SIZE = 1024 * 2;

        #endregion

        #region Instance fields and properties

        /// <summary>
        ///     Represents a user data storage.
        /// </summary>
        public GDFUserDataProcessKind Process { set; get; } = GDFUserDataProcessKind.None;

        #region From interface IGDFStorageData

        public int Channels { get; set; }

        public string ClassName { get; set; }

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public DateTime Creation { get; set; }

        public long DataVersion { get; set; }

        public string Json { get; set; }

        public DateTime Modification { get; set; }

        public long Project { get; set; }

        public long RowId { get; set; }

        /// <summary>
        ///     Represents a data storage for user accounts.
        /// </summary>
        public int Storage { set; get; } = 1;

        public long SyncCommit { get; set; }
        public DateTime SyncDateTime { get; set; }

        public bool Trashed { get; set; }

        #endregion

        #region From interface IGDFWritableAccountData

        /// <summary>
        ///     Represents a user data storage in the GDF system.
        /// </summary>
        public long Account { set; get; }

        #endregion

        #region From interface IGDFWritableStringReference

        [GDFDbLength(50)] public string Reference { get; set; }

        #endregion

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents a storage class for user data.
        /// </summary>
        public GDFUserDataDataStorage()
        {
        }

        #endregion

        #region Instance methods

        /// <summary>
        ///     Estimates the storage required for the user data.
        /// </summary>
        /// <returns>The estimated storage size in units.</returns>
        private int StorageEstimate()
        {
            return (int)(MathF.Floor(Json.Length / K_UNIT_STORAGE_SIZE) + 1);
        }

        /// <summary>
        ///     Checks if the storage for the user data is okay.
        /// </summary>
        /// <returns>Returns true if the storage is okay, false otherwise.</returns>
        public bool StorageIsOk()
        {
            return StorageEstimate() < K_UNIT_MAX;
        }

        /// <summary>
        ///     Calculates the storage size of the GDFUserDataDataStorage object.
        /// </summary>
        public void StorageSize()
        {
            Storage = StorageEstimate();
        }

        #region From interface IGDFStorageData

        public object GetReference() => Reference;

        #endregion

        #endregion
    }
}