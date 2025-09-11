#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IGDFWritableAccountData.cs create at 2025/03/26 17:03:59
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    public interface IWritableFieldAccount : IGDFWritableData, IFieldAccount
    {
        #region Instance fields and properties

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        [GDFDbUnique(constraintName = "Identity")]
        public new long Account { get; set; }

        #endregion
    }
}