#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj EmailPasswordSignLostExchange.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents a data transfer object used in Email-Password Sign Lost operations.
    ///     This class encapsulates properties required for authentication or account restoration purposes.
    /// </summary>
    [Serializable]
    public class EmailPasswordSignLostExchange : IFieldEmail, IFieldCountry, IProjectReference, IFieldLanguageIso
    {
        #region Instance fields and properties

        /// <summary>
        ///     Gets or sets the IETF BCP 47 language code associated with the user's locale, which represents a two-letter standard language identifier.
        /// </summary>
        public string LanguageIso { get; set; } = string.Empty;

        #region From interface IEmailSign

        /// <summary>
        ///     Gets or sets the email address associated with the sign-in process.
        ///     This property is used to specify or retrieve the email address for authentication purposes.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        #endregion

        #region From interface IProjectReference

        /// <summary>
        ///     Gets or sets the unique identifier referencing the project. This property is used for associating operations or requests with a specific project configuration.
        /// </summary>
        public long ProjectReference { set; get; } = 0;

        #endregion

        #region From interface ISignCountry

        /// <summary>
        ///     Gets or sets the ISO 3166-1 alpha-2 country code representing the user's country or region, which is a two-letter standardized identifier.
        /// </summary>
        public Country Country { get; set; }

        #endregion

        #endregion
    }
}