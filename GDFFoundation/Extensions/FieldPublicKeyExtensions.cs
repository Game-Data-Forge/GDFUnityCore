#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldPublicKeyExtensions.cs create at 2025/09/02 14:09:21
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    static public class FieldPublicKeyExtensions
    {
        public static void CheckPublicKeyValidity(this IFieldPublicKey item, string key)
        {
            if (item.PublicKey == null || item.PublicKey != key)
            {
                throw FieldPublicKeyException.NotValid;
            }
        }
    }
}