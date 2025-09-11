#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFAccountSignType.cs create at 2025/05/06 19:05:40
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents the type of account sign-in.
    /// </summary>
    [Serializable]
    //[JsonConverter(typeof(JsonStringEnumConverter))]
    public enum GDFAccountSignType : int
    {
        /// <summary>
        ///     Represents an unknown account sign type.
        /// </summary>
        None = 0, // NEVER CHANGE INT VALUE !

        /// <summary>
        ///     Represents the device ID sign type.
        /// </summary>
        DeviceId = 1, // NEVER CHANGE INT VALUE !

        /// <summary>
        ///     Represents the sign-in type for an account using an email and password.
        /// </summary>
        EmailPassword = 10, // NEVER CHANGE INT VALUE !

        /// <summary>
        ///     Represents the Facebook sign-in option for an GDFAccount.
        /// </summary>
        Facebook = 20, // NEVER CHANGE INT VALUE !

        /// <summary>
        ///     Represents the Google sign-in method.
        /// </summary>
        Google = 21, // NEVER CHANGE INT VALUE !

        /// <summary>
        ///     Represents an Apple account sign-in type.
        /// </summary>
        Apple = 22, // NEVER CHANGE INT VALUE !

        /// <summary>
        ///     Microsoft sign-in account type
        /// </summary>
        Microsoft = 23, // NEVER CHANGE INT VALUE !

        /// <summary>
        ///     Represents a Twitter account sign-in type.
        /// </summary>
        Twitter = 24, // NEVER CHANGE INT VALUE !

        /// <summary>
        ///     LinkedIn sign-in method.
        /// </summary>
        LinkedIn = 25, // NEVER CHANGE INT VALUE !

        /// <summary>
        ///     Discord sign-in method for GDFAccount.
        /// </summary>
        Discord = 30, // NEVER CHANGE INT VALUE !
    }
}