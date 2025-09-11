#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFAccountDataManagement.csproj ServiceException.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Net;

#endregion

namespace GDFFoundation
{
    //[Obsolete($"use {nameof(FieldServiceException)}")]
    public class ServiceException : APIException
    {
        #region Static fields and properties

        /// <summary>
        ///     A static property that returns a default instance of the <see cref="ServiceException" /> class.
        /// </summary>
        /// <remarks>
        ///     The <c>DefaultException</c> property creates a <see cref="ServiceException" /> object with default
        ///     configuration, having an error number of 0 and a message indicating it is the default instance.
        /// </remarks>
        /// <returns>
        ///     A pre-defined <see cref="ServiceException" /> instance representing a default exception state.
        /// </returns>
        public static ServiceException DefaultException => new ServiceException(10, $"Default {nameof(ServiceException)}");
        public static ServiceException AccountServiceUnknown => new ServiceException(20, $"{nameof(GDFAccountService)} unknown!");
        public static ServiceException AccountServiceInsertedIsNull => new ServiceException( 30, $"Failed to insert a {nameof(GDFAccountService)}!");
        public static ServiceException TooMany => new ServiceException(99, $"Too many service. The maximum is ${GDFConstants.K_SERVICE_MAX} services. Recycle old service.");
        
        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents a custom exception specifically for service-related errors within the application.
        ///     Inherits from the <see cref="APIException" /> class and provides additional functionality
        ///     to standardize service error handling, including categorization and error indexing.
        /// </summary>
        public ServiceException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_SERVICE_EXCEPTION_CATEGORY, GDFConstants.K_SERVICE_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }

        #endregion
    }
}