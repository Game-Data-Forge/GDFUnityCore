#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFOAuthKind.cs create at 2025/09/01 15:09:14
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    [Serializable]
    public enum GDFOAuthKind : int
    {
        None = GDFAccountSignType.None,
        Facebook = GDFAccountSignType.Facebook,
        Google = GDFAccountSignType.Google,
        Apple = GDFAccountSignType.Apple,
        Microsoft = GDFAccountSignType.Microsoft,
        Twitter = GDFAccountSignType.Twitter,
        LinkedIn = GDFAccountSignType.LinkedIn,
        Discord = GDFAccountSignType.Discord,
    }
}