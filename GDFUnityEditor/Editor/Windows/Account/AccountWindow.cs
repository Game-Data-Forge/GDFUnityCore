using GDFFoundation;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class AccountWindow : Window
    {
        internal const string HELP_URL = "/unity/editor-windows/account/overview"; 
        
        [MenuItem("GDF/Account/Account...", priority = 100, secondaryPriority = 3)]
        static public void Display()
        {
            AccountWindow window = GetWindow<AccountWindow>();
            window.titleContent = window.Title;
            window.Focus();
        }

        internal AccountMenu menu;

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            GDFEditor.Account.AccountChanged.onMainThread -= OnAccountChanged;
        }

        protected override void GUIReady()
        {
            TwoPaneSplitView main = new TwoPaneSplitView(0, 150, TwoPaneSplitViewOrientation.Horizontal);
            menu = new AccountMenu(this);
            AccountView view = new AccountView(this);

            menu.BuildViews(this, view);

            main.Add(menu);
            main.Add(view);

            HelpButton help = new HelpButton(HELP_URL, Position.Absolute);

            RegisterHelp(help);

            MainView.AddBody(main);
            MainView.AddBody(help);

            GDFEditor.Account.AccountChanged.onMainThread -= OnAccountChanged;
            GDFEditor.Account.AccountChanged.onMainThread += OnAccountChanged;

            OnAccountChanged(GDFEditor.Account.Token);
        }

        protected override GUIContent GenerateTitle()
        {
            return new GUIContent("Account", DefaultIcon);
        }

        protected override LoadingView BuildLoadingView()
        {
            VisualElement mainViewBody = new VisualElement();
            mainViewBody.style.flexGrow = 1;
            return new LoadingView(mainViewBody);
        }

        private void OnAccountChanged(MemoryJwtToken token)
        {
            if (token == null)
            {
                menu.AuthenticationDisplay();
            }
            else
            {
                menu.AccountDisplay();
            }
        }
    }
}
