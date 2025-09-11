#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldProjectExtensions.cs create at 2025/09/01 17:09:06
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    public static class FieldProjectExtensions
    {
        public static void CheckProjectValidity(this IFieldProject item, long project)
        {
            if (item.Project != project)
            {
                throw FieldProjectException.Default;
            }
        }
    }
}