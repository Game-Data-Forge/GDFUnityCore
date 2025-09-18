using UnityEditor;
using UnityEngine;

namespace GDFUnity.Editor
{
    public class MessagesWindow : EditorWindow
    {
        internal const string HELP_URL = "/unity/editor-windows/account/overview";

        //[MenuItem("GDF/Messages...", priority = 1000)]
        static public void Display()
        {
            MessagesWindow window = GetWindow<MessagesWindow>();
            window.titleContent = new GUIContent("Messages", Window.DefaultIcon);
            window.Focus();
        }
        
    }
}
