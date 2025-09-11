using System;
using GDFEditor;
using GDFFoundation;
using GDFUnityEditor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class GlobalPreferencesProvider : SettingsProvider
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

        public const string PATH = "Preferences/Game Data Forge";
        static private readonly string[] _KEYWORDS = new string[] { "Game", "Data", "Forge", "GDF" };

        static private GlobalPreferencesProvider _instance = null;
        
        [MenuItem("GDF/Preferences...", priority = 1)]
        static public void Display()
        {
            SettingsService.OpenUserPreferences(PATH);
        }
        [SettingsProvider]
        static public SettingsProvider Generate()
        {
            if (_instance == null)
            {
                _instance = new GlobalPreferencesProvider();
            }
            return _instance;
        }

        internal VisualElement root;

        private GlobalPreferencesProvider() : base(PATH, SettingsScope.User, _KEYWORDS)
        {
            GDFLogger.SetWriter(new GDFLoggerUnityEditor(GDFLogLevel.Trace));
            EditorApplication.playModeStateChanged += OnDomainChange;
        }

        ~GlobalPreferencesProvider()
        {
            EditorApplication.playModeStateChanged -= OnDomainChange;
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            root = rootElement;
            root.Add(new TitleLabel("Game Data Forge"));

            root.Add(new CategoryLabel("General"));

            CountryField preferedCountry = new CountryField("Prefered country");
            preferedCountry.style.paddingLeft = 7;
            preferedCountry.UnregisterPreferences();
            preferedCountry.changed += value => Preferences.Country = value;

            root.Add(preferedCountry);
            root.Add(new HelpButton(null, Position.Absolute));

            if (Application.isPlaying)
            {
                OnDomainChange(PlayModeStateChange.EnteredPlayMode);
            }
        }

        private void OnDomainChange(PlayModeStateChange state)
        {
            root?.SetEnabled(state == PlayModeStateChange.EnteredEditMode);
        }
    }
}