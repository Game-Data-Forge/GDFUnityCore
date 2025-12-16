#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IFieldLanguageIso.cs create at 2025/09/01 15:09:55
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    public interface IFieldLanguageIso
    {
        /// <summary>
        /// Gets or sets the ISO language code associated with the consent.
        /// </summary>
        /// <remarks>
        /// The property typically stores a language code following the ISO 639 standard.
        /// This value may be used for localization or internationalization purposes.
        /// </remarks>
        public string LanguageIso { set; get; }
    }
}