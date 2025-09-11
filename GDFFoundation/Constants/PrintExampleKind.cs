#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj PrintExampleKind.cs create at 2025/08/29 17:08:53
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    [Flags]
    public enum PrintExampleKind
    {
        None = 0,
        AppSetting = 1 << 0,
        Configuration = 1 << 1,
    }
}