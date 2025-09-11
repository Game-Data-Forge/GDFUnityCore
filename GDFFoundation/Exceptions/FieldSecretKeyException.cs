#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldSecretKeyException.cs create at 2025/09/02 14:09:50
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System.Net;

namespace GDFFoundation
{
    public class FieldSecretKeyException : APIException
    {
        public static FieldSecretKeyException NotValid => new FieldSecretKeyException(0, $"The field '{nameof(IFieldSecretKey.SecretKey)}' is invalid!");

        
        //public static SecretKeyException DefaultException => new SecretKeyException(10, $"Default {nameof(ServiceException)}");
        //public static SecretKeyException SecretUnknown => new SecretKeyException(20, $"{nameof(GDFAccountService)} unknown!");
        
        public FieldSecretKeyException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_SECRET_KEY_EXCEPTION_CATEGORY, GDFConstants.K_SECRET_KEY_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }
    }
}