#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj OAuthSignInExchange.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public class OAuthSignInExchange : IFieldsOAuth, IFieldChannel
    {
        #region Instance fields and properties

        public Country Country { get; set; }

        #region From interface IOAuthSign

        public string AccessToken { get; set; } = string.Empty;
        public string ClientId { get; set; }
        public GDFOAuthKind OAuth { get; set; }

        #endregion

        #region From interface ISignChannel

        public short Channel { get; set; }

        #endregion

        #endregion
    }
}