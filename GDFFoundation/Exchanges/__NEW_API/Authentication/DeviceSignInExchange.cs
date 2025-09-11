#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj DeviceSignInExchange.cs create at 2025/04/29 09:04:30
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public class DeviceSignInExchange : IFieldDevice, IFieldChannel
    {
        #region Instance fields and properties

        public Country Country { get; set; }

        #region From interface IDeviceSign

        // public GDFExchangeDevice DeviceKind { set; get; } = GDFExchangeDevice.Unknown;
        public string UniqueIdentifier { set; get; } = string.Empty;

        #endregion

        #region From interface ISignChannel

        public short Channel { get; set; }

        #endregion

        #endregion
    }
}