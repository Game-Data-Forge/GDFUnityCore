#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFAccountDataManagement.csproj TokenException.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System.Net;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents an exception specific to token handling within the GDFAccountDataManagement namespace.
    /// </summary>
    /// <remarks>
    ///     This exception extends the APIException class and is used to signify errors related to token operations.
    ///     It builds upon the error handling mechanisms provided by the base classes, adding context specific to token management.
    /// </remarks>
    public class TokenException : APIException
    {
        #region Static fields and properties

        /// <summary>
        ///     Represents an error related to account token operations in the system.
        /// </summary>
        /// <remarks>
        ///     This property is utilized to create a specific instance of <see cref="TokenException" />
        ///     when an issue occurs during account token management, such as failures in token
        ///     creation, deletion, or retrieval. The error is classified under the
        ///     "TKN" category with a specific error number for identification.
        /// </remarks>
        /// <seealso cref="TokenException" />
        static public TokenException AccountTokenError => new TokenException(1, $"Account token error.");

        /// <summary>
        ///     Represents the default instance of a <see cref="TokenException" /> with predefined settings.
        /// </summary>
        /// <remarks>
        ///     The <c>DefaultException</c> is a static property that provides a default configuration for a <see cref="TokenException" />.
        ///     It is initialized with an error number of <c>0</c> and a generic message indicating a default TokenException.
        /// </remarks>
        public static TokenException DefaultException => new TokenException(0, $"Default {nameof(TokenException)}");

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents an exception specific to token-related operations within the application.
        ///     Inherits from <see cref="APIException" /> to handle API-related errors.
        /// </summary>
        public TokenException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_TOKEN_EXCEPTION_CATEGORY, GDFConstants.K_TOKEN_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }

        #endregion
    }
}