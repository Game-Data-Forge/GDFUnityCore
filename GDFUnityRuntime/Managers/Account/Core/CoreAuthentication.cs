using System;
using GDFFoundation;
using GDFRuntime;
using static GDFUnity.CoreAccountManager;

namespace GDFUnity
{
    public abstract class CoreAccountAuthentication : IRuntimeAccountManager.IRuntimeAuthentication
    {
        protected CoreAccountManager _manager;

        public abstract IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeLocal Local { get; }
        public abstract IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeDevice Device { get; }
        public abstract IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeEmailPassword EmailPassword { get; }
        public abstract IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeLastSession LastSession { get; }

        public CoreAccountAuthentication(CoreAccountManager manager)
        {
            _manager = manager;
            manager.AccountDeleted.onBackgroundThread += SignOutRunner;
        }

        ~CoreAccountAuthentication()
        {
            _manager.AccountDeleted.onBackgroundThread -= SignOutRunner;
        }

        public abstract Job SignOut();
        
        internal void SignOutRunner(IJobHandler handler)
        {
            if (!_manager.IsAuthenticated)
            {
                return;
            }

            using IDisposable _ = _manager.Lock();

            handler.StepAmount = 3;
            TokenStorage token = _manager.storage;

            _manager.ResetToken(handler.Split());

            if (_manager.IsLocal)
            {
                _manager.SetToken(handler.Split(2), null);
                return;
            }

            try
                {
                    string url = _manager.GenerateURL(token.Country, "/api/v1/authentication/close-session");
                    _manager.Put<int>(handler.Split(), url);
                }
                catch { }
                finally
                {
                    _manager.SetToken(handler.Split(), null);
                }
        }
    }

    public class CoreAccountAuthentication<T, U, V, W> : CoreAccountAuthentication
        where T : CoreAuthenticationLocal
        where U : CoreAuthenticationDevice
        where V : CoreAuthenticationEmailPassword
        where W : CoreAuthenticationLastSession
    {
        protected T _local;
        protected U _device;
        protected V _emailPassword;
        protected W _reSign;

        public override IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeLocal Local => _local;
        public override IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeDevice Device => _device;
        public override IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeEmailPassword EmailPassword => _emailPassword;
        public override IRuntimeAccountManager.IRuntimeAuthentication.IRuntimeLastSession LastSession => _reSign;

        public CoreAccountAuthentication(CoreAccountManager manager) : base(manager)
        {
        }

        public override Job SignOut()
        {
            lock (_manager.LOCK)
            {
                _manager.EnsureUseable();
                _manager.job = Job.Run(SignOutRunner, "Sign out");

                return _manager.job;
            }
        }
    }
}