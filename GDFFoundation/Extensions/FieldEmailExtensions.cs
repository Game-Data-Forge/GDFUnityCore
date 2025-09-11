#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldEmaiExtensions.cs create at 2025/09/01 13:09:27
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;
using System.Data;
using System.Text.RegularExpressions;

namespace GDFFoundation
{
    public static class FieldEmailExtensions
    {
        public static void CheckEmailValidity(this IFieldEmail item)
        {
            CheckEmailValidity(item.Email);
        }
        public static void CheckEmailValidity(string email)
        {
            
            if (email==null)
            {
                throw new NoNullAllowedException($"The field `{nameof(IFieldEmail.Email)}` is null.");
            }

            if (email.Length < GDFConstants.K_EMAIL_LENGTH_MIN)
            {
                throw new FormatException($"The field `{nameof(IFieldEmail.Email)}` is too short.");
            }

            if (email.Length > GDFConstants.K_EMAIL_LENGTH_MAX)
            {
                throw new FormatException($"The field `{nameof(IFieldEmail.Email)}` is too long.");
            }

            if (!Regex.IsMatch(email, GDFConstants.K_EMAIL_EREG_PATTERN))
            {
                throw new FormatException($"The field `{nameof(IFieldEmail.Email)}` is not a valid email address.");
            }
        }
    }
}