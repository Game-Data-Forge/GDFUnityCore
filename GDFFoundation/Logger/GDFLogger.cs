#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFLogger.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Runtime.CompilerServices;
#if !UNITY_EDITOR && ! UNITY_STANDALONE && !UNITY_ANDROID
using System.Text.Json;
#endif

#endregion

#if UNITY_EDITOR
using UnityEngine;
#endif


namespace GDFFoundation
{
    /// <summary>
    ///     GDFLogger class provides logging functionalities.
    /// </summary>
    public static class GDFLogger
    {
        public static bool TraceAll = true;

        /// <summary>
        ///     The Writer class provides methods for logging messages using the chosen logger.
        /// </summary>
        private static IGDFLogger Writer = new GDFConsoleLogger();

        /// <summary>
        ///     The constant string that represents the message template for when a configuration is already loaded.
        /// </summary>
        public const string K_CONFIG_ALREADY_LOADED = "{0} already loaded before!";

        /// <summary>
        ///     Represents a constant string for enable the Razor Runtime Compilation.
        /// </summary>
        public const string K_RAZOR_RUNTIME_COMPILATION_ENABLE = "{0} active Razor Runtime Compilation by {1} configuration!";

        /// <summary>
        ///     Represents the configuration to disable Razor runtime compilation.
        /// </summary>
        public const string K_RAZOR_RUNTIME_COMPILATION_DISABLE = "{0} disable Razor Runtime Compilation by {1} configuration (IsDevelopment = false)!";

        /// <summary>
        ///     The K_RAZOR_COMPILE_NOT_FOR_DEV constant is used to disable Razor Runtime Compilation in development mode.
        /// </summary>
        public const string K_RAZOR_COMPILE_NOT_FOR_DEV = "{0} disable Razor Runtime from parameter : sRuntimeCompileForDev";

        /// <summary>
        ///     The configuration key for finding a value in the app settings file or in a specific JSON file.
        /// </summary>
        public const string K_FOUND_IN_APP_SETTINGS = "{0} config found in app" + "settings.json or in {0}.json!";

        /// <summary>
        ///     The constant string used when a configuration is not found in the app settings.
        /// </summary>
        public const string K_NOT_FOUND_IN_APP_SETTINGS = "{0} config not found in app" + "settings.json or in {0}.json!";

        /// <summary>
        ///     Represents the example value for the K_CONFIG_JSON_EXAMPLE constant.
        /// </summary>
        public const string K_CONFIG_JSON_EXAMPLE = "{0}.json Example";

        /// <summary>
        ///     Sets the writer for the GDFLogger.
        /// </summary>
        /// <param name="sWriter">The writer implementing the IGDFLogger interface.</param>
        public static void SetWriter(IGDFLogger sWriter)
        {
            Writer = sWriter;
        }

        /// <summary>
        ///     Splits a JSON string into an array of strings, separating each element, object, and value with a delimiter.
        /// </summary>
        /// <param name="sJson">The JSON string to split.</param>
        /// <returns>An array of strings with each element, object, and value separated by the delimiter.</returns>
        private static string[] SplitJson(string sJson)
        {
            return sJson.Replace(",", ",•").Replace("{", "{•").Replace("}", "•}").Split('•');
        }

        /// <summary>
        ///     Splits a serialized object into an array of strings, where each string represents a separate line in the serialized format.
        /// </summary>
        /// <param name="sObject">The object to be serialized and split.</param>
        /// <returns>An array of strings representing the serialized object split by lines.</returns>
        public static string[] SplitObjectSerializable(object item)
        {
            #if !UNITY_EDITOR && ! UNITY_STANDALONE && !UNITY_ANDROID
            return JsonSerializer.Serialize(item, new JsonSerializerOptions { WriteIndented = true }).Replace(",\\\"", ",\n\\\"").Replace("{\\\"", "{\n\\\"").Replace("\\\"}", "\\\"\n}").Split('\n');
            //return JsonConvert.SerializeObject(sObject, Formatting.Indented).Replace(",\\\"", ",\n\\\"").Replace("{\\\"", "{\n\\\"").Replace("\\\"}", "\\\"\n}").Split('\n');
            #else
            return item.ToString().Split('\n');
            #endif
        }

