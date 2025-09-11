using System.Collections.Generic;
using GDFFoundation;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace GDFUnity.Editor
{
    public class CredentialsView : IWindowView<AccountWindow>
    {
        private abstract class State
        {
            public abstract string Title { get; }
            public abstract string Help { get; }

            public abstract void OnActivate(CredentialsView view);

            public virtual void OnDeactivate(CredentialsView view)
            {

            }
        }

        private class MainState : State
        {
            private class Credentials : VisualElement, IPoolItem
            {
                public Pool Pool { get; set; }

                protected HeaderFoldout _foldout;
                private LabelField _hash;

                public Credentials()
                {
                    style.flexGrow = 1;
                    _hash = new LabelField();
                    _hash.text = "UID";

                    _foldout = new HeaderFoldout();
                    _foldout.hierarchy[1].style.marginLeft = 9;
                    _foldout.hierarchy[3].style.marginLeft = 25;
                    _foldout.value = false;

                    _foldout.Add(_hash);

                    Add(_foldout);
                }

                public void SetSign(CredentialsView view, GDFAccountSign sign)
                {
                    _foldout.text = sign.SignType.ToString();
                    _hash.value = sign.SignHash;

                    if (sign.SignType != GDFAccountSignType.EmailPassword)
                    {
                        _foldout.UnregisterActionMenu();
                        return;
                    }

                    _foldout.RegisterActionMenu(() =>
                    {
                        GenericMenu menu = new GenericMenu();
                        menu.AddItem(new GUIContent("Edit password..."), false, () =>
                        {
                            view._passwordEdit.SetSign(sign);
                            view.SetState(view._passwordEdit);
                        });
                        menu.ShowAsContext();
                    });
                }

                public void Dispose()
                {
                    PoolItem.Release(this);
                }

                public void OnPooled()
                {

                }

                public void OnReleased()
                {

                }

                public void ProcessSearch(string[] parts)
                {
                    style.display = DisplayStyle.Flex;

                    for (int i = parts.Length - 1; i >= 0; i--)
                    {
                        if (_foldout.text.IndexOf(parts[i], System.StringComparison.InvariantCultureIgnoreCase) < 0)
                        {
                            style.display = DisplayStyle.None;
                            return;
                        }
                    }
                }
            }

            static private Pool<Credentials> _pool = new Pool<Credentials>();

            public override string Title => null;
            public override string Help => null;

            private Toolbar _toolbar;
            private ScrollView _scrollView;
            private ToolbarSearchField _search;

            public MainState(CredentialsView view)
            {
                _toolbar = new Toolbar();

                ToolbarButton refresh = new ToolbarButton();
                refresh.style.IconContent("Refresh");
                refresh.style.minWidth = 26;
                refresh.tooltip = "Refresh";
                refresh.clicked += () => Refresh(view);
                _toolbar.Add(refresh);

                _search = new ToolbarSearchField();
                _search.style.flexGrow = 1;
                _search.style.flexShrink = 1;
                _toolbar.Add(_search);
                _search.onChanged += Search;

                HelpButton help = new HelpButton(Position.Relative);
                help.url = "/unity/editor-windows/account/account/credentials/overview";
                _toolbar.Add(help);

                _scrollView = new ScrollView(ScrollViewMode.Vertical);
            }

            public override void OnActivate(CredentialsView view)
            {
                view._body.style.marginLeft = -23;
                view._body.style.marginRight = -23;

                view._body.Add(_toolbar);
                view._body.Add(_scrollView);

                Refresh(view);
            }

            private void Refresh(CredentialsView view)
            {
                foreach (VisualElement element in _scrollView.Children())
                {
                    Credentials credentials = element as Credentials;
                    if (credentials == null) continue;

                    credentials.Dispose();
                }

                _scrollView.Clear();

                view._window.MainView.AddCriticalLoader(GDF.Account.Credentials.Refresh(), job =>
                {
                    List<GDFAccountSign> signs = GDF.Account.Credentials.Credentials;

                    foreach (GDFAccountSign sign in signs)
                    {
                        Credentials credentials = _pool.Get();
                        credentials.SetSign(view, sign);
                        _scrollView.Add(credentials);
                    }

                    Search();
                });
            }

            private void Search()
            {
                string[] parts = _search.value?.Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
                foreach (VisualElement element in _scrollView.Children())
                {
                    Credentials credentials = element as Credentials;
                    if (credentials == null) continue;

                    credentials.ProcessSearch(parts);
                }
            }
        }

        private class PasswordEditState : State
        {
            public override string Title => "Account credentails: Edit password";
            public override string Help => "/unity/editor-windows/account/account/credentials/email-password#password-edition-view";

            private CredentialsView _view;
            private TextField _email;

            private TextField _password;
            private TextField _newPassword;
            private TextField _confirmPassword;

            private ButtonList _buttons;
            private Button _validate;
            
            private GDFAccountSign _sign;

            public PasswordEditState(CredentialsView view)
            {
                _view = view;

                _email = new TextField("Email");
                _email.RegisterValueChangedCallback(_ => VaidateForm());

                _password = new TextField("Current password");
                _password.style.marginBottom = 5;
                _password.isPasswordField = true;
                _password.RegisterValueChangedCallback(_ => VaidateForm());

                _newPassword = new TextField("New password");
                _newPassword.isPasswordField = true;
                _newPassword.RegisterValueChangedCallback(_ => VaidateForm());

                _confirmPassword = new TextField("Confirm password");
                _confirmPassword.isPasswordField = true;
                _confirmPassword.RegisterValueChangedCallback(_ => VaidateForm());

                _buttons = new ButtonList();
                _buttons.style.flexDirection = FlexDirection.Row;
                _buttons.style.paddingTop = 20;
                _buttons.style.paddingBottom = 20;
                _buttons.style.flexShrink = 0;

                Button cancel = new Button();
                cancel.text = "Cancel";
                cancel.style.width = 100;
                cancel.clicked += () =>
                {
                    view.SetState(view._main);
                };

                _validate = new Button();
                _validate.text = "Edit password";
                _validate.style.width = 150;
                _validate.clicked += EditPassword;

                _buttons.Add(cancel);
                _buttons.Add(_validate);
            }

            public override void OnActivate(CredentialsView view)
            {
                view._body.style.marginLeft = 0;
                view._body.style.marginRight = 0;
                
                _email.value = "";
                _password.value = "";
                _newPassword.value = "";
                _confirmPassword.value = "";

                view._body.Add(_email);
                view._body.Add(_password);
                view._body.Add(_newPassword);
                view._body.Add(_confirmPassword);
                view._body.Add(_buttons);

                VaidateForm();
            }

            public void SetSign(GDFAccountSign sign)
            {
                _sign = sign;
            }

            private void VaidateForm()
            {
                try
                {
                    FieldEmailExtensions.CheckEmailValidity(_email.value);
                    FieldPasswordExtensions.CheckPasswordValidity(_newPassword.value);
                    FieldPasswordExtensions.CheckPasswordValidity(_confirmPassword.value);

                    _validate.SetEnabled(_newPassword.value == _confirmPassword.value);
                }
                catch
                {
                    _validate.SetEnabled(false);
                }
            }

            private void EditPassword()
            {
                _view._window.MainView.AddCriticalLoader(
                    GDF.Account.Credentials.EmailPassword.EditPassword(GDF.Account.Country, _sign.Reference, _email.value, _password.value, _newPassword.value), job =>
                    {
                        if (job.State != JobState.Success) return;
                        
                        _view.SetState(_view._main);
                    });
            }
        }

        public string Name => "Credentials";
        public string Title => _current?.Title;
        public string Help => _current?.Help;

        private WindowView<AccountWindow> _view;
        private AccountWindow _window;
        private VisualElement _body;

        private State _current;
        private MainState _main;
        private PasswordEditState _passwordEdit;

        public CredentialsView(AccountWindow window)
        {
            _window = window;

            _body = new VisualElement();
            _body.style.flexGrow = 1;

            _main = new MainState(this);
            _passwordEdit = new PasswordEditState(this);
        }

        public void OnActivate(AccountWindow window, WindowView<AccountWindow> view)
        {
            _view = view;

            SetState(_main);

            view.Add(_body);
        }

        public void OnDeactivate(AccountWindow window, WindowView<AccountWindow> view)
        {
            _current?.OnDeactivate(this);
        }

        private void SetState(State current)
        {
            _body.Clear();

            _current?.OnDeactivate(this);
            _current = current;

            _view.Text = Title;
            _window.helpUrl = Help;

            _current?.OnActivate(this);
        }
    }
}
