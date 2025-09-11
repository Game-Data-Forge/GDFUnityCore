#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IGDFProjectKey.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    /// <summary>
    ///     Represents a project key for a specific GDF project.
    /// </summary>
    public interface IGDFProjectKey
    {
        #region Instance methods

        /// <summary>
        ///     Retrieves the project key for a given project ID and environment kind.
        /// </summary>
        /// <param name="projectReference">The project ID.</param>
        /// <param name="environmentKind">The environment kind.</param>
        /// <returns>The project key as a string.</returns>
        public string GetProjectKey(long projectReference, ProjectEnvironment environmentKind);

        /// <summary>
        ///     Retrieves the name of the project key instance.
        /// </summary>
        /// <returns>The name of the project key instance.</returns>
        [Obsolete("to remove")]
        public string GetProjectKeyInstanceName();

        #endregion
    }
}