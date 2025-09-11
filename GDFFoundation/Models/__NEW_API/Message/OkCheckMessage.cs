#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFServerApi.csproj OkCheckMessage.cs create at 2025/09/07 18:09:48
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    [Serializable]
    public class OkCheckMessage //TODO add IApiResult
    {
        #region Instance fields and properties
        public long project { get; set; }
        public string gitCommit { get; set; }
        public string version { get; set; }

        #endregion
    }
}