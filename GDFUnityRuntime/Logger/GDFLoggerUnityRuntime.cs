using Newtonsoft.Json;
using GDFFoundation;
using System;
using UnityEngine;

namespace GDFUnity
{
    [Serializable]
    public class GDFLoggerUnityRuntime : IGDFLogger
    {
        private const string K_PROJECT_PREF = nameof(GDFLoggerUnityRuntime) + nameof(GDFLogLevel);
        [JsonProperty]
        private GDFLogLevel Level = GDFLogLevel.None;

        public GDFLoggerUnityRuntime(GDFLogLevel sLogLevel)
        {
            Level = sLogLevel;
        }

        public void SetLogLevel(GDFLogLevel sLogLevel)
        {
            Level = sLogLevel;
        }

        public GDFLogLevel DefaultLogLevel()
        {
            return GDFLogLevel.Information;
        }

        public void WriteLog(GDFLogLevel sLogLevel, GDFLogCategory sLogCategory, string sString, object sObject)
        {
            WriteLog(sLogLevel, sString, sObject);
        }

        public void WriteLog(GDFLogLevel sLogLevel, GDFLogCategory sLogCategory, string sTitle, object sObject, string[] sMessages)
        {
            WriteLog(sLogLevel, sTitle + "\n" + string.Join('\n', sMessages), sObject);
        }

        public bool IsActivated()
        {
            return true;
        }

        public GDFLogLevel LogLevel()
        {
            return Level;
        }

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