// #region Copyright
//
// // Game-Data-Forge Solution
// // Written by CONTART Jean-François & BOULOGNE Quentin
// // GDFAccountDataManagement.csproj AccountException.cs create at 2025/03/25 11:03:36
// // ©2024-2025 idéMobi SARL FRANCE
//
// #endregion
//
// #region
//
// using System;
// using System.Net;
//
// #endregion
//
// namespace GDFFoundation
// {
//     /// <summary>
//     ///     Represents an exception specific to account-related operations in the system.
//     /// </summary>
//     /// <remarks>
//     ///     This exception is a specialized implementation of the <see cref="APIException" />
//     ///     to provide more context and categorization for account-related errors. It associates
//     ///     an error category, error index, and status code along with a descriptive message.
//     /// </remarks>
//     /// <seealso cref="APIException" />
//     /// <seealso cref="GDFException" />
//    
//    // [Obsolete($"use {nameof(FieldAccountException)}")]
//     public class AccountException : APIException
//     {
//         #region Static fields and properties
//
//         /// <summary>
//         ///     Represents a default instance of the <see cref="AccountException" /> class with preset properties.
//         /// </summary>
//         /// <remarks>
//         ///     The <c>DefaultException</c> property provides a predefined, default <see cref="AccountException" />
//         ///     instance with an error number of 0 and a message indicating it is the default exception for the
//         ///     <see cref="AccountException" /> class.
//         /// </remarks>
//         /// <value>
//         ///     Returns a new instance of <see cref="AccountException" /> representing the default state.
//         /// </value>
//         public static AccountException DefaultException => new AccountException(0, $"Default {nameof(AccountException)}");
//
//         #endregion
//
//         #region Instance constructors and destructors
//
//         /// <summary>
//         ///     Represents an exception specific to account-related operations.
//         ///     This exception is derived from the <see cref="APIException" /> class and provides
//         ///     additional context for errors occurring in account management functionality.
//         /// </summary>
//         public AccountException(ushort errorNumber, string message, string help = "")
//             : base(HttpStatusCode.InternalServerError, GDFConstants.K_ACCOUNT_EXCEPTION_CATEGORY, GDFConstants.K_ACCOUNT_EXCEPTION_INDEX + errorNumber, message, help)
//         {
//         }
//
//         #endregion
//     }
// }