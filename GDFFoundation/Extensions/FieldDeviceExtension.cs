#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldDeviceExtension.cs create at 2025/09/01 15:09:31
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    public static class FieldDeviceExtension
    {
        public static void CheckDeviceValidity(string uniqueIdentifier)
        {
            if (uniqueIdentifier.Length < GDFConstants.K_DEVICE_IDENTIFIER_LENGTH_MIN)
            {
                throw FieldDeviceException.TooShort;
            }
        }

        public static void CheckDeviceValidity(this IFieldDevice item)
        {
            CheckDeviceValidity(item.UniqueIdentifier);
        }
    }
}