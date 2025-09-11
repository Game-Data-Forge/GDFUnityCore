#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj ISignConsent.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

#endregion

namespace GDFFoundation
{
    /// <summary>
    /// Defines the contract for consent-related operations, including language, consent status,
    /// versioning, and consent options for implementations.
    /// </summary>
    /// <remarks>
    /// This interface extends <see cref="IFieldCountry"/> and <see cref="IFieldChannel"/> to provide
    /// additional contract requirements for consent management.
    /// </remarks>
    public interface IFieldsConsent : IFieldCountry, IFieldChannel, IFieldLanguageIso
    {
        
        public const int K_LANGUAGE_ISO_LENGTH = 8;
        public const int K_CONSENT_VERSION_LENGTH = 32;
        public const int K_CONSENT_KEY_LENGTH = 128;
        
        #region Instance fields and properties

        /// <summary>
        /// Gets or sets the ISO language code associated with the consent.
        /// </summary>
        /// <remarks>
        /// The property typically stores a language code following the ISO 639 standard.
        /// This value may be used for localization or internationalization purposes.
        /// </remarks>
        public string LanguageIso { set; get; }

        /// <summary>
        /// Represents the consent provided by a user for a specific operation or action.
        /// </summary>
        /// <remarks>
        /// This property is typically used to indicate whether a user has explicitly agreed
        /// to a process, service, or action. The value of this property can be used
        /// to ensure compliance with regulatory or legal standards.
        /// </remarks>
        /// <value>
        /// A boolean value that indicates whether consent has been granted.
        /// </value>
        public bool Consent { set; get; }

        /// <summary>
        /// Gets or sets the version of consent associated with the user or entity.
        /// This property is typically used to track which version of consent terms
        /// has been agreed upon.
        /// </summary>
        public string ConsentVersion { set; get; }

        /// <summary>
        /// Represents options related to game consent management.
        /// </summary>
        public ConsentOptions AdditionalOptions { set; get; }

        /// <summary>
        /// Gets or sets the current version of the game consent.
        /// </summary>
        /// <remarks>
        /// This property tracks the version number associated with user consent for the game,
        /// typically used to ensure that users are compliant with the latest terms and conditions.
        /// </remarks>
        public string ConsentName { set; get; }

        #endregion
    }
}