#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFBootstrapFullKindOfStyle.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    /// Represents the secondary outline style in the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration.
    /// </summary>
    [Serializable]
    public enum GDFBootstrapFullKindOfStyle : int
    {
        /// <summary>
        /// Represents the primary style in the context of <see cref="GDFBootstrapFullKindOfStyle"/>.
        /// </summary>
        /// <remarks>
        /// This enum member is designated to signify the use of the primary styling option
        /// defined by the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration within its intended scope.
        /// </remarks>
        Primary,

        /// <summary>
        /// Specifies the "outline-primary" style variant within the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration.
        /// Typically used to apply a primary outline styling to HTML elements.
        /// </summary>
        OutlinePrimary,

        /// <summary>
        /// Represents the secondary style for the Bootstrap framework.
        /// </summary>
        /// <remarks>
        /// This member is used in the <see cref="GDFBootstrapKindOfStyle"/> enumeration in the <see cref="GDFFoundation"/> namespace.
        /// It represents the secondary style for the Bootstrap framework.
        /// </remarks>
        Secondary,

        /// <summary>
        /// Represents the outline secondary style in the context of <see cref="GDFBootstrapFullKindOfStyle"/>.
        /// </summary>
        /// <remarks>
        /// This enum member is designated to signify the use of an outlined variation of the secondary styling option
        /// defined by the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration within its intended scope.
        /// </remarks>
        OutlineSecondary,

        /// <summary>
        /// Represents a successful style for the Bootstrap framework.
        /// </summary>
        /// <remarks>
        /// This member is part of the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration in the GDFFoundation namespace.
        /// It signifies a successful style commonly applied to visually indicate success or positive outcomes.
        /// </remarks>
        Success,

        /// <summary>
        /// Indicates a success style with an outlined appearance in the context of
        /// <see cref="GDFBootstrapFullKindOfStyle"/>.
        /// </summary>
        OutlineSuccess,

        /// <summary>
        /// Represents the warning style for the Bootstrap framework.
        /// </summary>
        /// <remarks>
        /// This member is part of the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration
        /// in the GDFFoundation namespace. It signifies the warning style within the Bootstrap framework.
        /// </remarks>
        Warning,

        /// <summary>
        /// Specifies the outline warning style within the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration.
        /// This style is typically used to indicate a warning state with an outlined appearance.
        /// </summary>
        OutlineWarning,

        /// <summary>
        /// Represents the danger style for the Bootstrap framework.
        /// </summary>
        /// <remarks>
        /// This member is part of the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration
        /// in the <see cref="GDFFoundation"/> namespace. It is used to indicate a
        /// danger or error-related style in the Bootstrap framework.
        /// </remarks>
        Danger,

        /// <summary>
        /// Specifies the "outline-danger" style in the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration,
        /// typically used to apply an outlined danger theme to HTML elements or components.
        /// </summary>
        OutlineDanger,

        /// <summary>
        /// Indicates that the style represents informational elements in the context of
        /// <see cref="GDFBootstrapFullKindOfStyle"/>.
        /// </summary>
        Info,

        /// <summary>
        /// Represents the outline information style option within the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration.
        /// </summary>
        OutlineInfo,

        /// <summary>
        /// Represents the <see cref="GDFBootstrapFullKindOfStyle"/> where the style is set to a default or normal appearance.
        /// </summary>
        Normal,

        /// <summary>
        /// Specifies a normal outline style in the <see cref="GDFBootstrapFullKindOfStyle"/> enumeration.
        /// </summary>
        OutlineNormal,

        /// <summary>
        /// Represents the <see cref="GDFBootstrapFullKindOfStyle"/> option for a light styling of an HTML element.
        /// </summary>
        Light,

        /// <summary>
        /// Specifies the light outline variant for a bootstrap style in <see cref="GDFBootstrapFullKindOfStyle"/>.
        /// </summary>
        OutlineLight,

        /// <summary>
        /// Specifies the dark bootstrap style variant for HTML elements,
        /// intended for use in components styled with <see cref="GDFBootstrapFullKindOfStyle"/>.
        /// </summary>
        Dark,

        /// <summary>
        /// Specifies a dark outline style variant in the <see cref="GDFBootstrapFullKindOfStyle"/> enum.
        /// </summary>
        OutlineDark,
    }
}