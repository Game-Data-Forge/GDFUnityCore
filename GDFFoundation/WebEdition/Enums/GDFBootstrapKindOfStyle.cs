#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFBootstrapKindOfStyle.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents the kind of bootstrap style for HTML elements.
    /// </summary>
    [Serializable]
    public enum GDFBootstrapKindOfStyle : int
    {
        /// <summary>
        ///     Represents the primary style for the Bootstrap framework.
        /// </summary>
        /// <remarks>
        ///     This member is used in the GDFBootstrapKindOfStyle enumeration in the GDFFoundation namespace.
        ///     It represents the primary style for the Bootstrap framework.
        /// </remarks>
        Primary,

        /// <summary>
        ///     Represents a secondary style for GDFBootstrapKindOfStyle enum.
        /// </summary>
        Secondary,

        /// <summary>
        ///     Represents a Tertiary style for GDFBootstrapKindOfStyle enum.
        /// </summary>
        Tertiary,

        /// <summary>
        ///     Represents the Success member of the GDFBootstrapKindOfStyle enum.
        /// </summary>
        Success,

        /// <summary>
        ///     The Warning member of the GDFBootstrapKindOfStyle enumeration.
        /// </summary>
        Warning,

        /// <summary>
        ///     Represents the danger style of a Bootstrap component.
        /// </summary>
        Danger,

        Info,

        Normal,

        Light,
        Dark,
    }
}