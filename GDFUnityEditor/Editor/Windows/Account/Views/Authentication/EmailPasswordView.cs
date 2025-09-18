using UnityEngine.UIElements;

namespace GDFUnity.Editor.ServiceProviders
{
    public class EmailPasswordView : AuthenticationView, IWindowView<AccountWindow>
    {
        private class LoginState : AuthenticationViewState
        {
            public override string Name => "Login";
            public override string Help => "/unity/editor-windows/account/authentication/email-password#the-login-page";

            private EmailPasswordView _view;

            private TextField _email;
            private TextField _password;
            private Button _registerPage;
            private Button _rescuePage;
            private Button _login;

            public LoginState(EmailPasswordView view)
            {
                _view = view;

                _email = new TextField("Email");
                _email.style.marginBottom = 5;
                _email.keyboardType = UnityEngine.TouchScreenKeyboardType.EmailAddress;
                _email.RegisterCallback<NavigationSubmitEvent>((_) =>
                {
                    Login(view);
                });
                _email.RegisterValueChangedCallback(e => {
                    view._email = e.newValue;
                });

                _password = new TextField("Password");
                _password.isPasswordField = true;
                _password.RegisterCallback<NavigationSubmitEvent>((_) =>
                {
                    Login(view);
                });

                _rescuePage = new Button();
                _rescuePage.text = "Rescue page";
                _rescuePage.style.width = 100;
                _rescuePage.clicked += () =>
                {
                    view.SetState(view._rescue);
                };

                _registerPage = new Button();
                _registerPage.text = "Register page";
                _registerPage.style.width = 100;
                _registerPage.clicked += () =>
                {
                    view.SetState(view._register);
                };

                _login = new Button();
                _login.text = "Login";
                _login.style.width = 100;
                _login.clicked += () =>
                {
                    Login(view);
                };
            }

            public override void OnActivate(AccountWindow window, AccountView view)
            {
                _email.value = _view._email;

                _view.body.Add(_email);
                _view.body.Add(_password);

                view.buttons.Add(_registerPage);
                view.buttons.Add(_rescuePage);
                view.buttons.Add(_login);
            }

            public override void OnDeactivate(AccountWindow window, AccountView view) { }

            private void Login(AuthenticationView view)
            {
                view.Load(GDFEditor.Account.Authentication.EmailPassword.Login(_view._view.country.value, _email.value, _password.value));
            }
        }
        private class RescueState : AuthenticationViewState
        {
            public override string Name => "Rescue";
            public override string Help => "/unity/editor-windows/account/authentication/email-password#the-lost-password-page";

            private EmailPasswordView _view;

            private TextField _email;
            private Button _loginPage;
            private Button _rescue;

            public RescueState(EmailPasswordView view)
            {
                _view = view;
                
                _email = new TextField("Email");
                _email.style.marginBottom = 5;
                _email.keyboardType = UnityEngine.TouchScreenKeyboardType.EmailAddress;
                _email.RegisterCallback<NavigationSubmitEvent>(_ => {
                    Rescue(view);
                });
                _email.RegisterValueChangedCallback(e => {
                    _view._email = e.newValue;
                });

                _loginPage = new Button();
                _loginPage.text = "Login page";
                _loginPage.style.width = 100;
                _loginPage.clicked += () => {
                    _view.SetState(_view._login);
                };

                _rescue = new Button();
                _rescue.text = "Rescue";
                _rescue.style.width = 100;
                _rescue.clicked += () => {
                    Rescue(view);
                };
            }

            public override void OnActivate(AccountWindow window, AccountView view)
            {
                _email.value = _view._email;

                _view.body.Add(_email);

                view.buttons.Add(_loginPage);
                view.buttons.Add(_rescue);
            }

            public override void OnDeactivate(AccountWindow window, AccountView view)
            {
                
            }

            private void Rescue(AuthenticationView view)
            {
                view.Load(GDFEditor.Account.Authentication.EmailPassword.Rescue(_view._view.country.value, _email.value));
            }
        }
        private class RegisterState : AuthenticationViewState
        {
            public override string Name => "Register";
            public override string Help => "/unity/editor-windows/account/authentication/email-password#the-register-page";

            private EmailPasswordView _view;

            private TextField _email;
            private Button _loginPage;
            private Button _register;
            private HelpBox _message;

            public RegisterState(EmailPasswordView view)
            {
                _view = view;

                _email = new TextField("Email");
                _email.style.marginBottom = 5;
                _email.keyboardType = UnityEngine.TouchScreenKeyboardType.EmailAddress;
                _email.RegisterCallback<NavigationSubmitEvent>((_) =>
                {
                    Register(view);
                });
                _email.RegisterValueChangedCallback(e => {
                    _view._email = e.newValue;
                });

                _loginPage = new Button();
                _loginPage.text = "Login page";
                _loginPage.style.width = 100;
                _loginPage.clicked += () =>
                {
                    _view.SetState(_view._login);
                };

                _register = new Button();
                _register.text = "Register";
                _register.style.width = 100;
                _register.clicked += () =>
                {
                    Register(view);
                };

                _message = new HelpBox("Enter a valid email address.\nThe account password will be sent to it on register.", HelpBoxMessageType.Info);
                _message.style.marginBottom = 5;
            }

            public override void OnActivate(AccountWindow window, AccountView view)
            {
                _email.value = _view._email;

                _view.body.Add(_message);
                _view.body.Add(_email);
                
                view.buttons.Add(_loginPage);
                view.buttons.Add(_register);

                view.consent.onChanged += OnConsentChanged;
                view.consent.Value = false;
                
                _register.SetEnabled(view.consent.Value);
            }

            public override void OnDeactivate(AccountWindow window, AccountView view)
            {
                view.consent.onChanged -= OnConsentChanged;
            }

            private void Register(AuthenticationView view)
            {
                view.Load(GDFEditor.Account.Authentication.EmailPassword.Register(_view._view.country.value, _email.value, true));
            }

            private void OnConsentChanged(bool value)
            {
                _register.SetEnabled(value);
            }
        }

        public string Name => "Email password";
        public string Title => $"{Name} authentication: {_current?.Name}";
        public string Help => _current?.Help;
        public override bool NeedConsent => _needConsent;

        internal VisualElement body;

        private AuthenticationViewState _current;
        private RegisterState _register;
        private RescueState _rescue;
        private LoginState _login;
        private string _email;
        private bool _needConsent = false;

        public EmailPasswordView(AccountView view) : base(view)
        {
            body = new VisualElement();

            _register = new RegisterState(this);
            _rescue = new RescueState(this);
            _login = new LoginState(this);
        }

        protected override void OnActivate(AccountWindow window, AccountView view)
        {
            view.Add(body);

            SetState(_login);
        }

        protected override void OnDeactivate(AccountWindow window, AccountView view)
        {
            _current?.OnDeactivate(window, view);
        }

        private void SetState(AuthenticationViewState current)
        {
            body.Clear();
            _view.buttons.Clear();

            _current?.OnDeactivate(_view.Window, _view);
            _current = current;

            _needConsent = current is RegisterState;

            Update();

            _current?.OnActivate(_view.Window, _view);
        }
    }
}
