#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj SignListExchange.cs create at 2025/09/01 13:09:10
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;
using System.Collections.Generic;

namespace GDFFoundation
{
    [Serializable]
    public class SignListExchange : IApiResult
    {
        #region Instance fields and properties

        public List<GDFAccountSign> Signs { get; set; }

        #region From interface IApiResult

        [GDFDbLength(256)]
        public string Status { get; set; }
        public bool Success { get; set; }

        #endregion

        #endregion
    }
}