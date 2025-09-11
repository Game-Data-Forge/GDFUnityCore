#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFAccountDataManagement.csproj SecretKeyException.cs create at 2025/08/26 19:08:22
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;
using System.Net;

namespace GDFFoundation
{
    /// <summary>
    ///     Represents a service-level exception that inherits from the APIException class.
    /// </summary>
    /// <remarks>
    ///     This exception is used to encapsulate errors that occur during service operations. It extends the functionality
    ///     of APIException by providing a specific error category and an error index for service-related issues.
    /// </remarks>
    //[Obsolete($"use {nameof(FieldSecretKeyException)}")]
    public class SecretKeyException : APIException
    {
        #region Static fields and properties

        public static SecretKeyException DefaultException => new SecretKeyException(10, $"Default {nameof(SecretKeyException)}");
        public static SecretKeyException SecretUnknown => new SecretKeyException(20, $"{nameof(GDFAccountService)} unknown!");
        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents a custom exception specifically for service-related errors within the application.
        ///     Inherits from the <see cref="APIException" /> class and provides additional functionality
        ///     to standardize service error handling, including categorization and error indexing.
        /// </summary>
        public SecretKeyException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_SECRET_KEY_EXCEPTION_CATEGORY, GDFConstants.K_SECRET_KEY_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }

        #endregion
    }
}