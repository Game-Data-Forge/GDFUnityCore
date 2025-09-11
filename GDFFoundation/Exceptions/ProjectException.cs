#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFProjectDataManagement.csproj ProjectException.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System.Net;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents an exception specific to project-level concerns, extending the <see cref="APIException" /> class.
    /// </summary>
    /// <remarks>
    ///     Each instance of this exception corresponds to an error related to project operations in the system.
    ///     The exception provides additional details such as an error number and an optional help message.
    ///     It leverages predefined exception categories and indexes defined in <see cref="GDFConstants" />.
    /// </remarks>
    public class ProjectException : APIException
    {
        #region Static fields and properties

        /// <summary>
        ///     Provides a pre-defined instance of the <see cref="ProjectException" /> class representing a default exception.
        ///     The <see cref="DefaultException" /> is initialized with an error number of 0 and a generic default message.
        /// </summary>
        public static ProjectException DefaultException => new ProjectException(0, $"Default {nameof(ProjectException)}");

        /// <summary>
        ///     Represents an exception specific to the operation of retrieving project credentials
        ///     by project reference within the <see cref="ProjectCredentialsManager" /> class.
        /// </summary>
        /// <remarks>
        ///     This exception is thrown when the retrieval of project credentials
        ///     by project reference encounters an error or fails within
        ///     the <see cref="ProjectCredentialsManager.GetByProjectReferenceAndEnvironment(long, ProjectEnvironment)" /> method.
        /// </remarks>
        public static ProjectException GetByProjectException => new ProjectException(10, $"{nameof(ProjectException)}");

        /// <summary>
        ///     Represents a predefined <see cref="ProjectException" /> that is specifically thrown when an
        ///     invalid or ambiguous public key is encountered during the retrieval of project credentials.
        /// </summary>
        /// <remarks>
        ///     This property returns a <see cref="ProjectException" /> configured with a unique error number
        ///     and a detailed message indicating the context in which the exception occurred,
        ///     namely the use of <see cref="ProjectCredentialsManager.GetByPublicKey" />.
        /// </remarks>
        public static ProjectException GetByPublicKeyException => new ProjectException(11, $"{nameof(ProjectException)}");

        /// <summary>
        ///     Represents a specific exception used when an operation involving the retrieval of project credentials
        ///     by the secret key encounters an issue. This exception is thrown by
        ///     <c>ProjectCredentialsManager.GetBySecretKey</c> when certain conditions such as
        ///     invalid or ambiguous retrieval results are met.
        /// </summary>
        /// <remarks>
        ///     The exception is categorized under the project exception category and is intended to handle
        ///     errors related to the secret key retrieval process within the project credential management system.
        /// </remarks>
        public static ProjectException GetBySecretKeyException => new ProjectException(20, $"{nameof(ProjectException)}");

        
        public static ProjectException EnvironmentStatusInTransfert => new ProjectException(403, $"Project's environment is in transfert.");
        public static ProjectException EnvironmentStatusInactive => new ProjectException(404, $"Project's environment is inactive.");
        public static ProjectException StatusUpgrading => new ProjectException(405, $"Project need to be upgraded.");
        public static ProjectException StatusInactive => new ProjectException(406, $"Project is inactive.");
        public static ProjectException StatusInTransfert => new ProjectException(407, $"Project is in transfert.");
        public static ProjectException StatusFreeOverflow => new ProjectException(408, $"Project is free be the amount of request is over limit.");
        public static ProjectException StatusUnpayed => new ProjectException(409, $"Invoices are not payed! Pay to continue to use this project.");
        public static ProjectException StatusLastWarning => new ProjectException(410, $"Invoices are not payed! Pay to continue to use this project. The project is about to be deleted!");
        public static ProjectException StatusDeletable => new ProjectException(411, $"Invoices was not payed! The project is about to be deleted!");
        
        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents a custom exception specific to project-related errors within the GDFManagementTests namespace.
        ///     This exception extends the APIException class and includes predefined error categories and messages
        ///     for project-related scenarios.
        /// </summary>
        public ProjectException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_PROJECT_EXCEPTION_CATEGORY, GDFConstants.K_PROJECT_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }

        #endregion
    }
}