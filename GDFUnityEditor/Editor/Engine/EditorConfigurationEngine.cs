using System;
using System.Collections.Generic;
using System.IO;
using GDFEditor;
using GDFFoundation;
using GDFRuntime;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace GDFUnity.Editor
{
    public class EditorConfigurationEngine : RuntimeConfigurationEngine, IEditorConfigurationEngine
    {
        private const string _CFG_FILE_NAME = "GDFEditorConfiguration.json";
        public const string DASHBOARD_ADDRESS = "https://www.game-data-forge.com/";

        static public class EditorExceptions
        {
            static public GDFException InvalidDashboard => new GDFException("CFG", 10, $"Configuration does not have a valid {nameof(IEditorConfiguration.Dashboard)} !");
            static public GDFException InvalidChannels => new GDFException("CFG", 11, $"Configuration does not have valid {nameof(IEditorConfiguration.Channels)} !");
            static public GDFException InvalidCredentials => new GDFException("CFG", 12, $"Configuration does not have valid {nameof(IEditorConfiguration.Credentials)} !");
        }

        static private IEditorConfigurationEngine _instance = null;
        static public new IEditorConfigurationEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EditorConfigurationEngine();
                }

                return _instance;
            }
        }

        static public event Action<bool> onConfigurationChange;

        private SettingsManager _manager = new SettingsManager();

        public List<GDFException> ValidationReport(IEditorConfiguration configuration)
        {
            List<GDFException> result = new List<GDFException>();

            DefaultValidation(result, configuration);

            CredentialsValidation(result, configuration);

            return result;
        }

        protected void DefaultValidation(List<GDFException> result, IEditorConfiguration configuration)
        {
            base.DefaultValidation(result, configuration);

            if (string.IsNullOrWhiteSpace(configuration.Dashboard))
            {
                result.Add(EditorExceptions.InvalidDashboard);
            }

            if (configuration.Channels.Count == 0)
            {
                result.Add(EditorExceptions.InvalidChannels);
            }
        }

        private void CredentialsValidation(List<GDFException> result, IEditorConfiguration configuration)
        {
            if (configuration.Credentials.Count != Enum.GetValues(typeof(ProjectEnvironment)).Length)
            {
                result.Add(EditorExceptions.InvalidCredentials);
            }
        }

        public void Validate(IEditorConfiguration configuration)
        {
            List<GDFException> result = ValidationReport(configuration);
            if (result.Count > 0)
            {
                throw result[0];
            }
        }

        IEditorConfiguration IEditorConfigurationEngine.Load()
        {
            IEditorConfiguration configuration = LoadWithoutValidation();
            Validate(configuration);
            return configuration;
        }

        public IEditorConfiguration LoadWithoutValidation()
        {
            if (!ProjectSettings.Instance.Exists<EditorConfiguration>(_CFG_FILE_NAME))
            {
                throw RuntimeExceptions.NotFound;
            }

            try
            {
                return ProjectSettings.Instance.Load<EditorConfiguration>(_CFG_FILE_NAME);
            }
            catch
            {
                throw RuntimeExceptions.NotCastable;
            }
        }

        public bool IsValidDashboardAddress(string address)
        {
            return _manager.IsValidAddress(address);
        }

        public IJob<DateTime> ContactDashboard(string dashboardAddress)
        {
            GDFManagers.Start();

            GDFManagers.AddSingleton<IEditorConfigurationEngine>(this);

            return GDFManagers.Lock(typeof(IEditorConfigurationEngine), () => _manager.ContactDashboard(dashboardAddress));
        }

        public IJob<GDFProjectConfiguration> RequestConfigurationUpdate(string dashboardAddress, string role)
        {
            return GDFManagers.Lock(typeof(IEditorConfigurationEngine), () => _manager.RequestConfigurationUpdate(dashboardAddress, role));
        }

        public IRuntimeConfiguration CreateRuntimeConfiguration()
        {
            IEditorConfiguration configuration = GDFEditor.Configuration;
            return new RuntimeConfiguration
            {
                Reference = configuration.Reference,
                Name = configuration.Name,
                Organization = configuration.Organization,
                Environment = GDFEditor.Environment.Environment,
                PublicToken = configuration.Credentials[GDFEditor.Environment.Environment].PublicKey,
                Channel = configuration.Channel,
                CloudConfig = configuration.CloudConfig
            };
        }

        public void Save(IEditorConfiguration configuration)
        {
            try
            {
                if (configuration == null)
                {
                    throw new ArgumentNullException(nameof(configuration));
                }

                ProjectSettings.Instance.Save(configuration, _CFG_FILE_NAME);
                onConfigurationChange?.Invoke(true);
            }
            catch (Exception e)
            {
                ProjectSettings.Instance.Delete(typeof(EditorConfiguration), _CFG_FILE_NAME);
                if (e is not ArgumentNullException)
                {
                    GDFLogger.Error(e);
                }
                onConfigurationChange?.Invoke(false);
            }
        }

        public void Save(IRuntimeConfiguration configuration)
        {
            try
            {
                Validate(configuration);
            }
            catch (Exception e)
            {
                GDFLogger.Warning("Invalid GDF configuration!\nYou wont be able to use GDF at runtime...\nError:" + e.Message);
                return;
            }

            string fullPath = Path.Combine(Application.dataPath, "../", _PATH);
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            AssetDatabase.DeleteAsset(_PATH);
            string json = JsonConvert.SerializeObject(configuration);
            TextAsset asset = new TextAsset(json);
            AssetDatabase.CreateAsset(asset, _PATH);
            AssetDatabase.SaveAssets();
        }
    }
}
