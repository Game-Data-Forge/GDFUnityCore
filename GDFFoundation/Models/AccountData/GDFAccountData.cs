#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFAccountData.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Data directly recorded in Database: no serialization storage.
    /// </summary>
    [Serializable]
    public abstract class GDFAccountData : IGDFDbStorage, IGDFRangedData, IWritableFieldAccount, IGDFWritableLongReference
    {
        #region Instance fields and properties

        #region From interface IGDFDbStorage

        public long Project { get; set; }

        public long RowId { get; set; }

        #endregion

        #region From interface IGDFRangedData

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public DateTime Creation { get; set; }

        public long DataVersion { get; set; }

        public DateTime Modification { get; set; }
        public int Range { get; set; }
        public bool Trashed { get; set; }

        #endregion

        #region From interface IGDFWritableAccountData

        /// <summary>
        ///     Abstract class representing an account in the system.
        /// </summary>
        [GDFDbUnique(constraintName = "Identity")]
        public long Account { set; get; }

        #endregion

        #region From interface IGDFWritableLongReference

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public long Reference { get; set; }

        #endregion

        #endregion

        #region Instance methods

        #region From interface IGDFRangedData

        public object GetReference() => Reference;

        #endregion

        #endregion
    }
}