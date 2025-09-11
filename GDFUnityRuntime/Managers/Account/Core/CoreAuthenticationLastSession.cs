using System;
using GDFFoundation;
using GDFRuntime;
using static GDFUnity.CoreAccountManager;

namespace GDFUnity
{
    public abstract class CoreAuthenticationLastSession : IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeLastSession
    {
        public abstract bool IsAvailable { get; }

        public abstract Job Login();
    }

    public class CoreAuthenticationLastSession<T> : CoreAuthenticationLastSession where T : IRuntimeEngine
    {
        protected T _engine;
        protected TokenStorage _storage;
        private CoreAccountManager _manager;

        public override bool IsAvailable
        {
            get
            {
                LoadStorage();
                return _storage != null;
            }
        }

        public CoreAuthenticationLastSession(T engine, CoreAccountManager manager)
        {
            _engine = engine;
            _manager = manager;

            _manager.AccountChanging.onBackgroundThread += OnAccountChanging;
            _manager.AccountChanged.onBackgroundThread += OnAccountChanged;
        }

        ~CoreAuthenticationLastSession()
        {
            _manager.AccountChanging.onBackgroundThread -= OnAccountChanging;
            _manager.AccountChanged.onBackgroundThread -= OnAccountChanged;
        }

        public override Job Login()
        {
            return _manager.JobLocker(AutoSignInJob);
        }

        private void OnAccountChanging(IJobHandler handler, MemoryJwtToken value)
        {
            GDFUserSettings.Instance.Delete<TokenStorage>(container: GDFUserSettings.EnvironmentContainer(_engine));
            _storage = null;
        }

        private void OnAccountChanged(IJobHandler handler, MemoryJwtToken value)
        {
            if (_manager.storage == null) return;

            GDFUserSettings.Instance.Save(_manager.storage, container: GDFUserSettings.EnvironmentContainer(_engine));
        }

        private void LoadStorage()
        {
            if (_storage != null) return;

            _storage = GDFUserSettings.Instance.LoadOrDefault<TokenStorage>(null, container: GDFUserSettings.EnvironmentContainer(_engine));
        }

        private Job AutoSignInJob()
        {
            string taskName = "Last session login";
            if (!IsAvailable)
            {
                return Job.Failure(new GDFException("ACC", 1, ""), taskName);
            }

            TokenStorage token = _storage;

            return Job.Run(handler =>
            {
                _manager.ResetToken(handler);
                _manager.SetToken(handler, token);
            });
        }
    }
}