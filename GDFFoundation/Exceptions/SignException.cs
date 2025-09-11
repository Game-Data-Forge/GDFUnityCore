#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFAccountDataManagement.csproj SignException.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System.Net;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents a custom exception specifically for account sign-related errors.
    /// </summary>
    /// <remarks>
    ///     This exception extends the <see cref="APIException" /> class and provides additional
    ///     context for handling errors within the account sign module of the application.
    /// </remarks>
    public class SignException : APIException
    {
        #region Static fields and properties

        /// <summary>
        /// Provides a predefined <see cref="SignException" /> that represents a conflict error
        /// when a <see cref="GDFAccountSign" /> already exists in the system.
        /// </summary>
        /// <remarks>
        /// This property is used to throw a <see cref="SignException" /> with the HTTP status code 409 (Conflict)
        /// and an appropriate error message when attempting to add a duplicate <see cref="GDFAccountSign" />.
        /// </remarks>
        public static SignException AccountSignAlreadyExist => new SignException(HttpStatusCode.Conflict, 4, $"{nameof(GDFAccountSign)} already exists!");

        /// <summary>
        /// Represents an exception thrown when a rescue operation on a <see cref="GDFAccountSign" /> already exists in the system.
        /// </summary>
        /// <remarks>
        /// This exception is used to indicate a conflict caused by attempting to process a rescue sign-up or add operation on an
        /// already existing <see cref="GDFAccountSign" />. It inherits from the <see cref="SignException" /> class and is associated
        /// with the HTTP status code 409 (Conflict).
        /// </remarks>
        public static SignException AccountSignRescueAlreadyExist => new SignException(HttpStatusCode.Conflict, 41, $"{nameof(GDFAccountSign)} already exists!");

        /// <summary>
        /// Represents an exception thrown when the insertion of a <see cref="GDFAccountSign"/> fails due to a null reference or internal error.
        /// </summary>
        /// <remarks>
        /// This exception is used to indicate that a <see cref="GDFAccountSign"/> was not successfully inserted into the system.
        /// It inherits from the <see cref="SignException"/> class and is associated with the HTTP status code 500 (Internal Server Error).
        /// </remarks>
        public static SignException AccountSignInsertedIsNull => new SignException(HttpStatusCode.InternalServerError, 2, $"Failed to insert a {nameof(GDFAccountSign)}!");

        /// <summary>
        /// Provides a predefined <see cref="SignException" /> that represents an error
        /// when the <see cref="GDFAccountSign" /> to be inserted is null or undefined.
        /// </summary>
        /// <remarks>
        /// This property is used to throw a <see cref="SignException" /> with the HTTP status code 500 (Internal Server Error)
        /// and an appropriate error message when the application encounters a failure due to a null <see cref="GDFAccountSign" />
        /// during an insertion operation.
        /// </remarks>
        public static SignException AccountSignToInsertIsNull => new SignException(HttpStatusCode.InternalServerError, 3, $"Failed to insert a {nameof(GDFAccountSign)}!");

        /// <summary>
        /// Provides a predefined <see cref="SignException" /> that represents a forbidden error
        /// when a <see cref="GDFAccountSign" /> is marked as unknown or invalid in the system.
        /// </summary>
        /// <remarks>
        /// This property is used to throw a <see cref="SignException" /> with the HTTP status code 403 (Forbidden)
        /// and an appropriate error message when a <see cref="GDFAccountSign" /> cannot be processed due to being unknown or unrecognized.
        /// </remarks>
        public static SignException AccountSignUnknown => new SignException(HttpStatusCode.Forbidden, 5, $"{nameof(GDFAccountSign)} unknown!");

        /// <summary>
        ///     Represents a specific <see cref="SignException" /> thrown when mandatory consent is not provided.
        /// </summary>
        /// <remarks>
        ///     Defined as a static property within the <see cref="SignException" /> class. This exception is
        ///     constructed with an HTTP status of <see cref="HttpStatusCode.Forbidden" />, an error number of 1,
        ///     and a descriptive message indicating that consent must be true for the given operation.
        /// </remarks>
        public static SignException ConsentException => new SignException(HttpStatusCode.Forbidden, 1, $"{nameof(GDFAccountSign)} insertion error: Consent must be true!");

        /// <summary>
        ///     Represents the default exception instance of the <see cref="SignException" /> class.
        ///     This property provides a predefined instance of the <see cref="SignException" />
        ///     configured with an Internal Server Error status code, a default error number of 0,
        ///     and a generic default message.
        /// </summary>
        public static SignException DefaultException => new SignException(HttpStatusCode.InternalServerError, 0, $"Default {nameof(SignException)}");

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        /// Represents an exception specific to signing operations within the <see cref="GDFAccountDataManagement"/> namespace.
        /// Inherits from <see cref="APIException"/> to provide additional details for errors encountered
        /// during account signing processes, such as conflicts or missing data.
        /// </summary>
        /// <remarks>
        /// This class provides predefined static instances of <see cref="SignException"/> for common signing-related errors,
        /// such as pre-existing signatures, null insertion attempts, or missing consent.
        /// It facilitates a structured approach to handling exceptions in the account signing module.
        /// </remarks>
        public SignException(HttpStatusCode status, ushort errorNumber, string message, string help = "")
            : base(status, GDFConstants.K_SIGN_EXCEPTION_CATEGORY, GDFConstants.K_SIGN_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }

        #endregion
    }
}