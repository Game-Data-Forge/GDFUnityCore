#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldPasswordExtensions.cs create at 2025/09/01 14:09:05
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;
using System.Text.RegularExpressions;

namespace GDFFoundation
{
    public static class FieldPasswordExtensions
    {
        public static void CheckPasswordValidity(this IFieldPassword item)
        {
            CheckPasswordValidity(item.Password);
        }

        public static void CheckPasswordValidity(string password)
        {
            if (password.Length < GDFConstants.K_PASSWORD_LENGTH_MIN)
            {
                throw new FormatException($"The field `{nameof(IFieldPassword.Password)}` is too short.");
            }

            if (password.Length > GDFConstants.K_PASSWORD_LENGTH_MAX)
            {
                throw new FormatException($"The field `{nameof(IFieldPassword.Password)}` is too long.");
            }

            if (!Regex.IsMatch(password, GDFConstants.K_PASSWORD_EREG_PATTERN))
            {
                throw new FormatException($"The field `{nameof(IFieldPassword.Password)}` must contains {GDFConstants.K_PASSWORD_REQUIRE}");
            }
        }
    }
}