#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFAccountSign.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Text;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents an account sign information.
    /// </summary>
    [Serializable]
    public class GDFAccountSign :  GDFAccountData , IFieldSignHash
    {
        #region Static methods
        /// <summary>
        ///     Creates a device sign using the specified device type, unique identifier, and project reference.
        /// </summary>
        /// <param name="deviceKind">The type of the device (e.g., iOS, Android, etc.).</param>
        /// <param name="uniqueIdentifier">The unique identifier (UDID) of the device.</param>
        /// <param name="project">The ID of the project associated with the device.</param>
        /// <returns>A GDFAccountSign instance containing the generated device sign information.</returns>
        public static GDFAccountSign CreateDeviceSign(string uniqueIdentifier, long project)
        {
            GDFAccountSign result = new GDFAccountSign();
            result.Project = project;
            result.SignType = GDFAccountSignType.DeviceId;
            result.Name = GDFSecurityTools.CryptAes(result.SignType.ToString(), project.ToString(), project.ToString());
            string typeObfuscation = SignTypeObfuscation(result.SignType);
            result.SignHash = typeObfuscation + "-" + GDFSecurityTools.GenerateSha(uniqueIdentifier + project) + "-" + GDFSecurityTools.GenerateSha(uniqueIdentifier);
            result.RescueHash = result.SignHash;
            return result;
        }

        /// <summary>
        ///     Create a sign with email and password
        /// </summary>
        /// <param name="email">The email address for the account</param>
        /// <param name="password">The password for the account</param>
        /// <param name="project">The project ID</param>
        /// <returns>A new instance of GDFAccountSign with the specified email, password, and project ID</returns>
        public static GDFAccountSign CreateEmailPassword(string email, string password, long project)
        {
            GDFAccountSign rReturn = new GDFAccountSign();
            rReturn.Project = project;
            rReturn.Name = GDFSecurityTools.CryptAes(EmailToPartialString(email), project.ToString(), project.ToString());
            string tType = SignTypeObfuscation(rReturn.SignType);
            rReturn.SignType = GDFAccountSignType.EmailPassword;
            rReturn.SignHash = tType + "-" + GDFSecurityTools.GenerateSha(email + password + project) + "-" + GDFSecurityTools.GenerateSha(password + email);
            rReturn.RescueHash = tType + "-" + GDFSecurityTools.GenerateSha(email + project) + "-" + GDFSecurityTools.GenerateSha(email);
            return rReturn;
        }

        /// <summary>
        ///     Creates an OAuth-based account sign object with the specified authentication kind, user identifier, and project reference.
        /// </summary>
        /// <param name="authKind">The type of OAuth provider used for authentication.</param>
        /// <param name="userIdentifier">The identifier of the user, typically provided by the OAuth service.</param>
        /// <param name="project">The project reference key used to associate the account sign with a specific project.</param>
        /// <returns>A newly created <see cref="GDFAccountSign" /> object configured with the provided details.</returns>
        public static GDFAccountSign CreateOAuthSign(GDFOAuthKind authKind, string userIdentifier, long project)
        {
            GDFAccountSign result = new GDFAccountSign();
            result.Project = project;
            result.SignType = (GDFAccountSignType)authKind;
            result.Name = GDFSecurityTools.CryptAes(result.SignType.ToString(), project.ToString(), project.ToString());
            string signKind = SignTypeObfuscation(result.SignType);
            result.SignHash = signKind + "-" + GDFSecurityTools.GenerateSha(userIdentifier + project) + "-" + GDFSecurityTools.GenerateSha(userIdentifier);
            result.RescueHash = result.SignHash;
            return result;
        }

        /// <summary>
        ///     Obfuscates the device type based on the given sign type.
        /// </summary>
        /// <param name="kind">The sign type to obfuscate.</param>
        /// <returns>The obfuscated device type sign.</returns>
        private static string SignTypeObfuscation(GDFAccountSignType kind)
        {
            string result = "";
            switch (kind)
            {
                case GDFAccountSignType.EmailPassword:
                    result = "A";
                break;
                case GDFAccountSignType.DeviceId:
                    result = "D";
                break;
                case GDFAccountSignType.Facebook:
                    result = "E";
                break;
                case GDFAccountSignType.Google:
                    result = "F";
                break;
                case GDFAccountSignType.Apple:
                    result = "G";
                break;
                case GDFAccountSignType.Microsoft:
                    result = "H";
                break;
                case GDFAccountSignType.Twitter:
                    result = "I";
                break;
                case GDFAccountSignType.LinkedIn:
                    result = "J";
                break;
                case GDFAccountSignType.Discord:
                    result = "K";
                break;
                default:
                    result = "Z";
                break;
            }

            return result;
        }


        /// <summary>
        ///     Converts the original email address to a partially masked string.
        /// </summary>
        /// <param name="email">The original email address.</param>
        /// <returns>A partially masked string representation of the email address.</returns>
        public static string EmailToPartialString(string email)
        {
            StringBuilder result = new StringBuilder();

            if (email.Length > 6)
            {
                result.Append(email[0]);
                result.Append(email[1]);
                for (int t = 2; t < email.Length - 2; t++)
                {
                    if (email[t] == '@')
                    {
                        result.Append("@");
                    }
                    else if (email[t] == ' ')
                    {
                        // It's a login email password case ... 
                        result.Append(" / ");
                        if (email.Length >= t + 4)
                        {
                            t++;
                            result.Append(email[t]);
                            t++;
                            result.Append(email[t]);
                        }
                    }
                    else if (email[t] == '.')
                    {
                        result.Append(".");
                    }
                    else
                    {
                        result.Append("•");
                    }
                }

                result.Append(email[^2]);
                result.Append(email[^1]);
            }
            else
            {
                result.Append(email);
            }

            return result.ToString();
        }

        /// <summary>
        ///     Generate a rescue hash by concatenating the email and project ID and generating a SHA hash.
        /// </summary>
        /// <param name="email">The email associated with the account</param>
        /// <param name="project">The ID of the project</param>
        /// <returns>The rescue hash</returns>
        public static string GenerateHashRescue(string email, long project)
        {
            return GDFSecurityTools.GenerateSha(email + project) + "-" +
                   GDFSecurityTools.GenerateSha(email);
        }

        #endregion

        #region Instance fields and properties

        /// <summary>
        ///     Name of the sign.
        /// </summary>
        public string Name { set; get; } = string.Empty;

        /// <summary>
        ///     The rescue hash associated with the account sign.
        /// </summary>
        public string RescueHash { set; get; } = string.Empty;

        /// <summary>
        ///     Represents the sign hash associated with an account.
        /// </summary>
        public string SignHash { set; get; } = string.Empty;

        /// <summary>
        ///     Represents the type of sign for an account.
        /// </summary>
        public GDFAccountSignType SignType { set; get; } = GDFAccountSignType.None;

        /// <summary>
        ///     Represents the rescue token for an account sign in the GDFAccountSign class.
        /// </summary>
        /// <remarks>
        ///     The rescue token is used in the account sign flow to securely verify and rescue an account.
        /// </remarks>
        public string TokenRescue { set; get; } = string.Empty;

        /// <summary>
        ///     The token rescue limit for an account sign.
        /// </summary>
        /// <remarks>
        ///     This property represents the maximum number of token rescue attempts allowed for an account sign.
        ///     Once this limit is reached, further token rescue attempts will be denied.
        /// </remarks>
        public DateTime TokenRescueLimit { set; get; }

        #endregion
    }
}