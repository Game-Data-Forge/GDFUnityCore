#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFAccountDataManagement.csproj ConsentException.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System.Net;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents an exception specifically related to consent errors in the system.
    /// </summary>
    /// <remarks>
    ///     This exception class is derived from <see cref="APIException" /> and is used to handle errors
    ///     associated with consent-related operations. It provides specific error categorization and numbering
    ///     for consent issues.
    /// </remarks>
    public class ConsentException : APIException
    {
        #region Static fields and properties

        /// <summary>
        ///     Represents an exception that is thrown when there is an error creating consent records.
        ///     This property provides a preconfigured instance of the <see cref="ConsentException" /> class,
        ///     highlighting a consent creation error scenario.
        /// </summary>
        /// <remarks>
        ///     This exception is thrown when the insertion process for a consent record fails, indicating
        ///     that a creation operation could not be successfully completed.
        /// </remarks>
        public static ConsentException CreationError => new ConsentException(0, $"Consent creation error");

        /// <summary>
        ///     A static property that provides a predefined instance of the <see cref="ConsentException" /> class
        ///     with default error number and message. The default instance serves as a general-purpose
        ///     representation of a consent-related exception when no specific details are provided.
        /// </summary>
        public static ConsentException DefaultException => new ConsentException(10, $"Default {nameof(ConsentException)}");
        static public ConsentException AccountConsentError => new ConsentException(20, $"Consent error.");
        static public ConsentException AccountConsentFalse=> new ConsentException(30, $"Consent not valid.");
        static public ConsentException AccountConsentDaoError => new ConsentException(40, $"Consent Dao error.");
        static public ConsentException AccountConsentResultNull => new ConsentException(50, $"Consent result null.");
        static public ConsentException AccountConsentIsNull => new ConsentException(51, $"Consent is null.");
        static public ConsentException AccountConsentMustBeAccepted => new ConsentException(52, $"Consent must be accepted.");
        static public ConsentException ConsentVersionException => new ConsentException(60, $"Consent field {nameof(IFieldsConsent.ConsentVersion)} is not valid.");
        static public ConsentException ConsentNameNotValid => new ConsentException(70, $"Consent field {nameof(IFieldsConsent.ConsentName)} is not valid.");
        static public ConsentException ConsentNameBadFormat => new ConsentException(71, $"Consent field {nameof(IFieldsConsent.ConsentName)} must be format as '{FieldsConsentExtensions._KEY_FORMAT}'.");
        static public ConsentException GDFConsentNameCannotBeDeleted => new ConsentException(71, $"This consent cannot be deleted.");
        static public ConsentException LanguageIsoException => new ConsentException(80, $"Consent field {nameof(IFieldsConsent.LanguageIso)} is not valid.");
        static public ConsentException GDFConsentException => new ConsentException(90, $"The field {nameof(IFieldsConsent.Consent)} of consent for ${GDFConstants.K_GDF} is not valid. It's must be '${true}'.");
        static public ConsentException GDFConsentVersionException => new ConsentException(91, $"The field {nameof(IFieldsConsent.ConsentVersion)} of consent for ${GDFConstants.K_GDF} is not valid. It's must be '${GDFConstants.K_CONSENT_VERSION}'.");
        static public ConsentException GDFConsentNameException => new ConsentException(92, $"The field {nameof(IFieldsConsent.ConsentName)} of consent for ${GDFConstants.K_GDF} is not valid. It's must be '${GDFConstants.K_CONSENT_NAME}'.");
        static public ConsentException TooMany => new ConsentException(99, $"Too many consent. The maximum is ${GDFConstants.K_CONSENT_MAX} consents. Recycle old consent");
        
        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents an exception specifically related to consent operations.
        ///     This class is a specialized exception inheriting from <see cref="APIException" />.
        ///     It provides a structured way to handle consent-related errors within the application.
        /// </summary>
        public ConsentException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_CONSENT_EXCEPTION_CATEGORY, GDFConstants.K_CONSENT_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }

        #endregion
    }
}