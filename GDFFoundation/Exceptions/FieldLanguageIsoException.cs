#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldLanguageIsoException.cs create at 2025/09/01 15:09:45
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System.Net;

namespace GDFFoundation
{
    public class FieldLanguageIsoException : APIException
    {
        public static FieldLanguageIsoException Default => new FieldLanguageIsoException(0, $"The field '${nameof(IFieldLanguageIso.LanguageIso)}' is invalid!");
        public static FieldLanguageIsoException TooShort => new FieldLanguageIsoException(10, $"The field '${nameof(IFieldLanguageIso.LanguageIso)}' is invalid!");

        public FieldLanguageIsoException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_LANGUAGE_ISO_EXCEPTION_CATEGORY, GDFConstants.K_LANGUAGE_ISO_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }
    }
}