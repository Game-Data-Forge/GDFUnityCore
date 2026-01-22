#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFWebPropertyHiddenDebugable.cs create at 2025/03/27 21:03:15
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Specifies that the property should be hidden and made debuggable in the web edition.
    /// </summary>
    public class GDFWebPropertyHiddenDebugable : GDFWebPropertyDescriptionAttribute
    {
        #region Instance constructors and destructors

        /// <summary>
        ///     Represents an attribute that provides a description for a web property and specifies whether it should be hidden in debug mode.
        /// </summary>
        public GDFWebPropertyHiddenDebugable()
        {
            Infos.Icon = string.Empty;
        }

        #endregion
    }
}