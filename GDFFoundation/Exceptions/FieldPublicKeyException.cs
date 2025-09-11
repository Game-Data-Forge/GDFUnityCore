#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldPublicKeyException.cs create at 2025/09/02 14:09:43
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System.Net;

namespace GDFFoundation
{
    public class FieldPublicKeyException : APIException
    {
        public static FieldPublicKeyException NotValid => new FieldPublicKeyException(0, $"The field '${nameof(IFieldPublicKey.PublicKey)}' is invalid!");

        public FieldPublicKeyException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_PUBLIC_KEY_EXCEPTION_CATEGORY, GDFConstants.K_PUBLIC_KEY_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }
    }
}