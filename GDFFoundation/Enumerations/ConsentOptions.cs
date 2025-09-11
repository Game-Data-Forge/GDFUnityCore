#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj ConsentOptions.cs create at 2025/08/29 14:08:11
// ©2024-2025 idéMobi SARL FRANCE

#endregion

using System;

namespace GDFFoundation
{
    /// <summary>
    /// Represents the options available for user consent.
    /// This enum is decorated with the <see cref="FlagsAttribute"/> to allow bitwise operations between its members.
    /// </summary>
    [Flags]
    public enum ConsentOptions : UInt64
    {
        /// <summary>
        /// Represents no consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        None = 0UL,

        /// <summary>
        /// Represents the first consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentOne = 1UL << 0,

        /// <summary>
        /// Represents the second consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwo = 1UL << 1,

        /// <summary>
        /// Represents the third consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThree = 1UL << 2,

        /// <summary>
        /// Represents the fourth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFour = 1UL << 3,

        /// <summary>
        /// Represents the fifth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFive = 1UL << 4,

        /// <summary>
        /// Represents the sixth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentSix = 1UL << 5,

        /// <summary>
        /// Represents the seventh consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentSeven = 1UL << 6,

        /// <summary>
        /// Represents the eighth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentEight = 1UL << 7,

        /// <summary>
        /// Represents the ninth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentNine = 1UL << 8,

        /// <summary>
        /// Represents the tenth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTen = 1UL << 9,

        /// <summary>
        /// Represents the eleventh consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentEleven = 1UL << 10,

        /// <summary>
        /// Represents the twelfth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwelve = 1UL << 11,

        /// <summary>
        /// Represents the thirteenth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirteen = 1UL << 12,

        /// <summary>
        /// Represents the fourteenth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFourteen = 1UL << 13,

        /// <summary>
        /// Represents the fifteenth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFifteen = 1UL << 14,

        /// <summary>
        /// Represents the sixteenth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentSixteen = 1UL << 15,

        /// <summary>
        /// Represents the seventeenth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentSeventeen = 1UL << 16,

        /// <summary>
        /// Represents the eighteenth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentEighteen = 1UL << 17,

        /// <summary>
        /// Represents the nineteenth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentNineteen = 1UL << 18,

        /// <summary>
        /// Represents the twentieth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwenty = 1UL << 19,

        /// <summary>
        /// Represents the twenty-first consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwentyOne = 1UL << 20,

        /// <summary>
        /// Represents the twenty-second consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwentyTwo = 1UL << 21,

        /// <summary>
        /// Represents the twenty-third consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwentyThree = 1UL << 22,

        /// <summary>
        /// Represents the twenty-fourth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwentyFour = 1UL << 23,

        /// <summary>
        /// Represents the twenty-fifth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwentyFive = 1UL << 24,

        /// <summary>
        /// Represents the twenty-sixth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwentySix = 1UL << 25,

        /// <summary>
        /// Represents the twenty-seventh consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwentySeven = 1UL << 26,

        /// <summary>
        /// Represents the twenty-eight consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwentyEight = 1UL << 27,

        /// <summary>
        /// Represents the twenty-nineth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentTwentyNine = 1UL << 28,

        /// <summary>
        /// Represents the thirtieth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirty = 1UL << 29,

        /// <summary>
        /// Represents the thirty-first consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirtyOne = 1UL << 30,

        /// <summary>
        /// Represents the thirty-second consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirtyTwo = 1UL << 31,

        /// <summary>
        /// Represents the thirty-third consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirtyThree = 1UL << 32,

        /// <summary>
        /// Represents the thirty-fourth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirtyFour = 1UL << 33,

        /// <summary>
        /// Represents the thirty-fifth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirtyFive = 1UL << 34,

        /// <summary>
        /// Represents the thirty-sixth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirtySix = 1UL << 35,

        /// <summary>
        /// Represents the thirty-seventh consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirtySeven = 1UL << 36,

        /// <summary>
        /// Represents the thirty-eighth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirtyEight = 1UL << 37,

        /// <summary>
        /// Represents the thirty-ninth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentThirtyNine = 1UL << 38,

        /// <summary>
        /// Represents the fortieth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentForty = 1UL << 39,

        /// <summary>
        /// Represents the forty-first consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFortyOne = 1UL << 40,

        /// <summary>
        /// Represents the forty-second consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFortyTwo = 1UL << 41,

        /// <summary>
        /// Represents the forty-third consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFortyThree = 1UL << 42,

        /// <summary>
        /// Represents the forty-fourth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFortyFour = 1UL << 43,

        /// <summary>
        /// Represents the forty-fifth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFortyFive = 1UL << 44,

        /// <summary>
        /// Represents the forty-sixth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFortySix = 1UL << 45,

        /// <summary>
        /// Represents the forty-seventh consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFortySeven = 1UL << 46,

        /// <summary>
        /// Represents the forty-eighth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFortyEight = 1UL << 47,


        /// <summary>
        /// Represents the forty-nineth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFortyNine = 1UL << 48,

        /// <summary>
        /// Represents the fiftieth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFifty = 1UL << 49,

        /// <summary>
        /// Represents the fifty-first consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFiftyOne = 1UL << 50,

        /// <summary>
        /// Represents the fifty-second consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFiftyTwo = 1UL << 51,

        /// <summary>
        /// Represents the fifty-third consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFiftyThree = 1UL << 52,

        /// <summary>
        /// Represents the fifty-fourth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFiftyFour = 1UL << 53,

        /// <summary>
        /// Represents the fifty-fifth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFiftyFive = 1UL << 54,

        /// <summary>
        /// Represents the fifty-sixth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFiftySix = 1UL << 55,

        /// <summary>
        /// Represents the fifty-seventh consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFiftySeven = 1UL << 56,

        /// <summary>
        /// Represents the fifty-eighth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFiftyEight = 1UL << 57,

        /// <summary>
        /// Represents the fifty-nineth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentFiftyNine = 1UL << 58,

        /// <summary>
        /// Represents the sixtieth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentSixty = 1UL << 59,

        /// <summary>
        /// Represents the sixty-first consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentSixtyOne = 1UL << 60,

        /// <summary>
        /// Represents the sixty-second consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentSixtyTwo = 1UL << 61,

        /// <summary>
        /// Represents the sixty-third consent option within the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentSixtyThree = 1UL << 62,

        /// <summary>
        /// Represents the sixty-fourth consent option in the <see cref="ConsentOptions"/> enumeration.
        /// </summary>
        ConsentSixtyFour = 1UL << 63,
    }
}