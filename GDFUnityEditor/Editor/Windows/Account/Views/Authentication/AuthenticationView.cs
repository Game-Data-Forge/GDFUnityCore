using GDFFoundation;
using UnityEngine.UIElements;

namespace GDFUnity.Editor.ServiceProviders
{
    public abstract class AuthenticationView
    {
        protected AccountView _view;

        public virtual bool NeedCountry => true;
        public virtual bool NeedConsent => true;

        public AuthenticationView(AccountView view)
        {
            _view = view;
        }

        public void OnActivate(AccountWindow window, WindowView<AccountWindow> view)
        {
            if (view != _view) return;

            _view.Add(_view.environment);
            
            _view.Add(_view.country);

            OnActivate(window, _view);

            _view.Add(_view.consent);

            Update();
        }
        public void OnDeactivate(AccountWindow window, WindowView<AccountWindow> view)
        {
            if (view != _view) return;

            OnDeactivate(window, _view);
        }

        protected abstract void OnActivate(AccountWindow window, AccountView view);
        protected abstract void OnDeactivate(AccountWindow window, AccountView view);

        public void Load(IJob job)
        {
            _view.Load(job, null);
        }

        public void Update()
        {
            _view.country.style.display = NeedCountry ? DisplayStyle.Flex : DisplayStyle.None;
            _view.consent.style.display = NeedConsent ? DisplayStyle.Flex : DisplayStyle.None;

            if (NeedConsent)
            {
                _view.consent.Refresh();
            }
            
            _view.Update();
        }
    }
}
