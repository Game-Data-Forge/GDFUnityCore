#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IFieldsOAuthExtensions.cs create at 2025/09/01 14:09:01
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    public static class FieldsOAuthExtensions
    {
        public static void CheckFieldsOAuthValidity(this IFieldsOAuth item)
        {
            if (item.AccessToken.Length < GDFConstants.K_O_AUTH_ACCESS_TOKEN_LENGTH_MIN)
            {
                throw FieldsOAuthException.AccessTokenTooShort;
            }
            if (string.IsNullOrEmpty(item.ClientId))
            {
                // TODO check?
            }
            if (item.OAuth == GDFOAuthKind.None)
            {
                // TODO check?
            }
        }
    }
}