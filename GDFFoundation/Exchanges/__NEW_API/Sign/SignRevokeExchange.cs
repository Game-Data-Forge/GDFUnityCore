#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj SignRevokeExchange.cs create at 2025/05/08 12:05:43
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public class SignRevokeExchange : IFieldReference
    {
        #region Instance fields and properties

        #region From interface ISignReference

        public long Reference { set; get; }

        #endregion

        #endregion
    }
}