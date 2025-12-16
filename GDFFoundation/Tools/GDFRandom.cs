#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFRandom.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Provides methods for generating random numbers, strings, and other data.
    /// </summary>
    public static class GDFRandom
    {
        #region Static fields and properties

        /// <summary>
        ///     Provides methods for generating random values.
        /// </summary>
        static readonly System.Random KRandom = new System.Random(); // Do Not delete namespace, it's for Unity compatibility.

        private static readonly string[] LoremIpsumWords = new[]
        {
            "Lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit", "sed", "do",
            "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore", "magna", "aliqua", "ut… Itoque ",
            "enim", "ad", "minim", "veniam", "quis", "nostrud", "exercitation", "ullamco", "laboris",
            "nisi", "ut", "aliquip", "ex", "ea", "commodo", "consequat", "salve. Salut", "julius! Horribilis", "ne? Sed etiam",
        };

        #endregion

        #region Static methods

        /// <summary>
        ///     Return random boolean value.
        /// </summary>
        /// <returns>true or false</returns>
        public static bool Boolean()
        {
            if (KRandom.Next(0, 1) == 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool CheckPassword(string password)
        {
            if (password.Length < GDFConstants.K_PASSWORD_LENGTH_MIN)
            {
                return false;
            }

            if (password.Length > GDFConstants.K_PASSWORD_LENGTH_MAX)
            {
                return false;
            }

            if (!Regex.IsMatch(password, GDFConstants.K_PASSWORD_EREG_PATTERN))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     Returns a random number between 0 and 255 (inclusive).
        /// </summary>
        /// <returns>The random number between 0 and 255.</returns>
        public static int Int255Numeric()
        {
            return KRandom.Next(0, 255);
        }

        /// <summary>
        ///     Return a random integer with length = sLength.
        /// </summary>
        /// <param name="sLength">The length of the integer.</param>
        /// <returns>The random integer.</returns>
        public static int IntNumeric(uint sLength)
        {
            uint tLength = Math.Max(1, Math.Min(9, sLength));
            string tReturn = RandomStringNumeric(tLength);
            int rReturn;
            int.TryParse(tReturn, out rReturn);
            return rReturn;
        }

        /// <summary>
        ///     Generates a random long integer with the specified length.
        ///     The result is a string representation of the generated number.
        /// </summary>
        /// <param name="sLength">The length of the desired number (1-18).</param>
        /// <returns>A randomly generated long integer.</returns>
        public static long LongNumeric(uint sLength = 18)
        {
            uint tLength = Math.Max(1, Math.Min(18, sLength));
            string tReturn = RandomStringNumeric(tLength);
            long rReturn;
            long.TryParse(tReturn, out rReturn);
            return rReturn;
        }

        public static string LoremIpsum(int min, int max)
        {
            if (min < 1)
            {
                min = 1;
            }

            if (max < 2)
            {
                max = 2;
            }

            if (min > max)
            {
                (min, max) = (max, min);
            }

            var random = new Random();
            int wordCount = random.Next(min, max + 1);
            string loremText = string.Join(" ", Enumerable.Range(0, wordCount).Select(_ => LoremIpsumWords[random.Next(LoremIpsumWords.Length)]));
            if (!string.IsNullOrWhiteSpace(loremText))
            {
                loremText = char.ToUpper(loremText[0]) + loremText.Substring(1);
            }

            return loremText.TrimEnd(new char[]{'.','!','?',';',',','…'}) +".";
        }

        /// <summary>
        ///     Generates a random integer between sMin (inclusive) and sMax (exclusive)
        /// </summary>
        /// <param name="sMin">The inclusive lower bound of the random number</param>
        /// <param name="sMax">The exclusive upper bound of the random number</param>
        /// <returns>A random integer between sMin (inclusive) and sMax (exclusive)</returns>
        public static int Random(int sMin, int sMax)
        {
            return KRandom.Next(sMin, sMax);
        }

        /// <summary>
        ///     Generates a random captcha string with no mistakes, using characters from "cdefhjkmnpqrtwxyCDEFHJKMNPRTWXY379".
        /// </summary>
        /// <param name="sLength">The length of the captcha string to generate.</param>
        /// <returns>A random captcha string.</returns>
        public static string RandomCaptchaNoMistake(uint sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "cdefhkmnpqrtwxyCEFHKMNPRTWXY379";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Return random DateTime object.
        /// </summary>
        /// <returns>The random DateTime object.</returns>
        public static DateTime RandomDateTime()
        {
            return DateTime.UtcNow.AddSeconds(KRandom.Next(int.MinValue + 1, int.MaxValue - 1));
        }

        /// <summary>
        ///     Returns a random email.
        /// </summary>
        /// <returns>The random email.</returns>
        public static string RandomEmail()
        {
            return RandomStringToken(12).ToLower() + "@" + RandomStringToken(8).ToLower() + ".com";
        }

        /// <summary>
        ///     Return a random Enum value of type T.
        /// </summary>
        /// <typeparam name="T">The type of the Enum.</typeparam>
        /// <returns>A random Enum value.</returns>
        /// <remarks>
        ///     This method returns a random Enum value from the specified Enum type T.
        ///     The Enum values are determined by using the Enum.GetValues method.
        /// </remarks>
        public static T RandomEnum<T>() where T : struct, Enum
        {
            List<T> tList = new List<T>();
            foreach (T tEnum in Enum.GetValues(typeof(T)))
            {
                if (tList.Contains(tEnum) == false)
                {
                    tList.Add(tEnum);
                }
            }

            return tList[KRandom.Next(0, tList.Count - 1)];
        }

        /// <summary>
        ///     Generates a random networked data token.
        /// </summary>
        /// <returns>A string representing the networked data token.</returns>
        public static string RandomGameDataForgeToken()
        {
            return RandomStringToken(8).ToUpper() + "-" +
                   RandomStringToken(8).ToUpper() + "-" +
                   RandomStringToken(8).ToUpper() + "-" +
                   RandomStringToken(8).ToUpper() + "-" +
                   RandomStringToken(8).ToUpper() + "-" +
                   RandomStringToken(8).ToUpper() + "-" +
                   RandomStringToken(8).ToUpper() + "-" +
                   RandomStringToken(8).ToUpper() + "-" +
                   RandomStringToken(8).ToUpper() + "-" +
                   RandomStringToken(8).ToUpper();
        }

        /// <summary>
        ///     Return random color in hex.
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sPrefix">Optional prefix for the hexadecimal color.</param>
        /// <param name="sAlpha">Flag to include alpha in the hexadecimal color.</param>
        public static string RandomHexadecimalColor(string sPrefix = "", bool sAlpha = false)
        {
            int tLength = 6;
            if (sAlpha == true)
            {
                tLength = 8;
            }

            StringBuilder rReturn = new StringBuilder();
            const string cChars = "0123456789ABCDEF";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < tLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }

            return sPrefix + rReturn.ToString();
        }

        public static long RandomLong(long sMin = 0, long sMax = long.MaxValue)
        {
            long value;
            do
            {
                value = KRandom.Next() << 32 | KRandom.Next();
            } while (value < sMin || value > sMax);

            return value;
        }

        public static string RandomPassword()
        {
            string result = RandomStringPassword(GDFConstants.K_PASSWORD_LENGTH_MIN);
            while (CheckPassword(result) == false)
            {
                result = RandomStringPassword(GDFConstants.K_PASSWORD_LENGTH_MIN);
            }

            return result;
        }

        /// <summary>
        ///     Return random string with given length. The string is composed of random characters from the set "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_".
        /// </summary>
        /// <param name="sLength">The length of the random string to generate.</param>
        /// <returns>A randomly generated string.</returns>
        public static string RandomString(uint sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Return random string with length = sLength and char random in "ABCDEFGHIJKLMNOPQRSTUVWXYZ".
        /// </summary>
        /// <param name="sLength">The length of the string.</param>
        /// <returns>The random string.</returns>
        public static string RandomStringAlpha(uint sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Return random string with length = sLength and characters randomly chosen from the Base64 character set "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+/".
        /// </summary>
        /// <returns>The randomly generated string.</returns>
        /// <param name="sLength">The length of the string to be generated.</param>
        public static string RandomStringBase64(uint sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+/";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Return random string with length = sLength and characters randomly chosen from the Base64 character set "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789+/".
        /// </summary>
        /// <returns>The randomly generated string.</returns>
        /// <param name="sLength">The length of the string to be generated.</param>
        public static string RandomStringBase64Url(uint sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Returns a random string with a specified length and characters random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_".
        /// </summary>
        /// <returns>The random string.</returns>
        /// <param name="sLength">The length of the string to generate.</param>
        public static string RandomStringCypher(uint sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                                  //"()[]{}" +
                                  //"+=_" +
                                  //"#$%&" +
                                  //"<^>" +
                                  //".!?:;" +
                                  "0123456789" +
                                  "";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Generates a random key consisting of multiple blocks of random characters, separated by a specified delimiter.
        /// </summary>
        /// <param name="blockLength">The number of characters in each block of the key. Minimum value is 1.</param>
        /// <param name="blockNumber">The number of blocks in the key. Minimum value is 1.</param>
        /// <param name="delimiter">The string that separates each block in the key.</param>
        /// <param name="chars">The set of characters from which the key is generated. The delimiter is excluded from this set.</param>
        /// <returns>A random key composed of the specified number of blocks and characters.</returns>
        public static string RandomStringKey(uint blockLength = 16, uint blockNumber = 7, string delimiter = "-", string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789#$%&*")
        {
            if (blockLength < 1)
            {
                blockLength = 1;
            }

            if (blockNumber < 1)
            {
                blockNumber = 1;
            }

            uint sLength = blockLength * blockNumber + blockNumber - 1;
            chars = chars.Replace(delimiter, "");
            StringBuilder rReturn = new StringBuilder();
            int tCharLenght = chars.Length;
            while (rReturn.Length < sLength)
            {
                if (rReturn.Length > 0 && rReturn.Length % (blockLength + 1) == 0)
                {
                    rReturn.Append(delimiter);
                }
                else
                {
                    rReturn.Append(chars[KRandom.Next(0, tCharLenght)]);
                }
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Return random string with length = sLength and char random in "cdefhjkmnpqrtuvwxyCDEFHJKMNPQRTUVWXY379".
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="sLength">Length parameter.</param>
        public static string RandomStringNoMistake(uint sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "cdefhjkmnpqrtuvwxyCDEFHJKMNPQRTUVWXY379";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Return random string with length = sLength and char random in "0123456789".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringNumeric(uint sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cCharsInit = "123456789";
            rReturn.Append(cCharsInit[KRandom.Next(0, cCharsInit.Length)]);
            const string cChars = "0123456789";
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, cChars.Length)]);
            }

            return rReturn.ToString();
        }

        // private static string RandomStringPassword(uint length)
        // {
        //
        //     StringBuilder rReturn = new StringBuilder();
        //     const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890123456789+-*-#@&+-*-#@&+-*-#@&+-*-#@&+-*-#@&";
        //     int tCharLenght = cChars.Length;
        //     while (rReturn.Length < length)
        //     {
        //         rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
        //     }
        //
        //     return rReturn.ToString();
        // }
        
        private static string RandomStringPassword(uint length)
        {
            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890123456789+-*-#@&+-*-#@&+-*-#@&+-*-#@&";
            int tCharLength = cChars.Length;
            StringBuilder rReturn = new StringBuilder((int)length);

            byte[] randomBytes = new byte[length];

            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomBytes);
            }

            for (int i = 0; i < length; i++)
            {
                rReturn.Append(cChars[randomBytes[i] % tCharLength]);
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Return random string token with length = sLength and characters chosen randomly from "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ".
        /// </summary>
        /// <returns>The string token.</returns>
        /// <param name="sLength">The length of the token.</param>
        public static string RandomStringToken(uint length)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < length)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_".
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringUnix(uint sLength)
        {
            StringBuilder rReturn = new StringBuilder();
            const string cChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            int tCharLenght = cChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn.Append(cChars[KRandom.Next(0, tCharLenght)]);
            }

            return rReturn.ToString();
        }

        /// <summary>
        ///     Returns a short numeric value with the specified length.
        /// </summary>
        /// <param name="sLength">The length of the short numeric value. Should be between 1 and 4.</param>
        /// <returns>A short numeric value.</returns>
        public static short ShortNumeric(uint sLength)
        {
            uint tLength = Math.Max(1, Math.Min(4, sLength));
            string tReturn = RandomStringNumeric(tLength);
            short rReturn;
            short.TryParse(tReturn, out rReturn);
            return rReturn;
        }

        /// <summary>
        ///     Returns an unsigned integer with a specified number of digits.
        /// </summary>
        /// <param name="sLength">The number of digits in the unsigned integer.</param>
        /// <returns>An unsigned integer with the specified number of digits.</returns>
        /// <remarks>
        ///     The returned integer will be between 1 and 9 digits long.
        /// </remarks>
        public static uint UnsignedIntNumeric(uint sLength)
        {
            uint tLength = Math.Max(1, Math.Min(9, sLength));
            string tReturn = RandomStringNumeric(tLength);
            uint rReturn;
            uint.TryParse(tReturn, out rReturn);
            return rReturn;
        }

        /// <summary>
        ///     Returns an unsigned long numeric value within the specified length.
        /// </summary>
        /// <param name="sLength">The length of the numeric value. Must be between 1 and 18 (both inclusive).</param>
        /// <returns>An unsigned long numeric value.</returns>
        public static ulong UnsignedLongNumeric(uint sLength)
        {
            uint tLength = Math.Max(1, Math.Min(18, sLength));
            string tReturn = RandomStringNumeric(tLength);
            ulong rReturn;
            ulong.TryParse(tReturn, out rReturn);
            return rReturn;
        }

        /// <summary>
        ///     Return random unsigned short numeric value with a specified number of digits.
        /// </summary>
        /// <returns>The unsigned short numeric value.</returns>
        /// <param name="sLength">The number of digits in the unsigned short numeric value. Must be between 1 and 4.</param>
        public static ushort UnsignedShortNumeric(uint sLength)
        {
            uint tLength = Math.Max(1, Math.Min(4, sLength));
            string tReturn = RandomStringNumeric(tLength);
            ushort rReturn;
            ushort.TryParse(tReturn, out rReturn);
            return rReturn;
        }

        #endregion
    }
}