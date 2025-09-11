#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj EmailPasswordSignUpExchange.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents the data exchange model for email and password-based sign-up.
    ///     This class implements several interfaces to gather all necessary information
    ///     regarding email, password, channel, user consent, country, and game details.
    ///     It is typically used to facilitate the process of signing up a user with their
    ///     email and password along with additional metadata requirements.
    /// </summary>
    /// <remarks>
    ///     This class is decorated with the [Serializable] attribute, making it suitable
    ///     for scenarios where object serialization is required.
    /// </remarks>
    /// <implements>
    ///     Implements:
    ///     - IEmailSign: Provides property for email.
    ///     - IPasswordSign: Provides property for password.
    ///     - ISignConsent: Combines consent, version, game, and channel information.
    /// </implements>
    /// <seealso cref="IFieldEmail" />
    /// <seealso cref="IFieldPassword" />
    /// <seealso cref="IFieldsConsent" />
    /// <seealso cref="IFieldCountry" />
    /// <seealso cref="IFieldChannel" />
    [Serializable]
    public class EmailPasswordSignUpExchange : IFieldEmail, IFieldsConsent
    {
        #region Instance fields and properties


        #region From interface IEmailSign

        /// <summary>
        ///     Represents the email address associated with the account.
        /// </summary>
        /// <remarks>
        ///     This property is required for email-based sign-up, sign-in, or account recovery operations.
        /// </remarks>
        public string Email { get; set; } = string.Empty;

        #endregion

        #region From interface IPasswordSign

        // /// <summary>
        // ///     Represents the password used for authentication purposes in various sign-up,
        // ///     sign-in, or account management operations. This property is a required field
        // ///     for ensuring secure access to relevant features or services. It is typically
        // ///     validated to meet defined security standards.
        // /// </summary>
        // public string Password { get; set; } = string.Empty;

        #endregion

        #region From interface ISignConsent

        public string LanguageIso { set; get; } = "en-US";

        /// <summary>
        ///     The Channel property represents the short identifier associated with the authentication or sign-up process.
        ///     It is used to specify the source or origin of the request (e.g., platform or client type).
        /// </summary>
        /// <remarks>
        ///     This property is primarily utilized in the authentication flow to associate specific actions
        ///     or tracking mechanisms with different channels. It implements the <see cref="IFieldChannel" /> interface.
        ///     Default value for this property is 0.
        /// </remarks>
        public short Channel { get; set; } = 0;

        /// <summary>
        ///     Represents the user's consent state during the sign-up process. The property is typically
        ///     utilized to determine whether the user has agreed to necessary terms and conditions or policies
        ///     required for creating an account or accessing a service.
        ///     A value of <c>true</c> indicates that the user has provided the required consent, while
        ///     <c>false</c> indicates that consent has not been given. This property is critical for ensuring
        ///     compliance with legal and organizational standards regarding user permissions.
        /// </summary>
        public bool Consent { get; set; } = false;

        /// <summary>
        ///     Gets or sets the version information used to identify the specific version of the system or application.
        /// </summary>
        /// <remarks>
        ///     This property is used across various implementations to ensure compatibility and validate versions during operations.
        ///     It is a required field and may throw exceptions if null or empty during certain checks.
        /// </remarks>
        public string ConsentVersion { get; set; } = string.Empty;

        /// <summary>
        ///     Represents the ISO 3166-1 alpha-2 country code.
        ///     This property is used to specify the user's country in a standardized format.
        /// </summary>
        public Country Country { get; set; }

        /// <summary>
        ///     Represents the GameBit property, which signifies a specific feature or capability
        ///     associated with a game or application.
        /// </summary>
        /// <remarks>
        ///     The GameBit property is of type uint and is commonly used to identify or track
        ///     game-specific details depending on the implementation or context.
        /// </remarks>
        public ConsentOptions AdditionalOptions { get; set; }

        /// <summary>
        ///     Gets or sets the version of the game associated with the consent or sign-up process.
        /// </summary>
        /// <remarks>
        ///     The GameVersion property is used to identify the version of the game for various purposes,
        ///     including validating consent, tracking versions, or managing compatibility.
        ///     Ensure this value is appropriately set in processes that interact with game-related functionalities.
        /// </remarks>
        public string ConsentName { get; set; } = string.Empty;

        #endregion

        #endregion
    }
}