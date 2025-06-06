#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IGDFAccountServiceDao.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System.Collections.Generic;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     IGDFAccountServiceDao interface represents a data access object for interacting with the account service data in the GDFFoundation.
    /// </summary>
    public interface IGDFAccountServiceDao : IGDFDao
    {
        #region Instance methods

        /// <summary>
        ///     Creates a new GDFAccountService object.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <param name="sModel">The GDFAccountService model.</param>
        /// <returns>The created GDFAccountService object.</returns>
        public GDFAccountService Create(ProjectEnvironment sEnvironment, long sProjectReference, GDFAccountService sModel);

        /// <summary>
        ///     Deletes a record from the database.
        /// </summary>
        /// <param name="sEnvironment">The environment in which the deletion should take place.</param>
        /// <param name="sProjectReference">The ID of the project to which the record belongs.</param>
        /// <param name="sReference">The reference of the record to delete.</param>
        public void Delete(ProjectEnvironment sEnvironment, long sProjectReference, long sReference);

        /// <summary>
        ///     Retrieves a list of GDFAccountService objects based on the specified environment and project ID.
        /// </summary>
        /// <param name="sEnvironment">The environment kind to search for.</param>
        /// <param name="sProjectReference">The project ID to search for.</param>
        /// <returns>A list of GDFAccountService objects.</returns>
        public List<GDFAccountService> FindAll(ProjectEnvironment sEnvironment, long sProjectReference);

        /// <summary>
        ///     Finds all modified GDFAccountService objects based on the specified parameters.
        /// </summary>
        /// <param name="sEnvironment">The environment kind to filter the results by.</param>
        /// <param name="sProjectReference">The project ID to filter the results by.</param>
        /// <param name="sSyncDate">The sync date to filter the results by.</param>
        /// <returns>
        ///     A list of GDFAccountService objects that match the specified criteria. Returns an empty list if no matches are found.
        /// </returns>
        public List<GDFAccountService> FindAllModified(ProjectEnvironment sEnvironment, long sProjectReference, long sSyncDate);

        /// <summary>
        ///     Retrieves a list of GDFAccountService objects based on the specified parameters.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <param name="sWhereClause">The list of database where models for filtering.</param>
        /// <returns>A list of GDFAccountService objects.</returns>
        public List<GDFAccountService> GetBy(ProjectEnvironment sEnvironment, long sProjectReference, List<GDFDatabaseWhereModel> sWhereClause);

        /// <summary>
        ///     Retrieves an GDFAccountService object by reference.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <param name="sReference">The reference value.</param>
        /// <returns>The GDFAccountService object if found, otherwise null.</returns>
        public GDFAccountService GetByReference(ProjectEnvironment sEnvironment, long sProjectReference, long sReference);

        /// <summary>
        ///     Inserts or updates a GDFAccountService model in the specified environment and project.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <param name="sModel">The GDFAccountService model to insert or update.</param>
        /// <returns>Returns the updated or inserted GDFAccountService model.</returns>
        public GDFAccountService InsertOrUpdate(ProjectEnvironment sEnvironment, long sProjectReference, GDFAccountService sModel);

        /// <summary>
        ///     Updates the GDFAccountService object in the specified environment and project.
        /// </summary>
        /// <param name="sEnvironment">The environment in which the update operation will be performed.</param>
        /// <param name="sProjectReference">The ID of the project.</param>
        /// <param name="sModel">The GDFAccountService object to be updated.</param>
        /// <returns>The updated GDFAccountService object.</returns>
        public GDFAccountService Update(ProjectEnvironment sEnvironment, long sProjectReference, GDFAccountService sModel);

        #endregion
    }
}