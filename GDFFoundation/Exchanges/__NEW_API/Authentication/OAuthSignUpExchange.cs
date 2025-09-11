#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj OAuthSignUpExchange.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public class OAuthSignUpExchange : IFieldsOAuth, IFieldsConsent
    {
        #region Instance fields and properties

        #region From interface IOAuthSign

        public string AccessToken { get; set; } = string.Empty;
        public string ClientId { get; set; }
        public GDFOAuthKind OAuth { get; set; }

        #endregion

        #region From interface ISignConsent
        public string LanguageIso { set; get; } = "en-US";
        public short Channel { get; set; }
        public bool Consent { set; get; }
        public string ConsentVersion { set; get; }
        public Country Country { get; set; }
        public ConsentOptions AdditionalOptions { set; get; }
        public string ConsentName { set; get; }

        #endregion

        #endregion
    }
}