using System;
using System.Collections.Generic;
using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    public class RuntimeLicenseManager : APIManager, IRuntimeLicenseManager
    {
        static private class Exceptions
        {
            static public GDFException NoLicense => new GDFException("LCS", 01, "No license found ! Please, Refresh the license first.");
        }

        private class APIErrorMiddleWare : ErrorManager<APIException>.IMiddleware
        {
            private RuntimeLicenseManager _consent;
            public int priority => 0;

            public APIErrorMiddleWare(RuntimeLicenseManager consent)
            {
                _consent = consent;
            }

            public void Runner(IJobHandler handler, ErrorManager<APIException>.State state)
            {
                if (
                    state.error != ConsentException.AccountConsentError
                    && state.error != ConsentException.AccountConsentFalse)
                {
                    return;
                }

                _consent.RefreshRunner(handler);
            }
        }

        protected IRuntimeEngine _engine;
        private APIErrorMiddleWare _errorMiddleware;
        private LicenseInformation _license;
        protected Dictionary<string, string> _headers = new Dictionary<string, string>();
        
        public RuntimeLicenseManager(IRuntimeEngine engine)
        {
            _engine = engine;

            _errorMiddleware = new APIErrorMiddleWare(this);
            errorManager.Add(_errorMiddleware);
        }

        ~RuntimeLicenseManager()
        {
            errorManager.Remove(_errorMiddleware);
        }

        protected LicenseInformation _License
        {
            get
            {
                if (_license == null)
                {
                    throw Exceptions.NoLicense;
                }
                return _license;
            }
        }

        public string URL => _License.URL;
        public string Name => _License.Name;
        public string Version => _License.Version;

        protected override Type JobLokerType => typeof(IRuntimeLicenseManager);

        public Job Refresh()
        {
            return JobLocker(() => Job.Run(RefreshRunner, "Refresh licence"));
        }

        internal void RefreshRunner(IJobHandler handler)
        {
            _headers.Clear();
            _engine.ServerManager.FillHeaders(_headers);

            string url = _engine.ServerManager.BuildAuthURL(Country.US, $"/api/v1/information/license");
            try
            {
                _license = Get<LicenseInformation>(handler, url, _headers);
            }
            catch
            {
                _license = null;
                throw;
            }
        }
    }
}