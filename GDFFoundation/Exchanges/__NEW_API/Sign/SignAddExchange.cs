#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj SignAddExchange.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public class SignAddExchange : IFieldEmail, IFieldPassword
    {
        #region Instance fields and properties

        public string AccessToken { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public GDFAccountSignType Kind { set; get; } = GDFAccountSignType.EmailPassword;

        #region From interface IEmailSign

        public string Email { get; set; } = string.Empty;

        #endregion

        #region From interface IPasswordSign

        public string Password { get; set; } = string.Empty;

        #endregion

        #endregion
    }
}