        /// <summary>
        ///     Checks if a log for a given <see cref="GDFLogLevel" /> can be written by the <see cref="Writer" />.
        /// </summary>
        /// <param name="sLogLevel">The log level to check</param>
        /// <returns>True if a log with the specified log level can be written, false otherwise</returns>
        private static bool CanWrite(GDFLogLevel sLogLevel)
        {
            return CanWrite(Writer, sLogLevel);
        }

        /// <summary>
        ///     Checks if a log for a given <see cref="GDFLogLevel" /> can be written by the specified writer.
        /// </summary>
        /// <param name="sWriter">The writer implementing the <see cref="IGDFLogger" /> interface.</param>
        /// <param name="sLogLevel">The log level to check.</param>
        /// <returns>True if the log can be written, otherwise false.</returns>
        internal static bool CanWrite(IGDFLogger sWriter, GDFLogLevel sLogLevel)
        {
            // Cannot write a log if the writer is not set
            if (sWriter == null)
            {
                return false;
            }

            // Cannot write a Log if the writer is not activated
            if (!sWriter.IsActivated())
            {
                return false;
            }

            // Cannot write a log if logLevel is below the wanted one or if it is equal to none
            if (sLogLevel < sWriter.LogLevel() || sLogLevel == GDFLogLevel.None)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     [Insert brief description of the method's purpose]
        /// </summary>
        /// <param name="/*[Insert parameter name]*/">[Insert description of the parameter]</param>
        /// <remarks>
        ///     [Insert any additional information about the method, such as side effects or limitations]
        /// </remarks>
        public static void TestLayout()
        {
            TraceAttention("test of layout");
            TraceError("test of layout");
            TraceFailed("test of layout");
            Exception(null);
            Exception(new Exception("Exception's message"));
            Trace("test of layout");
            Information("test of layout number 1");
            Debug("test of layout for debug");
            Warning("test of layout !warning! ");
            Error("test of layout ... Arghhh an error");
            Critical("test of layout ... critical ... I'am dead!");
            Trace("test of layout");
            Information("test of layout number 1");
            Debug("test of layout for debug");
            Warning("test of layout !warning! ");
            Error("test of layout ... Arghhh an error");
            Critical("test of layout ... critical ... I'am dead!");
        }

        /// <summary>
        ///     Writes the log using the wanted <see cref="Writer" />.
        /// </summary>
        /// <param name="sLogLevel">The <see cref="GDFLogLevel" /> indicating the severity level of the log.</param>
        /// <param name="sLogCategory">The <see cref="GDFLogCategory" /> indicating the category of the log.</param>
        /// <param name="sString">The string message to be logged.</param>
        /// <param name="sObject">The optional object to be logged.</param>
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        private static void WriteLog(
            GDFLogLevel logLevel,
            GDFLogCategory logCategory,
            string message,
            object item,
            string callerFile,
            string callerMethod,
            int callerLine
        )
        {
            if (CanWrite(logLevel))
            {
                if (TraceAll == true)
                {
                    Console.WriteLine($@"File {callerFile} method {callerMethod} line {callerLine}");
                }

                Writer.WriteLog(logLevel, logCategory, message, item);
                if (TraceAll == true)
                {
                    Console.WriteLine($@"");
                }
            }
        }

        /// <summary>
        ///     Writes a log using the specified log level, log category, title, object, and messages.
        /// </summary>
        /// <param name="sLogLevel">The log level of the log.</param>
        /// <param name="sLogCategory">The log category of the log.</param>
        /// <param name="sTitle">The title of the log.</param>
        /// <param name="sObject">The object associated with the log.</param>
        /// <param name="sMessages">The messages to be included in the log.</param>
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        private static void WriteLog(
            GDFLogLevel sLogLevel,
            GDFLogCategory sLogCategory,
            string sTitle,
            object sObject,
            string[] sMessages,
            string callerFile,
            string callerMethod,
            int callerLine
        )
        {
            if (CanWrite(sLogLevel))
            {
                if (TraceAll == true)
                {
                    Console.WriteLine($@"File {callerFile} method {callerMethod} line {callerLine}");
                }

                Writer.WriteLog(sLogLevel, sLogCategory, sTitle, sObject, sMessages);
                if (TraceAll == true)
                {
                    Console.WriteLine($@"");
                }
            }
        }

        /// <summary>
        ///     Writes the log using the wanted <see cref="Writer" />.
        /// </summary>
        /// <param name="sLogLevel">The <see cref="GDFLogLevel" /> indicating the severity level of the log.</param>
        /// <param name="sLogCategory">The <see cref="GDFLogCategory" /> indicating the category of the log.</param>
        /// <param name="sString">The string message to be logged.</param>
        /// <param name="sObject">The optional object to be logged.</param>
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        private static void WriteLog(
            GDFLogLevel sLogLevel,
            GDFLogCategory sLogCategory,
            Func<string> sString,
            object sObject,
            string callerFile,
            string callerMethod,
            int callerLine
        )
        {
            if (CanWrite(sLogLevel))
            {
                if (TraceAll == true)
                {
                    Console.WriteLine($@"File {callerFile} method {callerMethod} line {callerLine}");
                }

                Writer.WriteLog(sLogLevel, sLogCategory, sString.Invoke(), sObject);
                if (TraceAll == true)
                {
                    Console.WriteLine($@"");
                }
            }
        }

        /// <summary>
        ///     Writes a log using the specified log level, log category, title, object, and messages.
        /// </summary>
        /// <param name="sLogLevel">The log level of the log.</param>
        /// <param name="sLogCategory">The log category of the log.</param>
        /// <param name="sTitle">The title of the log.</param>
        /// <param name="sObject">The object associated with the log.</param>
        /// <param name="sMessages">The messages to be included in the log.</param>
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        private static void WriteLog(
            GDFLogLevel sLogLevel,
            GDFLogCategory sLogCategory,
            Func<string> sTitle,
            object sObject,
            string[] sMessages,
            [CallerFilePath] string callerFile = "",
            [CallerMemberName] string callerMethod = "",
            [CallerLineNumber] int callerLine = 0
        )
        {
            if (CanWrite(sLogLevel))
            {
                if (TraceAll == true)
                {
                    Console.WriteLine($@"File {callerFile} method {callerMethod} line {callerLine}");
                }

                Writer.WriteLog(sLogLevel, sLogCategory, sTitle.Invoke(), sObject, sMessages);
                if (TraceAll == true)
                {
                    Console.WriteLine($@"");
                }
            }
        }

        /// <summary>
        ///     Sends a <see cref="GDFLogLevel.Trace" /> log using the <see cref="Writer" />.
        /// </summary>
        /// <param name="sMessage">The message to be logged.</param>
        /// <param name="sObject">The object associated with the log message (optional).</param>
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Trace(string sMessage, object sObject = null, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Trace, GDFLogCategory.No, sMessage, sObject, callerFile, callerMethod, callerLine);
        }

        /// <summary>
        ///     Logs detailed trace information, including caller file path, method name, and line number.
        /// </summary>
        /// <param name="message">optional message</param>
        /// <param name="callerFile">The file path of the calling method. Automatically populated by the runtime.</param>
        /// <param name="callerMethod">The name of the calling method. Automatically populated by the runtime.</param>
        /// <param name="callerLine">The line number in the file where the method is called. Automatically populated by the runtime.</param>
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void TracePrint(string message = "", [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Warning, GDFLogCategory.Attention, message, null, callerFile, callerMethod, callerLine);
        }


        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Success(string sMessage, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.None, GDFLogCategory.Success, sMessage, null, callerFile, callerMethod, callerLine);
        }

