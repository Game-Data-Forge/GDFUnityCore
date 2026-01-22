#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFWebEditionPropertyDescription.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;
using System.Collections.Generic;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents a description of a web edition property.
    /// </summary>
    [Serializable]
    public class GDFWebEditionPropertyDescription
    {
        #region Instance fields and properties
        
        /// <summary>
        ///     Represents icon associated with a web edition property description.
        /// </summary>
        public string Icon = string.Empty;
        /// <summary>
        ///     Represents the style of a web edition property.
        /// </summary>
        public GDFWebEditionStyle Style = GDFWebEditionStyle.Hidden;

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents a class that contains the description for a web edition property.
        /// </summary>
        public GDFWebEditionPropertyDescription()
        {
        }

        /// <summary>
        ///     Represents a description for a web edition property.
        /// </summary>
        public GDFWebEditionPropertyDescription(
            GDFWebEditionStyle style,
            string icon
        )
        {
            Style = style;
            Icon = icon;
        }

        #endregion
    }
}