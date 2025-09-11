#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFEnvironmentStatus.cs create at 2025/03/26 17:03:59
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public enum GDFEnvironmentStatus
    {
        Active = 0,
        Inactive = 1,
        InTransfert = 2,
    }
}