#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldSecretKeyExtensions.cs create at 2025/09/02 14:09:26
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    static public class FieldSecretKeyExtensions
    {
        public static void CheckSecretKeyValidity(this IFieldSecretKey item, string key)
        {
            if (item.SecretKey == null || item.SecretKey != key)
            {
                throw FieldSecretKeyException.NotValid;
            }
        }
    }
}