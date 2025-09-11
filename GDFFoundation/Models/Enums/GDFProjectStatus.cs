#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFProjectStatus.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Define the project status in GDFServer : active, inactive, upgrading data
    /// </summary>
    [Serializable]
    public enum GDFProjectStatus
    {
        Valid = 0,
        Upgrading = 1,
        Inactive = 2,
        InTransfert = 3,
        
        OverflowOfTheFreePlan = 6,
        Unpayed = 7,
        LastWarning = 8,
        Deletable = 9,
    }
}