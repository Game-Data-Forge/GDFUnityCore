#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFOrganizationtStatus.cs create at 2025/08/22 14:08:27
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    [Serializable]
    public enum GDFOrganizationtStatus
    {
        Valid = 0,
        PaymentMethodAlert = 2,
        Unpayed = 7,
        LastWarning = 8,
        Deletable = 9,
    }
}