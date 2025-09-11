// #region Copyright
//
// // Game-Data-Forge Solution
// // Written by CONTART Jean-François & BOULOGNE Quentin
// // GDFFoundation.csproj EmailPasswordSignUpRequest.cs create at 2025/05/16 14:05:48
// // ©2024-2025 idéMobi SARL FRANCE
//
// #endregion
//
// #region
//
// using System;
//
// #endregion
//
// namespace GDFFoundation
// {
//     [Serializable]
//     public class EmailPasswordSignUpRequest : IFieldEmail, IFieldPassword, IFieldCountry, IProjectReference, IProjectSecretHash
//     {
//         #region Instance fields and properties
//
//         public GDFLanguageEnum Language { set; get; } = GDFLanguageEnum.English;
//
//         #region From interface IEmailSign
//
//         public string Email { get; set; }
//
//         #endregion
//
//         #region From interface IPasswordSign
//
//         public string Password { get; set; }
//
//         #endregion
//
//         #region From interface IProjectReference
//
//         public long ProjectReference { get; set; }
//
//         #endregion
//
//         #region From interface IProjectSecretHash
//
//         public string ProjectSecretHash { get; set; }
//
//         #endregion
//
//         #region From interface ISignCountry
//
//         public Country Country { get; set; }
//
//         #endregion
//
//         #endregion
//     }
// }