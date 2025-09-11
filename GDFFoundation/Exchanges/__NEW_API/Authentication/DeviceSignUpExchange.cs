#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj DeviceSignUpExchange.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public class DeviceSignUpExchange : IFieldDevice, IFieldsConsent
    {
        #region Instance fields and properties

        #region From interface IDeviceSign

        // public GDFExchangeDevice DeviceKind { set; get; } = GDFExchangeDevice.Unknown;
        public string UniqueIdentifier { set; get; } = string.Empty;

        #endregion

        #region From interface ISignConsent

        public string LanguageIso { set; get; } = "en-US";

        public short Channel { get; set; } = 0;
        public bool Consent { get; set; } = false;
        public string ConsentVersion { get; set; } = "1.0.0";
        public Country Country { get; set; }
        public ConsentOptions AdditionalOptions { get; set; } = 0;
        public string ConsentName { get; set; } = string.Empty;

        #endregion

        #endregion
    }
}