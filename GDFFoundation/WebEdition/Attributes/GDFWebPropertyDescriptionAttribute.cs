#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFWebPropertyDescriptionAttribute.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;
using System.Linq;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents a custom attribute used to provide web property description for a property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public class GDFWebPropertyDescriptionAttribute : Attribute
    {
        #region Instance fields and properties
        /// <summary>
        ///     Represents a custom attribute used to describe web properties for the GDFWebEdition.
        /// </summary>
        public GDFWebEditionPropertyDescription Infos = new GDFWebEditionPropertyDescription();

        #endregion
        #region Instance constructors and destructors

        /// <summary>
        ///     Attribute used to describe the web properties of a class.
        /// </summary>
        public GDFWebPropertyDescriptionAttribute()
        {
        }
        public GDFWebPropertyDescriptionAttribute(
            GDFWebEditionStyle style,
            string bootstrapIcon = ""
        )
        {
            Infos.Style = style;
            Infos.Icon = bootstrapIcon;
        }

        #endregion
    }
}