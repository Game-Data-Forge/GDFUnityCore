#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFConfigurationInstalled.cs create at 2025/03/26 17:03:00
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Collections.Generic;
using Newtonsoft.Json;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     The GDFConfigurationInstalled class is responsible for managing installed configurations.
    /// </summary>
    public static class GDFConfigurationInstalled
    {
        #region Static fields and properties

        /// <summary>
        ///     Represents a list of installed configurations.
        /// </summary>
        private static readonly List<IGDFConfiguration> ConfigurationList = new List<IGDFConfiguration>();

        /// <summary>
        ///     Provides configuration management for installed configurations.
        /// </summary>
        public static readonly Dictionary<string, Object> ConfigurationsInstalled = new Dictionary<string, Object>();

        #endregion

        #region Static methods

        /// <summary>
        ///     Adds a configuration object to the list of installed configurations.
        /// </summary>
        /// <param name="sObject">The configuration object to add.</param>
        public static void AddConfiguration(IGDFConfiguration configuration)
        {
            if (ConfigurationsInstalled.ContainsKey(configuration.GetType().Name) == false)
            {
                ConfigurationsInstalled.Add(configuration.GetType().Name, configuration);
            }

            if (ConfigurationList.Contains(configuration) == false)
            {
                ConfigurationList.Add(configuration);
            }
        }

        public static Dictionary<string, string> GetConfigurations()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (KeyValuePair<string, Object> configKeyValue in ConfigurationsInstalled)
            {
                result.TryAdd(configKeyValue.Key, JsonConvert.SerializeObject(configKeyValue.Value));
            }

            return result;
        }


        public static void WriteOptimizedConfigInConsole(bool fileByFile = false)
        {
            string env = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (!string.IsNullOrEmpty(env))
            {
                env = env + ".";
            }
            else
            {
                env = "";
            }

            if (fileByFile == true)
            {
                foreach (KeyValuePair<string, Object> configKeyValue in ConfigurationsInstalled)
                {
                    List<string> additionalFile = new List<string>();
                    additionalFile.Add($"{{");
                    additionalFile.Add($"\"{configKeyValue.Key}\" : ");
                    additionalFile.AddRange(GDFLogger.SplitObjectSerializable(configKeyValue.Value));
                    additionalFile.Add($"}}");
                    GDFLogger.Information($"{configKeyValue.Key}.{env}json example", additionalFile.ToArray());
                }
            }
            else
            {
                List<string> appsettings = new List<string>();
                appsettings.Add($"{{");
                appsettings.Add($"…");
                foreach (KeyValuePair<string, Object> configKeyValue in ConfigurationsInstalled)
                {
                    appsettings.Add($"\"{configKeyValue.Key}\" : ");
                    appsettings.AddRange(GDFLogger.SplitObjectSerializable(configKeyValue.Value));
                    appsettings[appsettings.Count - 1] = appsettings[appsettings.Count - 1] + ",";
                }

                appsettings.Add($"…");
                appsettings.Add($"}}");
                GDFLogger.Information($"appsettings.{env}json example", appsettings.ToArray());
            }
        }

        #endregion
    }
}