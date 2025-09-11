#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj PrintAsciiKind.cs create at 2025/08/29 17:08:01
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    [Flags]
    public enum PrintAsciiKind
    {
        None = 0,
        Information = 1 << 0,
        Version = 1 << 1,
        Logo = 1 << 2,
    }
}