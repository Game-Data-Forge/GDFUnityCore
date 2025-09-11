#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldSignHashException.cs create at 2025/09/01 17:09:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System.Net;

namespace GDFFoundation
{
    public class FieldSignHashException : APIException
    {
        public static FieldSignHashException SignHashDefault => new FieldSignHashException(0, $"The field '${nameof(IFieldSignHash.SignHash)}' is invalid!");
        public static FieldSignHashException SignHashTooShort => new FieldSignHashException(10, $"The field '${nameof(IFieldSignHash.SignHash)}' is too short!");
        public static FieldSignHashException SignHashTooLong => new FieldSignHashException(20, $"The field '${nameof(IFieldSignHash.SignHash)}' is too long!");
        public static FieldSignHashException SignHashEmpty => new FieldSignHashException(30, $"The field '${nameof(IFieldSignHash.SignHash)}' is empty!");

        public FieldSignHashException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_SIGN_HASH_EXCEPTION_CATEGORY, GDFConstants.K_SIGN_HASH_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }
    }
}