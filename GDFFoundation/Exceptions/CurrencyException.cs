#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFAccountDataManagement.csproj CurrencyException.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Net;

#endregion

namespace GDFFoundation
{
    //[Obsolete($"use {nameof(FieldCurrencyException)}")]
    public class CurrencyException : APIException
    {
        #region Static fields and properties

        /// <summary>
        ///     A static property that returns a default instance of the <see cref="CurrencyException" /> class.
        /// </summary>
        /// <remarks>
        ///     The <c>DefaultException</c> property creates a <see cref="CurrencyException" /> object with default
        ///     configuration, having an error number of 0 and a message indicating it is the default instance.
        /// </remarks>
        /// <returns>
        ///     A pre-defined <see cref="CurrencyException" /> instance representing a default exception state.
        /// </returns>
        public static CurrencyException DefaultException => new CurrencyException(10, $"Default {nameof(CurrencyException)}");

        public static CurrencyException AccountCurrencyUnknown => new CurrencyException(20, $"{nameof(GDFAccountCurrency)} unknown!");
        public static CurrencyException AccountCurrencyInsertedIsNull => new CurrencyException(30, $"Failed to insert a {nameof(GDFAccountCurrency)}!");
        public static CurrencyException AccountCurrencyAlreadyExists => new CurrencyException(20, $"{nameof(GDFAccountCurrency)} already exists!");
        public static CurrencyException TooMany => new CurrencyException(99, $"Too many currency. The maximum is ${GDFConstants.K_CURRENCY_MAX} currency. Recycle old currency.");

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents a custom exception specifically for Currency-related errors within the application.
        ///     Inherits from the <see cref="APIException" /> class and provides additional functionality
        ///     to standardize Currency error handling, including categorization and error indexing.
        /// </summary>
        public CurrencyException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_CURRENCY_EXCEPTION_CATEGORY, GDFConstants.K_CURRENCY_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }

        #endregion
    }
}