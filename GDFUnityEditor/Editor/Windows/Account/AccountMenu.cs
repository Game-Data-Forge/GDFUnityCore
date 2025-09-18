using System;
using System.Collections.Generic;
using GDFFoundation;
using GDFUnity.Editor.ServiceProviders;
using UnityEditor;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class AccountMenu : ListView
    {
        private class Element : VisualElement, IPoolItem
        {
            private TextElement _label;

            public string text
            {
                get => _label.text;
                set => _label.text = value;
            }

            public Element() : base()
            {
                style.paddingLeft = EditorGUIUtility.singleLineHeight;
                style.whiteSpace = WhiteSpace.NoWrap;

                _label = new TextElement();
                _label.style.marginTop = 1;
                _label.style.flexWrap = Wrap.NoWrap;

                Add(_label);
            }

            public Pool Pool { get; set; }

            public void Dispose() { }

            public void OnPooled() { }

            public void OnReleased()
            {
                PoolItem.Release(this);
            }
        }

        public event Action authenticationDisplayed;
        public event Action accountDisplayed;

        private List<IWindowView<AccountWindow>> _authenticationViews = null;
        private List<IWindowView<AccountWindow>> _accountViews = null;
        private List<IWindowView<AccountWindow>> _views = null;

        public AccountMenu(AccountWindow window)
        {
            Pool<Element> pool = new Pool<Element>();

            style.flexGrow = 1;
            horizontalScrollingEnabled = false;
            style.flexBasis = new StyleLength(StyleKeyword.Auto);
            fixedItemHeight = EditorGUIUtility.singleLineHeight;
            makeItem = () => pool.Get();
            bindItem = (ve, i) =>
            {
                Element label = ve as Element;
                label.text = _views[i].Name;
            };
            destroyItem = (ve) => (ve as Element).Dispose();
        }

        public void BuildViews(AccountWindow window, AccountView view)
        {
            _accountViews = new List<IWindowView<AccountWindow>>
            {
                new InformationView(),
                new LicenseView(window),
                new CredentialsView(window),
                new ManagementView(window)
            };

            _authenticationViews = new List<IWindowView<AccountWindow>>
            {
                new LocalView(view),
                new DeviceView(view),
                new EmailPasswordView(view),
                new LastSessionView(view)
            };
        }

        public void AuthenticationDisplay()
        {
            SetDisplay(_authenticationViews);
            authenticationDisplayed?.Invoke();
        }

        public void AccountDisplay()
        {
            SetDisplay(_accountViews);
            accountDisplayed?.Invoke();
        }

        private void SetDisplay(List<IWindowView<AccountWindow>> views)
        {
            _views = views;
            itemsSource = _views;
            selectedIndex = 0;
        }
    }
}
