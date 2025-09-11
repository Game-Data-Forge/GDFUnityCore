#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldAccountException.cs create at 2025/09/01 17:09:40
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System.Net;

namespace GDFFoundation
{
    public class FieldAccountException : APIException
    {
        public static FieldAccountException Default => new FieldAccountException(0, $"The field '${nameof(IFieldAccount.Account)}' is invalid!");

        public FieldAccountException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_ACCOUNT_EXCEPTION_CATEGORY, GDFConstants.K_ACCOUNT_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }
    }
}