        /// <summary>
        ///     Writes a log message with the specified message for a failed operation or event.
        /// </summary>
        /// <param name="sMessage">The message to be logged.</param>
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void TraceFailed(string sMessage, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Trace, GDFLogCategory.Failed, sMessage, null, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void TraceAttention(string sMessage, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Trace, GDFLogCategory.Attention, sMessage, null, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void TraceSuccess(string sMessage, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Trace, GDFLogCategory.Success, sMessage, null, callerFile, callerMethod, callerLine);
        }
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void TraceError(string sMessage, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Trace, GDFLogCategory.Error, sMessage, null, callerFile, callerMethod, callerLine);
        }
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Trace(string sTitle, string[] sMessages, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Trace, GDFLogCategory.No, sTitle, null, sMessages, callerFile, callerMethod, callerLine);
        }
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Trace(Func<string> sTitle, string[] sMessages, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Trace, GDFLogCategory.No, sTitle, null, sMessages, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Debug(string message, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Debug, GDFLogCategory.No, message, null, callerFile, callerMethod, callerLine);
        }
        
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static string Line([CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            string message = $"{callerFile} => method {callerMethod} =>line {callerLine}";
            Debug($"--------> {message}", callerFile, callerMethod, callerLine);
            return message;
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Debug(string sTitle, string[] sMessages, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Debug, GDFLogCategory.No, sTitle, null, sMessages, callerFile, callerMethod, callerLine);
        }
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Debug(Func<string> sMessage, object item = null, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Debug, GDFLogCategory.No, sMessage, item, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Debug(Func<string> sTitle, string[] sMessages, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Debug, GDFLogCategory.No, sTitle, null, sMessages, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Information(string sMessage, object sObject = null, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Information, GDFLogCategory.No, sMessage, sObject, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Information(string title, string[] messages, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Information, GDFLogCategory.No, title, null, messages, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Information(Func<string> sMessage, object sObject = null, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Information, GDFLogCategory.No, sMessage, sObject, callerFile, callerMethod, callerLine);
        }
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Information(Func<string> sTitle, string[] sMessages, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Information, GDFLogCategory.No, sTitle, null, sMessages, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Exception(Exception sObject, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Critical, GDFLogCategory.Exception, string.Empty, sObject, callerFile, callerMethod, callerLine);
        }

        /// <summary>
        ///     Logs an exception with the given message.
        /// </summary>
        /// <param name="sMessage">The message to be logged.</param>
        /// <param name="sObject">The exception object to be logged.</param>
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Exception(string sMessage, Exception sObject, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Critical, GDFLogCategory.Exception, sMessage, sObject, callerFile, callerMethod, callerLine);
        }

        /// <summary>
        ///     Sends a <see cref="GDFLogLevel.Warning" /> log using the WriteLog method.
        /// </summary>
        /// <param name="sMessage">The message to be logged.</param>
        /// <param name="sObject">Optional object to be logged alongside the message.</param>
        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Warning(string sMessage, object sObject = null, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Warning, GDFLogCategory.No, sMessage, sObject, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Warning(string sTitle, string[] sMessages, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Warning, GDFLogCategory.No, sTitle, null, sMessages, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Warning(Func<string> sMessage, object sObject = null, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Warning, GDFLogCategory.No, sMessage, sObject, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Warning(Func<string> sTitle, string[] sMessages, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Warning, GDFLogCategory.No, sTitle, null, sMessages, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Error(object sException, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Error, GDFLogCategory.Exception, string.Empty, sException, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Error(string sMessage, object sObject = null, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Error, GDFLogCategory.No, sMessage, sObject, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Error(string sTitle, string[] sMessages, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Error, GDFLogCategory.No, sTitle, null, sMessages, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Error(Func<string> sMessage, object sObject = null, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Error, GDFLogCategory.No, sMessage, sObject, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Critical(string sMessage, object sObject = null, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Critical, GDFLogCategory.No, sMessage, sObject, callerFile, callerMethod, callerLine);
        }

        #if UNITY_EDITOR
        [HideInCallstack]
        #endif
        public static void Critical(Func<string> sMessage, object sObject = null, [CallerFilePath] string callerFile = "", [CallerMemberName] string callerMethod = "", [CallerLineNumber] int callerLine = 0)
        {
            WriteLog(GDFLogLevel.Critical, GDFLogCategory.No, sMessage, sObject, callerFile, callerMethod, callerLine);
        }

        /// <summary>
        ///     Returns the current log level used by the <see cref="Writer" />.
        /// </summary>
        /// <returns>The current log level as a member of the <see cref="GDFLogLevel" /> enum.</returns>
        public static GDFLogLevel LogLevel()
        {
            return Writer?.LogLevel() ?? GDFLogLevel.Information;
        }

        /// <summary>
        ///     Sets the log level for the GDFLogger. The log level determines which log messages will be written by the Writer.
        /// </summary>
        /// <param name="sLevel">The log level to set. Must be one of the values defined in the GDFLogLevel enum.</param>
        public static void SetLogLevel(GDFLogLevel sLevel)
        {
            Writer?.SetLogLevel(sLevel);
        }
    }
}