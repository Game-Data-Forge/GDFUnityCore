using System.Collections.Generic;
using GDFUnity.Editor.ServiceProviders;
using UnityEditor;

namespace GDFUnity.Editor
{
    public class DevicesWindow : EditorWindow
    {
        [MenuItem("GDF/Account/Devices...", priority = 11, secondaryPriority = 0)]
        static public void Display()
        {
            DevicesWindow window = GetWindow<DevicesWindow>("Devices");
            window.Focus();
        }

        internal List<AuthenticationSettingsProvider> providers = new List<AuthenticationSettingsProvider>();
        internal LoadingView mainView;
        private DevicesView deviceView;

        public void CreateGUI()
        {
            providers = AuthenticationSelection.Providers;

            mainView = new LoadingView();
            
            DevicesToolbar toolbar = new DevicesToolbar();
            deviceView = new DevicesView(this, toolbar);

            mainView.AddBody(toolbar);
            mainView.AddBody(deviceView);

            mainView.AddPreloader(new EnginePreLoader());
            rootVisualElement.Add(mainView);
        }
        
        public void OnDestroy()
        {
            deviceView.OnDestroy();
        }
    }
}
