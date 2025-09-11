#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFAccountDataManagement.csproj ConsentTool.cs create at 2025/08/29 14:08:26
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;
using System.Text.RegularExpressions;

namespace GDFFoundation
{
    public static class FieldsConsentExtensions
    {

        public const string _KEY_FORMAT = "^[a-zA-Z0-9_-]+$";
        private static readonly Regex FormatRgx = new Regex(_KEY_FORMAT, RegexOptions.Compiled);
        public static bool IsValidKeyFormat(string item)
        {
            if (string.IsNullOrEmpty(item) == false)
            {
                return FormatRgx.IsMatch(item);
            }
            else
            {
                return false;
            }
        }

        public static void CheckIsValidKeyFormat(string item)
        {
            if (!IsValidKeyFormat(item))
            {
                throw ConsentException.ConsentNameBadFormat;
            }
        }
        public static void CheckIsNotGameDataForgeKey(string item)
        {
            if (item == GDFConstants.K_CONSENT_NAME)
            {
                throw ConsentException.ConsentNameBadFormat;
            }
        }

        public static void CheckConsentIsAccepted(this IFieldsConsent consent)
        {
            if (consent == null || !consent.Consent)
            {
                throw ConsentException.AccountConsentMustBeAccepted;
            }
        }

        public static void CheckConsentValidity(this IFieldsConsent  consent)
        {
            consent.CheckChannelValidity();
            consent.CheckCountryValidity();
            
            if (consent == null)
            {
                throw ConsentException.AccountConsentIsNull;
            }
            if (string.IsNullOrEmpty(consent.ConsentVersion)|| consent.ConsentVersion.Length> IFieldsConsent.K_CONSENT_VERSION_LENGTH)
            {
                throw ConsentException.ConsentVersionException;
            }
            if (string.IsNullOrEmpty(consent.ConsentName)|| consent.ConsentName.Length> IFieldsConsent.K_CONSENT_KEY_LENGTH)
            {
                throw ConsentException.ConsentNameNotValid;
            }
            if (!IsValidKeyFormat(consent.ConsentName))
            {
                throw ConsentException.ConsentNameBadFormat;
            }
            if (string.IsNullOrEmpty(consent.LanguageIso) || consent.LanguageIso.Length> IFieldsConsent.K_LANGUAGE_ISO_LENGTH)
            {
                throw ConsentException.LanguageIsoException;
            }
        }
        public static void CheckGameDataForgeConsentValidity(this IFieldsConsent consent)
        {
            consent.CheckConsentIsAccepted();
            consent.CheckConsentValidity();
            if (consent.ConsentName != GDFConstants.K_CONSENT_NAME)
            {
                throw ConsentException.GDFConsentVersionException;
            }
            if (consent.ConsentVersion != GDFConstants.K_CONSENT_VERSION)
            {
                throw ConsentException.GDFConsentNameException;
            }
        }
    }
}