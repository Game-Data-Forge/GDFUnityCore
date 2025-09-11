#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldOAuthException.cs create at 2025/09/01 17:09:07
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System.Net;

namespace GDFFoundation
{
    public class FieldsOAuthException : APIException
    {
        public static FieldsOAuthException AccessTokenDefault => new FieldsOAuthException(0, $"The field '${nameof(IFieldsOAuth.AccessToken)}' is invalid!");
        public static FieldsOAuthException AccessTokenTooShort => new FieldsOAuthException(10, $"The field '${nameof(IFieldsOAuth.AccessToken)}' is too short!");

        public FieldsOAuthException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_OAUTH_EXCEPTION_CATEGORY, GDFConstants.K_OAUTH_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }
    }
}