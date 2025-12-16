#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj SignKindExtension.cs create at 2025/03/26 17:03:59
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    static class SignKindExtension
    {
        #region Static methods

        public static string Obfuscation(this GDFAccountSignType kind)
        {
            switch (kind)
            {
                case GDFAccountSignType.EmailPassword:
                    return "M";
                case GDFAccountSignType.DeviceId:
                    return "P";
                case GDFAccountSignType.Facebook:
                    return "E";
                case GDFAccountSignType.Google:
                    return "R";
                case GDFAccountSignType.Apple:
                    return "D";
                case GDFAccountSignType.LinkedIn:
                    return "U";
                case GDFAccountSignType.Discord:
                    return "A";
                case GDFAccountSignType.None:
                    return "?";
                default:
                    return "W";
            }
        }

        #endregion
    }
}