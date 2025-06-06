#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IGDFAccountTokenDao.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System.Collections.Generic;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Interface defining methods for accessing and manipulating GDFAccountToken objects in a data store.
    /// </summary>
    public interface IGDFAccountTokenDao : IGDFDao
    {
        #region Instance methods

        /// <summary>
        ///     Creates a new GDFAccountToken object.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project Id.</param>
        /// <param name="sModel">The GDFAccountToken to create.</param>
        /// <returns>The created GDFAccountToken object.</returns>
        public GDFAccountToken Create(ProjectEnvironment sEnvironment, long sProjectReference, GDFAccountToken sModel);

        /// <summary>
        ///     Deletes an account token.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <param name="sReference">The reference ID.</param>
        public void Delete(ProjectEnvironment sEnvironment, long sProjectReference, long sReference);

        /// <summary>
        ///     Finds all GDFAccountToken objects based on the given environment and project ID.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <returns>A list of GDFAccountToken objects.</returns>
        public List<GDFAccountToken> FindAll(ProjectEnvironment sEnvironment, long sProjectReference);

        /// <summary>
        ///     Find all modified account tokens based on the provided environment, project ID, and sync date.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <param name="sSyncDate">The sync date.</param>
        /// <returns>A list of modified account tokens.</returns>
        public List<GDFAccountToken> FindAllModified(ProjectEnvironment sEnvironment, long sProjectReference, long sSyncDate);

        /// <summary>
        ///     Retrieves a list of GDFAccountToken objects based on the specified criteria.
        /// </summary>
        /// <param name="sEnvironment">The GDFEnvironmentKind value representing the environment.</param>
        /// <param name="sProjectReference">The project ID to filter the results.</param>
        /// <param name="sWhereClause">A list of GDFDatabaseWhereModel objects representing the WHERE clause criteria.</param>
        /// <returns>A list of GDFAccountToken objects matching the specified criteria.</returns>
        public List<GDFAccountToken> GetBy(ProjectEnvironment sEnvironment, long sProjectReference, List<GDFDatabaseWhereModel> sWhereClause);

        /// <summary>
        ///     Inserts or updates a GDFAccountToken object in the database.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <param name="sModel">The GDFAccountToken object to insert or update.</param>
        /// <returns>The inserted or updated GDFAccountToken object.</returns>
        public GDFAccountToken InsertOrUpdate(ProjectEnvironment sEnvironment, long sProjectReference, GDFAccountToken sModel);

        /// <summary>
        ///     Updates the GDFAccountToken model in the specified environment and project.
        /// </summary>
        /// <param name="sEnvironment">The environment in which the update operation is performed.</param>
        /// <param name="sProjectReference">The ID of the project.</param>
        /// <param name="sModel">The GDFAccountToken model to be updated.</param>
        /// <returns>The updated GDFAccountToken model.</returns>
        public GDFAccountToken Update(ProjectEnvironment sEnvironment, long sProjectReference, GDFAccountToken sModel);

        #endregion
    }
}