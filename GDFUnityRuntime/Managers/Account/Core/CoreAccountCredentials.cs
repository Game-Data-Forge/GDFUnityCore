using System.Collections;
using System.Collections.Generic;
using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    public abstract class CoreAccountCredentials : IRuntimeAccountManager.IRuntimeCredentials
    {
        protected List<GDFAccountSign> _credentials = null;
        public List<GDFAccountSign> Credentials => _credentials;

        public abstract IRuntimeAccountManager.IRuntimeCredentials.IRuntimeEmailPassword EmailPassword { get; }

        public abstract Job Refresh();

        public IEnumerator<GDFAccountSign> GetEnumerator()
        {
            return _credentials.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class CoreAccountCredentials<T, U> : CoreAccountCredentials
        where T : CoreAccountManager
        where U : CoreCredentialsEmailPassword
    {
        private T _manager;

        protected U _emailPassword;

        public CoreAccountCredentials(T manager)
        {
            _manager = manager;
        }

        public override IRuntimeAccountManager.IRuntimeCredentials.IRuntimeEmailPassword EmailPassword => _emailPassword;

        public override Job Refresh()
        {
            string jobName = "Get account credentials";
            
            if (_manager.IsLocal)
            {
                return Job.Success(jobName);
            }
            
            return _manager.JobLocker(() => Job.Run(handler =>
            {
                string url = _manager.GenerateURL(_manager.Country, "/api/v1/accounts/" + _manager.Reference + "/signs");

                IEnumerable<GDFAccountSign> response = _manager.Get<IEnumerable<GDFAccountSign>>(handler, url);
                if (_credentials == null)
                {
                    _credentials = new List<GDFAccountSign>();
                }
                else
                {
                    _credentials.Clear();
                }

                foreach (GDFAccountSign sign in response)
                {
                    _credentials.Add(sign);
                }
            }, jobName));
        }
    }
}