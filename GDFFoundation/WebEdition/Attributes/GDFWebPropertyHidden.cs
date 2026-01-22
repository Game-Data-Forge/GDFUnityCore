#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFWebPropertyHidden.cs create at 2025/03/27 21:03:15
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Attribute used to hide a property from web edition in the inspector.
    /// </summary>
    public class GDFWebPropertyHidden : GDFWebPropertyDescriptionAttribute
    {
        #region Instance constructors and destructors

        public GDFWebPropertyHidden()
        {
            Infos.Style = GDFWebEditionStyle.Hidden;
            Infos.Icon = string.Empty;
        }

        #endregion
    }
}