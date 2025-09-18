#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFAccountConsent.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    /// Represents an account consent that inherits from <see cref="GDFAccountData"/> and provides implementations for
    /// <see cref="IFieldsConsent"/> and <see cref="IFieldCountry"/> interfaces.
    /// </summary>
    /// <remarks>
    /// This class contains fields and properties for managing account consent and related meta-data
    /// such as address, language, country, consent options, and respective versions.
    /// </remarks>
    [Serializable]
    public class GDFAccountConsent : GDFAccountData, IFieldsConsent, IFieldCountry
    {
        #region Instance fields and properties

        /// <summary>
        /// Represents the address associated with a <see cref="GDFAccountConsent"/> instance.
        /// </summary>
        /// <remarks>
        /// This property stores the address in string format and is initialized to an empty string by default.
        /// It has a maximum length constraint defined by the <see cref="GDFDbLengthAttribute"/> and a default value set using the <see cref="GDFDbDefaultAttribute"/>.
        /// </remarks>
        /// <value>
        /// The address value as a <see cref="string"/>. The default value is an empty string.
        /// </value>
        [GDFDbLength(64)]
        [GDFDbDefault("?.?.?.?")]
        public string Address { get; set; } = string.Empty;

        #region From interface ISignConsent

        /// <summary>
        /// Gets or sets the ISO language code associated with the consent.
        /// </summary>
        /// <remarks>
        /// The <see cref="LanguageIso"/> property represents the ISO 639-1 language code, which is
        /// used to specify the language in the context of the consent process. The default value
        /// is set to "en-US".
        /// </remarks>
        /// <example>
        /// This property is typically utilized when associating a language preference with
        /// consent-related operations in the <see cref="GDFAccountConsent"/> class.
        /// </example>
        /// <seealso cref="GDFAccountConsent"/>
        /// <seealso cref="GDFDbLengthAttribute"/>
        /// <seealso cref="GDFDbDefaultAttribute"/>
        [GDFDbLength(IFieldsConsent.K_LANGUAGE_ISO_LENGTH)]
        [GDFDbDefault("en-US")]
        public string LanguageIso { set; get; } = "en-US";

        /// <summary>
        /// Gets or sets the channel associated with the consent.
        /// This property represents the context or medium from which the consent is provided,
        /// such as a specific platform, device, or integration.
        /// </summary>
        /// <remarks>
        /// The property utilizes the <see cref="GDFDbDefaultAttribute"/> attribute to set a default value of 0.
        /// It is frequently used within instances of <see cref="GDFAccountConsent"/> and is integral to the consent's metadata.
        /// </remarks>
        /// <seealso cref="IFieldChannel"/>
        [GDFDbDefault(0)]
        public short Channel { get; set; } = 0;

        /// <summary>
        /// Gets or sets a value indicating the user's consent status.
        /// </summary>
        /// <remarks>
        /// The <see cref="GDFAccountConsent"/> class implements this property to track whether a user has provided consent.
        /// By default, this property is set to <c>false</c>.
        /// </remarks>
        /// <value>
        /// A <see cref="bool"/> value where <c>true</c> indicates that the user has provided consent,
        /// and <c>false</c> indicates the user has not provided consent.
        /// </value>
        [GDFDbDefault(false)]
        public bool Consent { set; get; } = false;

        /// <summary>
        /// Gets or sets the version of consent for the account. This property
        /// is used to track the version of consent agreements the account adheres to.
        /// The default value is "0.0.0".
        /// </summary>
        /// <remarks>
        /// The property is decorated with the <see cref="GDFFoundation.GDFDbLengthAttribute"/>
        /// to enforce a maximum length of 16 characters in the database and with the
        /// <see cref="GDFFoundation.GDFDbDefaultAttribute"/> to specify a default value of "0.0.0".
        /// </remarks>
        [GDFDbLength(IFieldsConsent.K_CONSENT_VERSION_LENGTH)]
        [GDFDbDefault("0.0.0")]
        public string ConsentVersion { set; get; } = "0.0.0";

        /// <summary>
        /// Gets or sets the country associated with the <see cref="GDFAccountConsent"/>.
        /// </summary>
        /// <remarks>
        /// This property is decorated with the <see cref="GDFDbDefaultAttribute"/> to specify a default value of
        /// <see cref="Country"/> for instances of the <see cref="GDFAccountConsent"/> class.
        /// </remarks>
        /// <value>
        /// The value of the <see cref="Country"/> for the <see cref="GDFAccountConsent"/>.
        /// </value>
        [GDFDbDefault(Country.UnitedKingdom)]
        public Country Country { get; set; }

        /// <summary>
        /// Represents the property for configuring game consent options within the application.
        /// Used in conjunction with <see cref="GDFAccountConsent"/> and is initialized
        /// with the default value defined by <see cref="ConsentOptions.None"/>.
        /// </summary>
        /// <remarks>
        /// The property is associated with the <see cref="GDFDbDefaultAttribute"/>,
        /// which specifies the default value for this property.
        /// </remarks>
        /// <seealso cref="GDFAccountConsent"/>
        /// <seealso cref="ConsentOptions"/>
        [GDFDbDefault(ConsentOptions.None)]
        public ConsentOptions AdditionalOptions { set; get; } = ConsentOptions.None;

        /// <summary>
        /// Represents the version of game-related consent provided by a user.
        /// </summary>
        /// <remarks>
        /// This property is decorated with the <see cref="GDFDbLengthAttribute"/> and <see cref="GDFDbDefaultAttribute"/> attributes:
        /// - <see cref="GDFDbLengthAttribute"/> restricts the maximum length of the value.
        /// - <see cref="GDFDbDefaultAttribute"/> defines the default value as "0.0.0".
        /// </remarks>
        /// <value>
        /// A <see cref="string"/> representing the version of the game consent.
        /// </value>
        /// <seealso cref="GDFAccountConsent"/>
        /// <seealso cref="GDFDbLengthAttribute"/>
        /// <seealso cref="GDFDbDefaultAttribute"/>
        /// <seealso cref="IFieldsConsent"/>
        /// <seealso cref="ConsentManager"/>
        [GDFDbLength(IFieldsConsent.K_CONSENT_KEY_LENGTH)]
        [GDFDbDefault("0.0.0")]
        public string ConsentName { set; get; } = string.Empty;

        #endregion

        public GDFAccountConsent()
        {

        }

        public GDFAccountConsent(long account,
            int range,
            long project,
            IFieldsConsent signConsent,
            string address,
            DateTime now)
        {
            LanguageIso = signConsent.LanguageIso;
            Consent = signConsent.Consent;
            ConsentVersion = signConsent.ConsentVersion;
            ConsentName = signConsent.ConsentName;
            AdditionalOptions = signConsent.AdditionalOptions;
            Country = signConsent.Country;
            Channel = signConsent.Channel;
            Creation = now;
            Modification = now;
            Account = account;
            Range = range;
            Project = project;
            Address = address;
        }

        public void ModifiedToNewValues(GDFAccountConsent item)
        {
            LanguageIso = item.LanguageIso;
            Consent = item.Consent;
            ConsentVersion = item.ConsentVersion;
            AdditionalOptions = item.AdditionalOptions;
            Country = item.Country;
            Channel = item.Channel;
            Modification = GDFDatetime.Now;
            Address = item.Address;
        }
        
        #endregion
    }
}