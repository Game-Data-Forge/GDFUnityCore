#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IApiResult.cs create at 2025/05/06 17:05:33
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    /// <summary>
    /// Represents the result of an API operation.
    /// </summary>
    public interface IApiResult
    {
        #region Instance fields and properties

        /// <summary>
        /// Gets or sets the status of an API operation.
        /// </summary>
        /// <remarks>
        /// This property contains a textual representation of the API operation's status.
        /// Common values might include "ok" or error messages describing the operation's result.
        /// </remarks>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the API operation was successful.
        /// </summary>
        /// <remarks>
        /// This property returns a <see cref="bool" />, where <see langword="true" /> indicates success
        /// and <see langword="false" /> indicates failure.
        /// </remarks>
        public bool Success { get; set; }

        #endregion
    }
}