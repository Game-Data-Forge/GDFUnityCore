#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFLocalPlayerData.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public abstract class GDFLocalPlayerData : IGDFDbStorage, IWritableFieldAccount, IGDFWritableLongReference
    {
        #region Instance fields and properties

        public long AccountReference { get; set; }

        #region From interface IGDFDbStorage

        public long Project { get; set; }
        public long RowId { get; set; }

        #endregion

        #region From interface IGDFWritableAccountData

        public long Account { get; set; }

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public DateTime Creation { get; set; }

        public long DataVersion { get; set; }

        public DateTime Modification { get; set; }
        public bool Trashed { get; set; }

        #endregion

        #region From interface IGDFWritableLongReference

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public long Reference { get; set; }

        #endregion

        #endregion

        #region Instance methods

        #region From interface IGDFWritableAccountData

        public object GetReference() => Reference;

        #endregion

        #endregion
    }
}