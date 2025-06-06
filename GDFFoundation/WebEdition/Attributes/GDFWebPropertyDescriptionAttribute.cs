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
            string bootstrapIcon = "",
            bool useAsSortBy = false,
            bool useAsColumn = false,
            bool isPrimaryColumn = false
        )
        {
            Infos.Style = style;
            Infos.Icon = bootstrapIcon;
            Infos.UseAsSortBy = useAsSortBy;
            Infos.UseAsColumn = useAsColumn;
            Infos.IsPrimaryColumn = isPrimaryColumn;
        }

        /// <summary>
        ///     This attribute is used to provide web property descriptions for properties in a web edition of a class.
        /// </summary>
        public GDFWebPropertyDescriptionAttribute(
            string label,
            GDFWebEditionStyle style,
            bool required,
            string bootstrapIcon,
            string description,
            string placeholder,
            bool useAsSortBy = false,
            bool useAsColumn = false,
            bool isPrimaryColumn = false
        )
        {
            Infos.Style = style;
            Infos.Icon = bootstrapIcon;
            Infos.Description = description;
            Infos.UseAsSortBy = useAsSortBy;
            Infos.UseAsColumn = useAsColumn;
            Infos.IsPrimaryColumn = isPrimaryColumn;
        }

        /// <summary>
        ///     Attribute used for describing web properties.
        /// </summary>
        public GDFWebPropertyDescriptionAttribute(
            string label,
            GDFWebEditionStyle style,
            bool required,
            string bootstrapIcon,
            string description,
            string placeholder,
            Type dropDownValues,
            bool useAsSortBy = false,
            bool useAsColumn = false,
            bool isPrimaryColumn = false
        )
        {
            Infos.Style = style;
            Infos.Icon = bootstrapIcon;
            Infos.Description = description;
            Infos.UseAsSortBy = useAsSortBy;
            Infos.UseAsColumn = useAsColumn;
            Infos.IsPrimaryColumn = isPrimaryColumn;
            Infos.DropDownValues = Enum.GetNames(dropDownValues).ToList();
        }

        /// <summary>
        ///     Attribute for describing the web property.
        /// </summary>
        public GDFWebPropertyDescriptionAttribute(
            string label,
            GDFWebEditionStyle style,
            bool required,
            string bootstrapIcon,
            string description,
            string placeholder,
            string[] dropDownValues,
            bool useAsSortBy = false,
            bool useAsColumn = false,
            bool isPrimaryColumn = false
        )
        {
            Infos.Style = style;
            Infos.Icon = bootstrapIcon;
            Infos.Description = description;
            Infos.UseAsSortBy = useAsSortBy;
            Infos.UseAsColumn = useAsColumn;
            Infos.IsPrimaryColumn = isPrimaryColumn;
            Infos.DropDownValues = dropDownValues.ToList();
        }

        /// <summary>
        ///     Represents an attribute that provides web property description for a property.
        /// </summary>
        public GDFWebPropertyDescriptionAttribute(
            string label,
            GDFWebEditionStyle style,
            bool required,
            string bootstrapIcon,
            string description,
            string placeholder,
            Type dataRetriever,
            Type listType,
            bool useAsTitle = false,
            bool useAsSortBy = false,
            bool useAsDescription = false
        )
        {
            Infos.Style = style;
            Infos.Icon = bootstrapIcon;
            Infos.Description = description;
            Infos.UseAsSortBy = useAsSortBy;
            Infos.UseAsColumn = useAsDescription;
            Infos.IsPrimaryColumn = useAsTitle;
        }

        #endregion
    }
}