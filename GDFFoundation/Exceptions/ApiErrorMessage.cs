#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj ApiErrorMessage.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    /// Represents a structured message for API error responses.
    /// </summary>
    /// <remarks>
    /// This struct is used to standardize error messages returned by the API and provide relevant information such as status codes, error codes, error messages, and optional help links.
    /// </remarks>
    [Serializable]
    public struct ApiErrorMessage
    {
        /// <summary>
        /// Gets or sets the status code related to the API error message.
        /// </summary>
        /// <remarks>
        /// This property represents the HTTP status code corresponding to the error described in the <see cref="ApiErrorMessage"/>.
        /// It can be used to identify the type of response and to handle errors based on the HTTP status codes.
        /// </remarks>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the inner code associated with the API error message.
        /// </summary>
        /// <remarks>
        /// This property represents a specific error code within the context of an API response.
        /// It is used to provide more granular information about the error and can be utilized to map to specific error handling logic.
        /// </remarks>
        public string InnerCode { get; set; }

        /// <summary>
        /// Gets or sets the error message associated with an API error response.
        /// </summary>
        /// <remarks>
        /// This property contains the descriptive message explaining the nature of the error returned in an <see cref="ApiErrorMessage"/>.
        /// It provides specific information about what caused the error, which can be used for debugging or displaying user-friendly error information.
        /// </remarks>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the help link or additional information associated with the error message.
        /// </summary>
        /// <remarks>
        /// This property provides a link or text information that may assist users or developers in understanding the error or resolving it.
        /// It is commonly used in conjunction with other properties such as <see cref="ApiErrorMessage.StatusCode"/> and <see cref="ApiErrorMessage.Message"/>
        /// to provide comprehensive error details in API responses.
        /// </remarks>
        public string Help { get; set; }
    }
}