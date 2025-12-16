using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class MessagesWindow : EditorWindow
    {
        internal const string HELP_URL = "/unity/editor-windows/account/overview";

        private MessagesTypeView _errors = null;
        private MessagesTypeView _warnings = null;
        private MessagesTypeView _information = null;

        public MessagesTypeView Errors
        {
            get
            {
                if (_errors == null)
                {
                    _errors = new MessagesTypeView(MessageEngine.Instance.errors, HelpBoxMessageType.Error);
                }

                return _errors;
            }
        }
        public MessagesTypeView Warnings
        {
            get
            {
                if (_warnings == null)
                {
                    _warnings = new MessagesTypeView(MessageEngine.Instance.warnings, HelpBoxMessageType.Warning);
                }

                return _warnings;
            }
        }
        public MessagesTypeView Information
        {
            get
            {
                if (_information == null)
                {
                    _information = new MessagesTypeView(MessageEngine.Instance.information, HelpBoxMessageType.Info);
                }

                return _information;
            }
        }


        [MenuItem("GDF/Messages...", priority = 1000)]
        static public void Display()
        {
            MessagesWindow window = GetWindow<MessagesWindow>();
            window.titleContent = new GUIContent("Messages", Window.DefaultIcon);
            window.Focus();
        }

        public void CreateGUI()
        {
            Toolbar toolbar = new Toolbar();
            VisualElement spacer = new VisualElement();
            spacer.style.flexGrow = 1;

            toolbar.Add(spacer);
            toolbar.Add(Information.Badge);
            toolbar.Add(Warnings.Badge);
            toolbar.Add(Errors.Badge);

            rootVisualElement.Add(toolbar);

            ScrollView scrollView = new ScrollView(ScrollViewMode.Vertical);

            scrollView.Add(Errors);
            scrollView.Add(Warnings);
            scrollView.Add(Information);

            rootVisualElement.Add(scrollView);
        }
    }
}
