using GDFFoundation;
using UnityEngine.UIElements;

namespace GDFUnity.Editor.ServiceProviders
{
    public abstract class OAuthView : AuthenticationView, IWindowView<AccountWindow>
    {
        public abstract string Name { get; }
        public abstract string Title { get; }
        public abstract string Help { get; }

        private bool _allowLogin => _view.consent.Value
            && !string.IsNullOrEmpty(_clientId.value)
            && !string.IsNullOrEmpty(_token.value);

        private TextField _clientId;
        private TextField _token;
        private Button _login;

        public OAuthView(AccountView view) : base(view)
        {
            _view = view;
            _clientId = new TextField("Client ID");
            _token = new TextField("Token");
            _login = new Button();
            _login.text = "Login";
            _login.clicked += () =>
            {
                Load(Login(view.country.value, _clientId.value, _token.value));
            };
        }

        protected abstract IJob Login(Country country, string clientId, string token);

        protected override void OnActivate(AccountWindow window, AccountView view)
        {
            _clientId.RegisterValueChangedCallback(OnStringChanged);
            _token.RegisterValueChangedCallback(OnStringChanged);
            view.consent.onChanged += OnConsentChanged;
            view.consent.Value = false;

            view.Add(_clientId);
            view.Add(_token);

            _login.style.width = 100;
            _login.SetEnabled(_allowLogin);

            view.buttons.Add(_login);
        }

        protected override void OnDeactivate(AccountWindow window, AccountView view)
        {
            view.consent.onChanged -= OnConsentChanged;
            _token.UnregisterValueChangedCallback(OnStringChanged);
            _clientId.UnregisterValueChangedCallback(OnStringChanged);
        }

        private void OnStringChanged(ChangeEvent<string> value)
        {
            _login.SetEnabled(_allowLogin);
        }

        private void OnConsentChanged(bool value)
        {
            _login.SetEnabled(_allowLogin);
        }
    }
}
