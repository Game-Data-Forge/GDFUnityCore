#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj OAuthSignRevokeExchange.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public class OAuthSignRevokeExchange : IFieldReference
    {
        #region Instance fields and properties

        #region From interface ISignReference

        public long Reference { get; set; }

        #endregion

        #endregion
    }
}