using System;
using GDFFoundation;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class AccountView : WindowView<AccountWindow>
    {
        internal ButtonList buttons;
        internal Environment environment;
        internal CountryField country;
        internal ConsentField consent;

        public AccountWindow Window => _window;

        public AccountView(AccountWindow window) : base(window)
        {
            AccountMenu menu = _window.menu;
            menu.selectionChanged += selection =>
            {
                Current = menu.selectedItem as IWindowView<AccountWindow>;
            };
            menu.itemsSourceChanged += () =>
            {
                Current = menu.selectedItem as IWindowView<AccountWindow>;
            };
            menu.accountDisplayed += () =>
            {
                buttons.style.display = DisplayStyle.None;
            };
            menu.authenticationDisplayed += () =>
            {
                buttons.style.display = DisplayStyle.Flex;
            };

            buttons = new ButtonList();
            buttons.style.flexDirection = FlexDirection.Row;
            buttons.style.paddingTop = 20;
            buttons.style.paddingBottom = 20;
            buttons.style.flexShrink = 0;

            environment = new Environment(window.MainView);
            environment.style.marginBottom = 20;
            environment.style.marginLeft = 0;

            country = new CountryField();
            country.style.marginBottom = 10;

            consent = new ConsentField();
            consent.style.marginTop = 40;

            AddBody(buttons);
        }

        protected override void SetView(IWindowView<AccountWindow> view)
        {
            buttons.Clear();
            base.SetView(view);
        }

        internal void Load(IJob load, Action<IJob> onDone = null)
        {
            _window.MainView.AddCriticalLoader(load, onDone);
        }

        internal void Update()
        {
            Text = _current.Title;
            _window.helpUrl = _current.Help;
        }
    }
}
