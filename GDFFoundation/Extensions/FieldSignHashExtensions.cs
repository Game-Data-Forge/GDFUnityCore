#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldSignHashExtensions.cs create at 2025/09/01 17:09:32
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    public static class FieldSignHashExtensions
    {
        public static void CheckSignHashValidity(this IFieldSignHash item)
        {
            if (string.IsNullOrEmpty(item.SignHash))
            {
                throw FieldSignHashException.SignHashEmpty;
            }

            if (item.SignHash.Length < GDFConstants.K_SIGN_HASH_MIN)
            {
                throw FieldSignHashException.SignHashTooShort;
            }

            if (item.SignHash.Length > GDFConstants.K_SIGN_HASH_MAX)
            {
                throw FieldSignHashException.SignHashTooLong;
            }
        }
    }
}