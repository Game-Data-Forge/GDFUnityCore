#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFConstants.cs create at 2025/04/03 09:04:09
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Globalization;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Constants class for Game-Data-Forge.
    /// </summary>
    public abstract class GDFConstants
    {
        #region Constants
        
        public const string K_CONSENT_VERSION = "1.0.0";
        public const string K_CONSENT_NAME = "Game-Data-Forge";
        //TODO change this URL 
        public const string K_CONSENT_URL = "https://www.game-data-forge.com/url_de_licence_a_modifier";
        
        public const string C_EMPTY_STRING = "";

        /// <summary>
        ///     The Editor_CheckDatasRepeatEvery variable determines the interval at which data checks are performed in the editor.
        ///     This variable is set to 30, indicating that data checks are performed every 30 seconds.
        /// </summary>
        public const int Editor_CheckDatasRepeatEvery = 30;

        /// <summary>
        ///     Editor_CheckRightsRepeatEvery constant. Represents the time interval in seconds for checking user rights repeatedly in the editor.
        /// </summary>
        public const int Editor_CheckRightsRepeatEvery = 60;

        /// <summary>
        ///     Constants related to the editor's removal locker.
        /// </summary>
        public const int Editor_RemoveLockerAfter = 600;

        /// <summary>
        ///     The Editor_UpdateDataSelectedRepeatEvery variable determines the repeat interval for updating selected data in the editor.
        /// </summary>
        public const int Editor_UpdateDataSelectedRepeatEvery = 10;
        

        /// <summary>
        ///     The constant value for variable K_A.
        /// </summary>
        /// <remarks>
        ///     This is defined in the GDFConstants class in the GDFFoundation namespace.
        /// </remarks>
        public const string K_A = "A";

        public const string K_ACCOUNT_EXCEPTION_CATEGORY = "ACC";
        public const int K_ACCOUNT_EXCEPTION_INDEX = 21000;
        
        public const string K_COUNTRY_EXCEPTION_CATEGORY = "CTY";
        public const int  K_COUNTRY_EXCEPTION_INDEX= 22000;
        
        public const string K_PROJECT_EXCEPTION_CATEGORY = "PRJ";
        public const int K_PROJECT_EXCEPTION_INDEX = 30000;
        
        public const string K_SERVICE_EXCEPTION_CATEGORY = "SRV";
        public const int K_SERVICE_EXCEPTION_INDEX = 24000;
        
        public const string K_SIGN_EXCEPTION_CATEGORY = "SIG";
        public const int K_SIGN_EXCEPTION_INDEX = 22000;
        
        public const string K_SECRET_KEY_EXCEPTION_CATEGORY = "SKY";
        public const int    K_SECRET_KEY_EXCEPTION_INDEX = 26000;
        
        public const string K_PUBLIC_KEY_EXCEPTION_CATEGORY = "PKY";
        public const int    K_PUBLIC_KEY_EXCEPTION_INDEX = 27000;
        
        public const string K_SYNC_EXCEPTION_CATEGORY = "SYC";
        public const int K_SYNC_EXCEPTION_INDEX = 90000;
        
        public const string K_TOKEN_EXCEPTION_CATEGORY = "TKN";
        public const int K_TOKEN_EXCEPTION_INDEX = 25000;
        
        public const string K_UNICITY_EXCEPTION_CATEGORY = "UNY";
        public const int K_UNICITY_EXCEPTION_INDEX = 10000;
        
        public const string K_CONSENT_EXCEPTION_CATEGORY = "CSN";
        public const int K_CONSENT_EXCEPTION_INDEX = 23000;
        
        public const string K_DEVICE_EXCEPTION_CATEGORY = "DVC";
        public const int K_DEVICE_EXCEPTION_INDEX = 24000;
        
        public const string K_OAUTH_EXCEPTION_CATEGORY = "OAU";
        public const int K_OAUTH_EXCEPTION_INDEX = 25000;
        
        public const string K_SIGN_HASH_EXCEPTION_CATEGORY = "HSH";
        public const int K_SIGN_HASH_EXCEPTION_INDEX = 26000;
        
        public const string K_LANGUAGE_ISO_EXCEPTION_CATEGORY = "LNG";
        public const int K_LANGUAGE_ISO_EXCEPTION_INDEX = 25000;
        
        public const string K_DASHBOARD_EXCEPTION_CATEGORY = "DSH";
        public const int K_DASHBOARD_EXCEPTION_INDEX = 40000;
        
        public const string K_CURRENCY_EXCEPTION_CATEGORY = "CRY";
        public const int K_CURRENCY_EXCEPTION_INDEX = 99000;
            
        public static int K_CUSTOM_CONSENT_MAX = 20;
        public static int K_SERVICE_MAX = 99;
        public static int K_CURRENCY_MAX = 99;
        public static int K_CHANNEL_MAX = 99;
        public static int K_CHANNEL_MIN = 0;
        public static int K_SIGN_HASH_MAX = 128;
        public static int K_SIGN_HASH_MIN = 24;
            
        public const int K_DEVICE_IDENTIFIER_LENGTH_MIN = 32;
        public const string K_EMAIL_EREG_PATTERN = @"^(?!.*\.\.)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        public const int K_EMAIL_LENGTH_MAX = 128;
        public const int K_EMAIL_LENGTH_MIN = 6;

        /// <summary>
        ///     Represents a fake crucial key used in the application.
        /// </summary>
        public const string K_FAKE_CRUCIAL_KEY = "89er845ds1f23zegtREF67F89ZR4G5DS64G4Q65SF4CD8SQ78945318964dfs4897T";

        /// <summary>
        ///     Represents the environment kind of the fake project.
        /// </summary>
        public const ProjectEnvironment K_FAKE_PROJECT_ENVIRONMENT = ProjectEnvironment.Development;

        /// <summary>
        ///     Represents the fake project ID constant.
        /// </summary>
        public const long K_FAKE_PROJECT_ID = 11111;

        /// <summary>
        ///     This constant represents the fake project key.
        /// </summary>
        public const string K_FAKE_PROJECT_KEY = "78945318964df89zegter845ds1f23s4897TREFG4Q65SF4CD8SQ67F89ZR4G5DS64";

        public const string K_FAKE_PROJECT_NAME = "MY GAME";

        /// <summary>
        ///     Represents the fake secret key used in the project.
        /// </summary>
        public const string K_FAKE_SECRET_KEY = "zegt78945318964df89er845ds1f23s4897TREF67F89ZR4G5DS64G4Q65SF4CD8SQ";

        /// <summary>
        ///     Represents the treat key used in a web treat configuration.
        /// </summary>
        public const string K_FAKE_TREAT_KEY = "egtREF67F889er845ds1f23z9ZR4G5DS64GCD8SQ78945318964dfs4897T4Q65SF4";

        /// <summary>
        ///     GDFConstants class contains various constant values used throughout the application.
        /// </summary>
        public const string K_GDFConfigClusterPath = "appsettings" + K_GDFConfigExtension;

        /// <summary>
        ///     Provides constant values and settings related to the Game-Data-Forge (GDF) configuration extension.
        /// </summary>
        public const string K_GDFConfigExtension = ".json";

        /// <summary>
        ///     Represents the file path for the GDF Unity Editor configuration file.
        /// </summary>
        public const string K_GDFConfigUnityEditorPath = "GDFConfigUnityEditor" + K_GDFConfigExtension;

        /// <summary>
        ///     Represents the path of the Unity runtime configuration file for the Game-Data-Forge (GDF) framework.
        /// </summary>
        public const string K_GDFConfigUnityRuntimePath = "GDFConfigUnityRuntime" + K_GDFConfigExtension;

        /// <summary>
        ///     Constant representing the hashtag symbol (#).
        /// </summary>
        public const string K_HASHTAG = "#";

        /// <summary>
        ///     The character used to represent a minus sign.
        /// </summary>
        public const string K_MINUS = "-";

        public const int K_O_AUTH_ACCESS_TOKEN_LENGTH_MIN = 10;


        // @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[-_+!?=])[A-Za-z\d\-_\+!?=]+$"
        public const string K_PASSWORD_EREG_PATTERN = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[^a-zA-Z0-9\s]).+$";
        public const int K_PASSWORD_LENGTH_MAX = 128;
        public const int K_PASSWORD_LENGTH_MIN = 12;
        public const string K_PASSWORD_REQUIRE = "one uppercase, one lowercase, one number and one special (not a letter, number or space)";

        /// <summary>
        ///     The global reference area for Game-Data-Forge.
        /// </summary>
        public const Int64 K_REFERENCE_AREA_GLOBAL = K_REFERENCE_AREA_RANGE * 10000;

        /// <summary>
        ///     The range of a reference area.
        /// </summary>
        public const Int64 K_REFERENCE_AREA_RANGE = 100000000000000;

        // long :0 to 18 446 744 073 709 551 615
        // ushort :	0 to 65 535 (use as range to prevent migration)
        // reference with long =>  1 | 8 446 7 |  44 073 709 551 615
        // reference => 0/1 | ushort | 10^14
        // number of row : 1 000                     : 10^3  : thousand (kilo)
        // number of row : 1 000 000                 : 10^6  : million (mega)
        // number of row : 1 000 000 000             : 10^9  : billion (giga)
        // number of row : 1 000 000 000 000         : 10^12 : trillion (tera)
        // for security use 10^12 for 
        // example for range 1244 => 1 01244 00 xxxxxxxxxxxx
        /// <summary>
        ///     The reference size used for generating unique identifiers.
        /// </summary>
        public const ushort K_REFERENCE_SIZE = 12;

        /// <summary>
        ///     Represents the constant value for the reference unique size.
        /// </summary>
        public const ushort K_REFERENCE_UNIQUE_SIZE = 16;

        /// <summary>
        ///     The K_RESOURCES constant is a string that represents the path to the localization resources in the GDFWebEditor project.
        /// </summary>
        public const string K_RESOURCES = "Resources";

        /// <summary>
        ///     Represents the newline character sequence.
        /// </summary>
        public const string K_ReturnLine = "\n";

        /// <summary>
        ///     Represents the length of a stat key.
        /// </summary>
        public const int K_STAT_KEY_LENGHT = 16;

        public const int K_STORAGE_BLOCK_SIZE_LENGTH = 2048;

        public const int K_STORAGE_SYNC_LIMIT = 200;

        /// <summary>
        ///     Represents the length of a token.
        /// </summary>
        public const int K_TOKEN_LENGHT = 64;


        #endregion

        #region Static fields and properties

        public static string DefaultLanguage = "en-US";

        /// <summary>
        ///     Represents the format string used to format double values.
        /// </summary>
        /// <remarks>
        ///     The format string should adhere to the standard .NET formatting rules for double values.
        /// </remarks>
        public static string DoubleFormat = "F5";

        /// <summary>
        ///     The DoubleSQLFormat class contains a constant string that represents the format to apply when converting a double value to an SQL string.
        /// </summary>
        public static string DoubleSQLFormat = "5";

        /// <summary>
        ///     The formatting string used for floating point numbers.
        /// </summary>
        public static string FloatFormat = "F5";

        /// <summary>
        ///     Represents the format used for storing floating point numbers in SQL databases.
        /// </summary>
        public static string FloatSQLFormat = "5";

        /// <summary>
        ///     Represents the format for displaying country-specific information.
        /// </summary>
        public static CultureInfo FormatCountry = CultureInfo.InvariantCulture;

        /// <summary>
        ///     Constants related to development name.
        /// </summary>
        public static string K_DEVELOPMENT_NAME = "Preview";

        /// <summary>
        ///     This class contains constants used in the GDFFoundation namespace.
        /// </summary>
        public static string K_EMPTY_STRING = string.Empty;

        /// <summary>
        ///     The K_GDF constant is a string representing the value "Game-Data-Forge"
        /// </summary>
        public static string K_GDF = "Game-Data-Forge";
        public static string K_GDF_TRIGRAMME = "GDF";

        /// <summary>
        ///     Represents the URL constant for the Net-Worked Data (GDF) platform.
        ///     This constant is used to specify the base URL for all API calls and web requests within the GDF platform.
        /// </summary>
        public static string K_GDFURL = "https://www.game-data-forge.com";

        /// <summary>
        ///     The K_IDEMOBI constant represents the string value "idéMobi".
        /// </summary>
        public static string K_IDEMOBI = "idéMobi";

        /// <summary>
        ///     The name of the playtest.
        /// </summary>
        public static string K_PLAYTEST_NAME = "Playtest";

        /// <summary>
        ///     Represents the name of the post-production phase.
        /// </summary>
        public static string K_POSTPRODUCTION_NAME = "Postprod";

        /// <summary>
        ///     A class that contains constants related to pre-production environment.
        /// </summary>
        public static string K_PREPRODUCTION_NAME = "Preprod";

        /// <summary>
        ///     This class contains constants related to production environments.
        /// </summary>
        public static string K_PRODUCTION_NAME = "Prod";

        /// <summary>
        ///     The name of the qualification.
        /// </summary>
        public static string K_QUALIFICATION_NAME = "Qualification";

        /// <summary>
        ///     Represents the random crucial key constant used in the Game-Data-Forge framework.
        /// </summary>
        public static string K_RANDOM_CRUCIAL_KEY = GDFRandom.RandomGameDataForgeToken();

        /// <summary>
        ///     Represents the environment of a random project.
        /// </summary>
        public static ProjectEnvironment K_RANDOM_PROJECT_ENVIRONMENT = ProjectEnvironment.Production;


        /// <summary>
        ///     Constant variable representing the random project ID.
        /// </summary>
        public static long K_RANDOM_PROJECT_ID = GDFRandom.LongNumeric(5);

        /// <summary>
        ///     Represents the randomly generated project key for the application.
        /// </summary>
        public static string K_RANDOM_PROJECT_KEY = GDFRandom.RandomGameDataForgeToken();

        /// <summary>
        ///     Represents the random secret key used for encryption and decryption in Net-Worked Data framework.
        /// </summary>
        public static string K_RANDOM_SECRET_KEY = GDFRandom.RandomGameDataForgeToken();

        /// <summary>
        ///     Represents a randomly generated treat key used in the GDFWebTreatConfiguration class.
        /// </summary>
        /// <remarks>
        ///     This variable is a constant string value that is randomly generated using the RandomGameDataForgeToken method from the GDFRandom class.
        /// </remarks>
        /// <value>
        ///     The treat key value.
        /// </value>
        public static string K_RANDOM_TREAT_KEY = GDFRandom.RandomGameDataForgeToken();

        /// <summary>
        ///     Empty field constant value.
        /// </summary>
        public static string kFieldEmpty = "empty";

        /// <summary>
        ///     Represents a string constant indicating "none".
        /// </summary>
        public static string kFieldNone = "none";

        /// <summary>
        ///     Represents the constant value for a non-empty field.
        /// </summary>
        public static string kFieldNotEmpty = "not empty";

        /// <summary>
        ///     The character used as a field separator A.
        /// </summary>
        public static char kFieldSeparatorA_char = '•';

        /// <summary>
        ///     The field separator A.
        /// </summary>
        public static string kFieldSeparatorA = string.Empty + kFieldSeparatorA_char;


        /// <summary>
        ///     Substitute string used to replace instances of <see cref="GDFConstants.kFieldSeparatorA" />
        ///     when cleaning and protecting strings.
        /// </summary>
        public static string kFieldSeparatorASubstitute = "@1#";

        /// <summary>
        ///     This variable represents the field separator B character.
        /// </summary>
        public static char kFieldSeparatorB_char = ':';

        /// <summary>
        ///     Represents the field separator B character used in string manipulation.
        /// </summary>
        public static string kFieldSeparatorB = string.Empty + kFieldSeparatorB_char;


        /// <summary>
        ///     Represents the substitute string for the second field separator in a text that needs to be cleaned.
        /// </summary>
        public static string kFieldSeparatorBSubstitute = "@2#";

        /// <summary>
        ///     Constant representing the field separator character '_'.
        /// </summary>
        public static char kFieldSeparatorC_char = '_';

        /// <summary>
        ///     Constants class for Field Separator Characters.
        /// </summary>
        public static string kFieldSeparatorC = string.Empty + kFieldSeparatorC_char;


        public static string kFieldSeparatorCSubstitute = "@3#";

        /// <summary>
        ///     The character used as field separator D.
        /// </summary>
        public static char kFieldSeparatorD_char = '∆';

        /// <summary>
        ///     Represents the field separator D used in the Game-Data-Forge framework.
        /// </summary>
        /// <remarks>
        ///     This field separator is used to separate multiple values within a single string field.
        ///     It has the Unicode character '∆' (U+2206) and is added as a string representation in the constant variable <see cref="GDFConstants.kFieldSeparatorD" />.
        /// </remarks>
        public static string kFieldSeparatorD = string.Empty + kFieldSeparatorD_char;


        /// <summary>
        ///     Represents the substitute string for the field separator D.
        /// </summary>
        public static string kFieldSeparatorDSubstitute = "@4#";

        /// <summary>
        ///     Constant representing the character field separator E.
        /// </summary>
        public static char kFieldSeparatorE_char = '∂';

        /// <summary>
        ///     The field separator E character used in string manipulation.
        /// </summary>
        public static string kFieldSeparatorE = string.Empty + kFieldSeparatorE_char;


        /// <summary>
        ///     A substitute for the field separator character '∂'.
        /// </summary>
        public static string kFieldSeparatorESubstitute = "@5#";

        /// <summary>
        ///     Represents the character used as the field separator F.
        /// </summary>
        public static char kFieldSeparatorF_char = ';';

        /// <summary>
        ///     Field separator F used in various string manipulation operations.
        /// </summary>
        public static string kFieldSeparatorF = string.Empty + kFieldSeparatorF_char;


        /// <summary>
        ///     Represents the key used to store the salt value A in the preferences.
        /// </summary>
        public static string kPrefSaltAKey = "SaltA";

        /// <summary>
        ///     Represents the key for the 'SaltB' preference salt value.
        /// </summary>
        public static string kPrefSaltBKey = "SaltB";

        /// <summary>
        ///     The constant key for the valid salt preference.
        /// </summary>
        public static string kPrefSaltValidKey = "SaltValid";

        /// <summary>
        ///     Represents the standard separator used for text CSV protection and unprotection.
        /// </summary>
        public static string kStandardSeparator = "|";

        /// <summary>
        ///     Substitute for the standard separator used in CSV text processing.
        /// </summary>
        public static string kStandardSeparatorSubstitute = "@0#";

        public static string LogoFormat = "svg+xml";
        public static string LogoBase64 =
            @"PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9Im5vIj8+CjwhRE9DVFlQRSBzdmcgUFVCTElDICItLy9XM0MvL0RURCBTVkcgMS4xLy9FTiIgImh0dHA6Ly93d3cudzMub3JnL0dyYXBoaWNzL1NWRy8xLjEvRFREL3N2ZzExLmR0ZCI+Cjxzdmcgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDgwIDgwIiB2ZXJzaW9uPSIxLjEiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgeG1sbnM6eGxpbms9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkveGxpbmsiIHhtbDpzcGFjZT0icHJlc2VydmUiIHhtbG5zOnNlcmlmPSJodHRwOi8vd3d3LnNlcmlmLmNvbS8iIHN0eWxlPSJmaWxsLXJ1bGU6ZXZlbm9kZDtjbGlwLXJ1bGU6ZXZlbm9kZDtzdHJva2UtbGluZWpvaW46cm91bmQ7c3Ryb2tlLW1pdGVybGltaXQ6MjsiPgogICAgPHJlY3QgaWQ9IkFzc2V0cyIgeD0iLTExMCIgeT0iLTEwIiB3aWR0aD0iMzAwMCIgaGVpZ2h0PSI1MDAwIiBzdHlsZT0iZmlsbDpub25lOyIvPgogICAgPGcgaWQ9IlN0YW5kYXJkLUludGVyZmFjZSIgc2VyaWY6aWQ9IlN0YW5kYXJkIEludGVyZmFjZSI+CiAgICA8L2c+CiAgICA8ZyBpZD0iR2FtZS1EYXRhLUZvcmdlLTIwMjUiIHNlcmlmOmlkPSJHYW1lLURhdGEtRm9yZ2UgMjAyNSI+CiAgICAgICAgPGc+CiAgICAgICAgICAgIDxnPgogICAgICAgICAgICAgICAgPGNpcmNsZSBjeD0iMzkuOTg4IiBjeT0iNDAiIHI9IjM5IiBzdHlsZT0iZmlsbDpub25lOyIvPgogICAgICAgICAgICAgICAgPGc+CiAgICAgICAgICAgICAgICAgICAgPGNsaXBQYXRoIGlkPSJfY2xpcDEiPgogICAgICAgICAgICAgICAgICAgICAgICA8Y2lyY2xlIGN4PSIzOS45ODgiIGN5PSI0MCIgcj0iMzkiLz4KICAgICAgICAgICAgICAgICAgICA8L2NsaXBQYXRoPgogICAgICAgICAgICAgICAgICAgIDxnIGNsaXAtcGF0aD0idXJsKCNfY2xpcDEpIj4KICAgICAgICAgICAgICAgICAgICAgICAgPGc+CiAgICAgICAgICAgICAgICAgICAgICAgICAgICA8Zz4KICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICA8cGF0aCBkPSJNMzguMTksNzcuODkzQzE4LjI2NCw3Ni43NyAyLjQyOSw2MC4yMzUgMi40MjksNDAuMDNDMi40MjksMTkuMSAxOS40MjMsMi4xMDcgNDAuMzUzLDIuMTA3QzUwLjgyNywyLjEwNyA1OS4zMywxMC42MSA1OS4zMywyMS4wODRDNTkuMzMsMzEuNTM0IDUwLjg2Niw0MC4wMjMgNDAuNDI1LDQwLjA2MUw0MC4zNTMsNDAuMDYxQzI5Ljg3OSw0MC4wNjEgMjEuMzc2LDQ4LjU2NSAyMS4zNzYsNTkuMDM4QzIxLjM3Niw2OC43ODEgMjguNzMzLDc2LjgxOCAzOC4xOSw3Ny44OTNaIiBzdHlsZT0iZmlsbDpyZ2IoOTksMTAwLDEwMik7ZmlsbC1vcGFjaXR5OjAuNTsiLz4KICAgICAgICAgICAgICAgICAgICAgICAgICAgIDwvZz4KICAgICAgICAgICAgICAgICAgICAgICAgPC9nPgogICAgICAgICAgICAgICAgICAgIDwvZz4KICAgICAgICAgICAgICAgIDwvZz4KICAgICAgICAgICAgICAgIDxwYXRoIGQ9Ik0zOS45ODgsMUM2MS41MTMsMSA3OC45ODgsMTguNDc1IDc4Ljk4OCw0MEM3OC45ODgsNjEuNTI1IDYxLjUxMyw3OSAzOS45ODgsNzlDMTguNDY0LDc5IDAuOTg4LDYxLjUyNSAwLjk4OCw0MEMwLjk4OCwxOC40NzUgMTguNDY0LDEgMzkuOTg4LDFaTTM5Ljk4OCwzLjczN0MxOS45NzQsMy43MzcgMy43MjUsMTkuOTg2IDMuNzI1LDQwQzMuNzI1LDYwLjAxNCAxOS45NzQsNzYuMjYzIDM5Ljk4OCw3Ni4yNjNDNjAuMDAzLDc2LjI2MyA3Ni4yNTIsNjAuMDE0IDc2LjI1Miw0MEM3Ni4yNTIsMTkuOTg2IDYwLjAwMywzLjczNyAzOS45ODgsMy43MzdaIiBzdHlsZT0iZmlsbDp3aGl0ZTsiLz4KICAgICAgICAgICAgICAgIDxwYXRoIGQ9Ik0xNy43NTIsNDcuNTM2QzEyLjA5Niw0Ny41MzYgNy41MDQsNDMuMTEgNy41MDQsMzcuNjU4QzcuNTA0LDMzLjMwNyAxMC40MjksMjkuNjA5IDE0LjQ4NSwyOC4yOTNDMTQuMzg1LDI3Ljg3OSAxNC4zMzEsMjcuNDQ2IDE0LjMzMSwyNy4wMDFDMTQuMzMxLDIzLjk4IDE2Ljc4NCwyMS41MjcgMTkuODA1LDIxLjUyN0MyMC4wMTIsMjEuNTI3IDIwLjIxNywyMS41MzkgMjAuNDE4LDIxLjU2MUMyNS4zODksMTYuNDc2IDMyLjMyMywxMy4zMTcgMzkuOTg4LDEzLjMxN0M0Ny42NTQsMTMuMzE3IDU0LjU4OCwxNi40NzYgNTkuNTU4LDIxLjU2MUM1OS43NiwyMS41MzkgNTkuOTY0LDIxLjUyNyA2MC4xNzIsMjEuNTI3QzYzLjE5MywyMS41MjcgNjUuNjQ1LDIzLjk4IDY1LjY0NSwyNy4wMDFDNjUuNjQ1LDI3LjQ0NyA2NS41OTIsMjcuODgxIDY1LjQ5LDI4LjI5N0M2OS41NDcsMjkuNjEyIDcyLjQ3MywzMy4zMDkgNzIuNDczLDM3LjY2QzcyLjQ3Myw0My4xMTEgNjcuODgxLDQ3LjUzNiA2Mi4yMjQsNDcuNTM2TDE3Ljc1Miw0Ny41MzZaTTMyLjMwOCwyNi4zMkMzMC42MzYsMjYuMjYyIDMwLjE3NiwyOC43MDMgMzAuMDczLDMxLjcyNUMyOS45NywzNC43NDcgMzAuMjM0LDM3LjIxMyAzMS45MzQsMzcuMjcxQzMzLjYzMywzNy4zMjkgMzQuMDY1LDM0Ljg4NyAzNC4xNjgsMzEuODY1QzM0LjI3MiwyOC44NDMgMzMuOTgsMjYuMzc3IDMyLjMwOCwyNi4zMlpNNDcuNjY5LDI2LjMyQzQ1Ljk5NywyNi4zNzcgNDUuNzA1LDI4Ljg0MyA0NS44MDgsMzEuODY1QzQ1LjkxMiwzNC44ODcgNDYuMzQzLDM3LjMyOSA0OC4wNDMsMzcuMjcxQzQ5Ljc0MywzNy4yMTMgNTAuMDA3LDM0Ljc0NyA0OS45MDMsMzEuNzI1QzQ5LjgsMjguNzAzIDQ5LjM0LDI2LjI2MiA0Ny42NjksMjYuMzJaTTIxLjM4Miw1Mi4yOTZDMjEuNzI1LDUwLjIyNiAzNS45MjEsNTEuNzYzIDM2LjQ3Myw1My4yNzZDMzcuMDI2LDU0Ljc4OSAzOS4zMzksNjYuNjU1IDMyLjEzNSw2Ni4yMTJDMjguNjE5LDY1Ljk5NSAyNS43MzUsNjQuMTE3IDIzLjgxOSw2MS41NDdDMjEuODEsNTguODUxIDIwLjg2OCw1NS4zOTQgMjEuMzgyLDUyLjI5NlpNNTguNTk1LDUyLjI5NkM1OS41OTksNTguMzQ3IDU1LjA0Niw2NS43NjggNDcuODQyLDY2LjIxMkM0MC42MzgsNjYuNjU1IDQyLjk1MSw1NC43ODkgNDMuNTAzLDUzLjI3NkM0NC4wNTYsNTEuNzYzIDU4LjI1MSw1MC4yMjYgNTguNTk1LDUyLjI5NloiIHN0eWxlPSJmaWxsOndoaXRlOyIvPgogICAgICAgICAgICA8L2c+CiAgICAgICAgPC9nPgogICAgPC9nPgo8L3N2Zz4K";

        // public static PrintAsciiKind PrintAscii = PrintAsciiKind.Logo | PrintAsciiKind.Information;
        public static PrintAsciiKind PrintAscii = PrintAsciiKind.Information;
        
        
        public static PrintExampleKind PrintExample = PrintExampleKind.None;

        #endregion
    }
}