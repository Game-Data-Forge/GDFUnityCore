#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFAccountCurrency.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;

#endregion

namespace GDFFoundation
{
    [Serializable]
    public class GDFAccountCurrency : GDFAccountData
    {
        
        #region Instance fields and properties
        
        /// <summary>
        ///     Defines the environment kind of an account Currency.
        /// </summary>
        public ProjectEnvironment EnvironmentKind { set; get; }


        /// <summary>
        ///     Represents the name of an account Currency.
        /// </summary>
        /// <value>
        ///     The name of the account Currency.
        /// </value>
        [GDFDbLength(256)] 
        [GDFDbDefault("Credits")]
        public string Name { set; get; } = string.Empty;
        [GDFDbDefault(false)]
        public bool Secure { set; get; }
        [GDFDbLength(2048)]
        [GDFDbDefault("")]
        public string SecureTransaction { set; get; }
        [GDFDbDefault(0)]
        public long Amount { set; get; }
        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     Represents an account Currency in the GDFFoundation.
        /// </summary>
        public GDFAccountCurrency()
        {
        }


        #endregion
    }
}