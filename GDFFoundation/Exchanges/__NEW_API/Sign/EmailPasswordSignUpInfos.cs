#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj EmailPasswordSignUpInfos.cs create at 2025/09/02 08:09:25
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    [Serializable]
    public class EmailPasswordSignUpInfos
    {
        #region Instance fields and properties
        public Country Country { get; set; }
        public string Email { get; set; }
        public string EmailFrom { get; set; }
        public string Status { get; set; }
        public string Subject { get; set; }
        public bool Success { get; set; }

        #endregion
    }
}