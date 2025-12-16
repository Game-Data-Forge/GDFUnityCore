using Newtonsoft.Json;
using GDFFoundation;
using GDFUnity;
using System;
using UnityEngine;

namespace GDFUnity.Editor
{
    /// <summary>
    /// The GDFLoggerUnityEditor class is an implementation of the IGDFLogger interface used for logging messages in the Unity Editor.
    /// </summary>
    [Serializable]
    public class GDFLoggerUnityEditor : IGDFLogger
    {
        /// <summary>
        /// Represents the project preference constant used in the GDFLoggerUnityEditor class.
        /// </summary>
        private const string K_PROJECT_PREF = nameof(GDFLoggerUnityEditor) + nameof(GDFLogLevel);

        /// <summary>
        /// Represents the logging level for the GDFLoggerUnityEditor class.
        /// </summary>
        [JsonProperty] private GDFLogLevel Level = GDFLogLevel.None;

        /// <summary>
        /// Implementation of the IGDFLogger interface for logging in UnityEditor.
        /// </summary>
        public GDFLoggerUnityEditor(GDFLogLevel sLogLevel)
        {
            Level = sLogLevel;
        }

        /// <summary>
        /// Sets the log level to be displayed by the logger.
        /// </summary>
        /// <param name="sLogLevel">The log level to be set.</param>
        public void SetLogLevel(GDFLogLevel sLogLevel)
        {
            Level = sLogLevel;
            //GDFUserSettings.Save(this);
        }

        /// <summary>
        /// Gets the default log level to be displayed by the logger.
        /// </summary>
        /// <returns>The default log level.</returns>
        public GDFLogLevel DefaultLogLevel()
        {
            return GDFLogLevel.Information;
        }

        /// <summary>
        /// Writes a log message with the specified log level, log category, string and object.
        /// </summary>
        /// <param name="sLogLevel">The log level of the log message.</param>
        /// <param name="sLogCategory">The log category of the log message.</param>
        /// <param name="sString">The log message string.</param>
        /// <param name="sObject">The additional object to include in the log message.</param>
        [HideInCallstack]
        public void WriteLog(GDFLogLevel sLogLevel, GDFLogCategory sLogCategory, string sString, object sObject)
        {
            WriteLog(sLogLevel, sString, sObject);
        }

        /// <summary>
        /// Writes log messages to the log system.
        /// </summary>
        /// <param name="sLogLevel">The log level of the log message.</param>
        /// <param name="sLogCategory">The category of the log message.</param>
        /// <param name="sTitle">The title of the log message.</param>
        /// <param name="sObject">The object associated with the log message.</param>
        /// <param name="sMessages">The log messages to write.</param>
        [HideInCallstack]
        public void WriteLog(GDFLogLevel sLogLevel, GDFLogCategory sLogCategory, string sTitle, object sObject, string[] sMessages)
        {
            WriteLog(sLogLevel, sTitle + "\n" + string.Join('\n', sMessages), sObject);
        }

        /// <summary>
        /// Determines if the debugger is active or not.
        /// </summary>
        /// <returns><c>true</c> if the debugger is active; otherwise, <c>false</c>.</returns>
        public bool IsActivated()
        {
            return true;
        }

        /// <summary>
        /// Retrieves the log level to be displayed by the logger.
        /// </summary>
        /// <returns>The log level to be displayed.</returns>
        public GDFLogLevel LogLevel()
        {
            return Level;
        }

        /// <summary>
        /// Writes a log message to the Unity Editor console.
        /// </summary>
        /// <param name="level">The log level of the message.</param>
        /// <param name="sString">The log message string.</param>
        /// <param name="sObject">The object associated with the log message.</param>
        [HideInCallstack]
        public void WriteLog(GDFLogLevel level, string sString, object sObject)
        {
            sString = "[" + level.ToString() + "] " + sString;

            UnityEngine.Object tObject = sObject as UnityEngine.Object;
            Exception tException = sObject as Exception;

            switch (level)
            {
                case GDFLogLevel.Trace:
                case GDFLogLevel.Debug:
                case GDFLogLevel.Information:
                    Debug.Log(sString, tObject);
                    break;
                case GDFLogLevel.Warning:
                    Debug.LogWarning(sString, tObject);
                    break;
                case GDFLogLevel.Error:
                case GDFLogLevel.Critical:
                    if (tException != null)
                    {
                        Debug.LogException(tException, tObject);
                    }
                    else
                    {
                        Debug.LogError(sString, tObject);
                    }

                    break;
                case GDFLogLevel.None:
                    break;
            }
        }
    }
}