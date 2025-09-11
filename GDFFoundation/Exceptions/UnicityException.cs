#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFUnicityDataManagement.csproj UnicityException.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System.Net;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents an exception specific to the Unicity Data Management system.
    ///     This exception is used for handling errors within the Unicity context, providing
    ///     detailed information such as error number, message, and optional help text.
    /// </summary>
    public class UnicityException : APIException
    {
        #region Static fields and properties

        /// <summary>
        ///     Represents a specific exception indicating a concurrency issue where a request is already in progress.
        /// </summary>
        /// <remarks>
        ///     This static property returns an instance of <see cref="UnicityException" /> with a pre-defined error code and message.
        ///     It is used to signal cases when a concurrent operation is detected, preventing duplicate or overlapping processes.
        /// </remarks>
        /// <value>
        ///     An instance of <see cref="UnicityException" /> with error number 1 and an appropriate message.
        /// </value>
        static public UnicityException ConcurrenceError => new UnicityException(1, $"Request already in progress.");

        /// <summary>
        ///     Gets the default instance of the <see cref="UnicityException" /> class.
        ///     The default exception represents a general, internally defined Unicity exception
        ///     with an error number of 0 and a message indicating it is the default exception.
        /// </summary>
        public static UnicityException DefaultException => new UnicityException(0, $"Default {nameof(UnicityException)}");

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents an exception type used specifically in the context of Unicity data management, inheriting from APIException.
        /// </summary>
        /// <remarks>
        ///     This exception is thrown when specific conditions related to Unicity operations occur.
        ///     It provides additional details about the exception by including an error category and error number.
        ///     The exception defaults to an HTTP status code of InternalServerError.
        /// </remarks>
        public UnicityException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_UNICITY_EXCEPTION_CATEGORY, GDFConstants.K_UNICITY_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }

        #endregion
    }
}