#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IGDFCrucialInformationDao.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System.Collections.Generic;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Interface for accessing crucial information data.
    /// </summary>
    public interface IGDFCrucialInformationDao : IGDFDao
    {
        #region Instance methods

        /// <summary>
        ///     Creates a new instance of GDFCrucialInformation with the given environment, project ID and model.
        /// </summary>
        /// <param name="sEnvironment">The environment of the GDF server.</param>
        /// <param name="sProjectReference">The ID of the project.</param>
        /// <param name="sModel">The model to create.</param>
        /// <returns>A new instance of GDFCrucialInformation.</returns>
        public GDFCrucialInformation Create(ProjectEnvironment sEnvironment, long sProjectReference, GDFCrucialInformation sModel);

        /// <summary>
        ///     Deletes a crucial information based on the given environment, project ID, and reference.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID of the crucial information.</param>
        /// <param name="sReference">The reference of the crucial information.</param>
        public void Delete(ProjectEnvironment sEnvironment, long sProjectReference, long sReference);

        /// <summary>
        ///     Finds all GDFCrucialInformation objects in the specified environment and project.
        /// </summary>
        /// <param name="sEnvironment">The environment to search in.</param>
        /// <param name="sProjectReference">The project ID to search for.</param>
        /// <returns>A list of GDFCrucialInformation objects found in the specified environment and project.</returns>
        public List<GDFCrucialInformation> FindAll(ProjectEnvironment sEnvironment, long sProjectReference);

        /// <summary>
        ///     Finds all modified GDFCrucialInformation objects based on the specified environment, project ID, and sync date.
        /// </summary>
        /// <param name="sEnvironment">The environment kind to filter by.</param>
        /// <param name="sProjectReference">The project ID to filter by.</param>
        /// <param name="sSyncDate">The sync date to filter by.</param>
        /// <returns>A list of GDFCrucialInformation objects that have been modified.</returns>
        public List<GDFCrucialInformation> FindAllModified(ProjectEnvironment sEnvironment, long sProjectReference, long sSyncDate);

        /// <summary>
        ///     Retrieves a list of GDFCrucialInformation objects based on the specified parameters.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <param name="sWhereClause">The where clause for filtering the results.</param>
        /// <returns>A list of GDFCrucialInformation objects.</returns>
        public List<GDFCrucialInformation> GetBy(ProjectEnvironment sEnvironment, long sProjectReference, List<GDFDatabaseWhereModel> sWhereClause);

        /// <summary>
        ///     Inserts or updates a crucial information model.
        /// </summary>
        /// <param name="sEnvironment">The environment kind.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <param name="sModel">The crucial information model to insert or update.</param>
        /// <returns>The inserted or updated crucial information model.</returns>
        public GDFCrucialInformation InsertOrUpdate(ProjectEnvironment sEnvironment, long sProjectReference, GDFCrucialInformation sModel);

        /// <summary>
        ///     Generates a new valid reference number.
        /// </summary>
        /// <param name="sEnvironmentKind">The environment kind to generate the reference for.</param>
        /// <param name="sProjectReference">The project ID associated with the reference (Optional).</param>
        /// <returns>A long value representing the new valid reference number.</returns>
        public long NewValidReference(ProjectEnvironment sEnvironmentKind, long sProjectReference);

        /// <summary>
        ///     Updates the GDFCrucialInformation object with the given environment, project ID, and model.
        /// </summary>
        /// <param name="sEnvironment">The environment kind of the crucial information.</param>
        /// <param name="sProjectReference">The ID of the project.</param>
        /// <param name="sModel">The model to update.</param>
        /// <returns>The updated GDFCrucialInformation object.</returns>
        public GDFCrucialInformation Update(ProjectEnvironment sEnvironment, long sProjectReference, GDFCrucialInformation sModel);

        #endregion
    }
}