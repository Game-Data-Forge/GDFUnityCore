using System;
using GDFFoundation;
using GDFRuntime;
using Newtonsoft.Json;
using static GDFUnity.CoreAccountManager;

namespace GDFUnity
{
    public abstract class CoreAuthenticationLocal : IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeLocal
    {
        public abstract bool Exists { get; }
        public abstract Job Login();
    }

    public class CoreAuthenticationLocal<T> : CoreAuthenticationLocal where T : IRuntimeEngine
    {
        protected T _engine;
        protected TokenStorage _storage;
        private CoreAccountManager _manager;

        public override bool Exists
        {
            get
            {
                LoadStorage();
                return _storage != null;
            }
        }
        
        public CoreAuthenticationLocal(T engine, CoreAccountManager manager)
        {
            _engine = engine;
            _manager = manager;
            _manager.AccountDeleting.onBackgroundThread += OnAccountDeleting;
        }

        ~CoreAuthenticationLocal()
        {
            _manager.AccountDeleting.onBackgroundThread -= OnAccountDeleting;
        }

        public override Job Login()
        {
            return _manager.JobLocker(() => Job.Run(handler =>
            {
                handler.StepAmount = 2;

                _manager.ResetToken(handler.Split());
                if (!Exists)
                {
                    _storage = new TokenStorage(Country.None, "###." + JsonConvert.SerializeObject(new MemoryJwtToken
                    {
                        Account = 0,
                        Channel = _engine.Configuration.Channel,
                        Country = Country.None,
                        Environment = _engine.EnvironmentManager.Environment,
                        LastSync = DateTime.MinValue,
                        Project = _engine.Configuration.Reference,
                        Range = 0,
                        Token = "LOCAL",
                    }).ToBase64URL() + ".###");

                    GDFUserSettings.Instance.Save(_storage, container: GDFUserSettings.EnvironmentContainer(_engine));
                }
                _manager.SetToken(handler.Split(), _storage);
            }, "Local login"));
        }

        private void OnAccountDeleting(IJobHandler handler)
        {
            if (!GDF.Account.IsLocal)
            {
                return;
            }

            GDFUserSettings.Instance.Delete<TokenStorage>(container: GDFUserSettings.EnvironmentContainer(_engine));
            _storage = null;
        }

        private void LoadStorage()
        {
            if (_storage != null) return;

            _storage = GDFUserSettings.Instance.LoadOrDefault<TokenStorage>(null, container: GDFUserSettings.EnvironmentContainer(_engine));
        }
    }
}