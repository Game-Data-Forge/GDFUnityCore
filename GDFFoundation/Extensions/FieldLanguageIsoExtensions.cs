#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldLanguageIsoExtensions.cs create at 2025/09/01 15:09:06
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    public static class FieldLanguageIsoExtensions
    {
        public static void CheckLanguageIsoValidity(this IFieldLanguageIso item)
        {
            if (string.IsNullOrEmpty(item.LanguageIso))
            {
                item.LanguageIso = "en-US";
            }
        }
    }
}