﻿#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFIp.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents an IP address with four octets: A, B, C, D.
    /// </summary>
    [Serializable]
    public class GDFIp : GDFDataType
    {
        #region Static methods

        /// <summary>
        ///     Generates a random IP address.
        /// </summary>
        /// <returns>A randomly generated GDFIp object representing an IP address.</returns>
        public static GDFIp Random()
        {
            return new GDFIp()
            {
                A = GDFRandom.Int255Numeric(),
                B = GDFRandom.Int255Numeric(),
                C = GDFRandom.Int255Numeric(),
                D = GDFRandom.Int255Numeric(),
            };
        }

        #endregion

        #region Instance fields and properties

        /// <summary>
        ///     Gets or sets the value of property A.
        /// </summary>
        public int A { set; get; }

        /// <summary>
        ///     Represents a property B in the GDFIp class.
        /// </summary>
        public int B { set; get; }

        /// <summary>
        ///     Represents the C property of the GDFIp class.
        /// </summary>
        /// <remarks>
        ///     This property represents the <c>C</c> value of a GDFIp object.
        /// </remarks>
        public int C { set; get; }

        /// <summary>
        ///     Represents the property D of the GDFIp class.
        /// </summary>
        /// <remarks>
        ///     This property is used to store the value of D for a GDFIp object.
        /// </remarks>
        public int D { set; get; }

        #endregion
    }
}