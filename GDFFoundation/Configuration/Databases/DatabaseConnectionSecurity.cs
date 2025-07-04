#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj DatabaseConnectionSecurity.cs create at 2025/03/25 11:03:36
// ©2024-2025 idéMobi SARL FRANCE

#endregion


#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents an SSL/TLS encryption level for a database connection where the certificate authority (CA) is verified.
    /// </summary>
    [Serializable]
    public enum DatabaseConnectionSecurity
    {
        /// <summary>
        ///     Specifies that no SSL/TLS encryption is used for the database connection.
        /// </summary>
        None = 0,

        /// <summary>
        ///     Specifies that SSL/TLS encryption is preferred for the database connection,
        ///     but the connection will still proceed if SSL/TLS is not available.
        /// </summary>
        Preferred = 1,

        /// <summary>
        ///     Specifies that SSL/TLS encryption is mandatory for the database connection.
        /// </summary>
        Required = 2,

        /// <summary>
        ///     Specifies that the database connection uses SSL/TLS encryption, and the certificate authority (CA) is verified to ensure the authenticity of the server's certificate.
        /// </summary>
        VerifyCA = 3,

        /// <summary>
        ///     Requires the highest level of SSL/TLS security for a database connection,
        ///     including hostname verification to ensure it matches the certificate.
        /// </summary>
        VerifyFull = 4
    }
}