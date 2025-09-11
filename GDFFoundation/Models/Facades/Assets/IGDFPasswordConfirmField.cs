#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IGDFPasswordConfirmField.cs create at 2025/09/01 13:09:37
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    public interface IGDFPasswordConfirmField :IGDFPasswordField
    {
        public string PasswordConfirm { get; set; }
    }
}