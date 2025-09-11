#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj FieldDeviceException.cs create at 2025/09/01 15:09:26
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System.Net;

namespace GDFFoundation
{
    public class FieldDeviceException : APIException
    {
        public static FieldDeviceException Default => new FieldDeviceException(0, $"The field '${nameof(IFieldDevice.UniqueIdentifier)}' is invalid!");
        public static FieldDeviceException TooShort => new FieldDeviceException(10, $"The field '${nameof(IFieldDevice.UniqueIdentifier)}' is invalid!");

        public FieldDeviceException(ushort errorNumber, string message, string help = "")
            : base(HttpStatusCode.InternalServerError, GDFConstants.K_DEVICE_EXCEPTION_CATEGORY, GDFConstants.K_DEVICE_EXCEPTION_INDEX + errorNumber, message, help)
        {
        }
    }
}