#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IGDFDbStorage.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    public interface IGDFDbStorage : IGDFIdStorage, IFieldProject
    {
        #region Instance fields and properties

        /// <summary>
        /// Represents the property <see cref="Project"/>, which is commonly used for identifying projects
        /// across entities implementing <see cref="IGDFDbStorage"/>.
        /// </summary>
        /// <remarks>
        /// This property is marked with specific attributes such as <see cref="GDFDbAccessAttribute"/>
        /// and <see cref="GDFDbUniqueAttribute"/> to define its database access rules and
        /// uniqueness constraints.
        /// - <see cref="GDFDbAccessAttribute"/> specifies that the property is restricted from updates in the database context.
        /// - <see cref="GDFDbUniqueAttribute"/> enforces a uniqueness constraint for this property in the database.
        /// The type of the property is defined as <see cref="long"/>.
        /// </remarks>
        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        [GDFDbUnique(constraintName = "Identity")]
        public new long Project { get; set; }

        #endregion
    }
}