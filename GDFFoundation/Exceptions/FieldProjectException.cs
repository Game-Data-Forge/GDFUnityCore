#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldProjectException.cs create at 2025/09/01 17:09:23
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System.Net;

namespace GDFFoundation
{
    public class FieldProjectException : APIException
    {
        public static FieldProjectException Default => new FieldProjectException(0, $"The field '${nameof(IFieldProject.Project)}' is invalid!");

        public FieldProjectException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_PROJECT_EXCEPTION_CATEGORY, GDFConstants.K_PROJECT_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }
    }
}