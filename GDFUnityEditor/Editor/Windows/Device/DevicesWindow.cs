using UnityEditor;
using UnityEngine;

namespace GDFUnity.Editor
{
    public class DevicesWindow : Window
    {
        [MenuItem("GDF/Account/Devices...", priority = 100, secondaryPriority = 0)]
        static public void Display()
        {
            DevicesWindow window = GetWindow<DevicesWindow>();
            window.titleContent = window.Title;
            window.Focus();
        }

        private DevicesView deviceView;

        protected override bool DisableInPlayMode => true;

        protected override void GUIReady()
        {
            DevicesToolbar toolbar = new DevicesToolbar();
            deviceView = new DevicesView(toolbar);

            MainView.AddBody(toolbar);
            MainView.AddBody(deviceView);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            deviceView.OnDestroy();
        }
        
        protected override GUIContent GenerateTitle()
        {
            return new GUIContent("Devices", DefaultIcon);
        }
    }
}
