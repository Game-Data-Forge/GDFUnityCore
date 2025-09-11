using System;
using GDFEditor;
using GDFFoundation;
using GDFUnityEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class GlobalSettingsProvider : SettingsProvider
    {
        internal enum State
        {
            NoAddress = 0,
            CheckingAddress = 1,
            NoRole = 2,
            CheckingRole = 3,
            NoChannel = 4,
            Ready = 5
        }

        internal class RoleConfiguration
        {
            public string Token { get; set; }
        }

        public const string PATH = "Project/Game Data Forge";
        static private readonly string[] _KEYWORDS = new string[] { "Game", "Data", "Forge", "GDF" };

        static private GlobalSettingsProvider _instance = null;
        
        [MenuItem("GDF/Project Settings...", priority = 0)]
        static public void Display()
        {
            SettingsService.OpenProjectSettings(PATH);
        }
        [SettingsProvider]
        static public SettingsProvider Generate()
        {
            if (_instance == null)
            {
                _instance = new GlobalSettingsProvider();
            }
            return _instance;
        }

        internal LoadingView mainView;
        internal string dashboardAddress;
        internal RoleConfiguration role = new RoleConfiguration();

        internal event Action<State> onStateChanged;
        internal event Action<IEditorConfiguration> onConfigurationChanged;
        internal event Action onConfigurationUpdateRequest;

        private State _state = State.NoAddress;
        private IEditorConfiguration _configuration;
        private RoleToken _roleToken;

        internal IEditorConfiguration Configuration
        {
            get => _configuration;
            set
            {
                long reference = _configuration?.Reference ?? 0;
                _configuration = value;
                UpdatedConfiguration(reference);
            }
        }
        internal State ProviderState
        {
            get => _state;
            set
            {
                _state = value;
                onStateChanged?.Invoke(_state);
            }
        }

        private GlobalSettingsProvider() : base(PATH, SettingsScope.Project, _KEYWORDS)
        {
            GDFLogger.SetWriter(new GDFLoggerUnityEditor(GDFLogLevel.Trace));

            try
            {
                _configuration = EditorConfigurationEngine.Instance.LoadWithoutValidation();
            }
            catch { }

            if (string.IsNullOrWhiteSpace(_configuration?.Dashboard))
            {
                dashboardAddress = EditorConfigurationEngine.DASHBOARD_ADDRESS;
                _state = State.NoAddress;
                return;
            }
            
            dashboardAddress = _configuration.Dashboard;
            
            role = GDFUserSettings.Instance.LoadOrDefault(new RoleConfiguration());
            if (!string.IsNullOrWhiteSpace(role.Token))
            {
                _state = State.NoRole;
                return;
            }

            if (_configuration.Channel == 0)
            {
                _state = State.NoChannel;
                return;
            }
            
            _state = State.Ready;
            EditorApplication.playModeStateChanged += OnDomainChange;
        }

        ~GlobalSettingsProvider()
        {
            EditorApplication.playModeStateChanged -= OnDomainChange;
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            rootElement.Add(new TitleLabel("Game Data Forge"));

            mainView = new LoadingView(new ScrollView(ScrollViewMode.Vertical));

            mainView.AddBody(new CategoryLabel("Dashboard connection"));
            mainView.AddBody(new ServerAddress(this));
            _roleToken = new RoleToken(this);
            mainView.AddBody(_roleToken);
            mainView.AddBody(new Channel(this));

            mainView.AddBody(new ProjectInformation(this));

            rootElement.Add(mainView);
            rootElement.Add(new HelpButton("/unity/configuration/overview", Position.Absolute));

            if (Application.isPlaying)
            {
                OnDomainChange(PlayModeStateChange.EnteredPlayMode);
            }
        }

        public void RequestConfigurationUpdate()
        {
            onConfigurationUpdateRequest?.Invoke();
        }

        public void UpdatedConfiguration(long oldReference)
        {
            onConfigurationChanged?.Invoke(_configuration);
            EditorConfigurationEngine.Instance.Save(_configuration);
            if (_configuration != null)
            {
                GDFUserSettings.Instance.Save(role);
            }
            else
            {
                EditorEngine.UnsafeInstance?.Stop();
                GDFUserSettings.Instance.Delete<RoleConfiguration>();
                if (oldReference != 0)
                {
                    GDFUserSettings.Instance.DeleteProject(oldReference);
                }
            }
        }

        private void OnDomainChange(PlayModeStateChange state)
        {
            mainView?.SetEnabled(state == PlayModeStateChange.EnteredEditMode);
        }
    }
}