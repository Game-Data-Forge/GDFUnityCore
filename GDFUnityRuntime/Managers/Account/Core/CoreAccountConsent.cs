using System;
using System.Collections.Generic;
using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    public abstract class CoreAccountConsent : IRuntimeAccountManager.IRuntimeConsent
    {
        static private class Exceptions
        {
            static public GDFException NoLicense => new GDFException("CNS", 01, "No license found ! Please, Refresh the license first.");
        }


        protected LicenseInformation _license;
        private bool _consent;

        public bool AgreedToLicense
        {
            get
            {
                return _consent;
            }
            set => _consent = value;
        }
        public string LicenseURL => _License.URL;
        public string LicenseName => _License.Name;
        public string LicenseVersion => _License.Version;
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

        public abstract Job RefreshLicense();

        public abstract Job SaveLicenseAgreement();

        public abstract Job<bool> CheckLicenseAgreementValidity();

        protected bool CheckTokenLicenseAgreement(MemoryJwtToken token)
        {
            if (!token.Consent) return false;

            return token.ConsentVersion == _License.Version && token.ConsentName == _License.Name;
        }
        
        protected void OnAccountChanged(IJobHandler handler, MemoryJwtToken token)
        {
            if (token != null)
            {
                _consent = false;
                return;
            }

            try
                {
                    _consent = CheckTokenLicenseAgreement(token);
                }
                catch { }
        }
    }
    
    public abstract class CoreAccountConsent<T> : CoreAccountConsent
        where T : CoreAccountManager
    {
        private T _manager;

        public CoreAccountConsent(T manager)
        {
            _manager = manager;

            _manager.AccountChanged.onBackgroundThread += OnAccountChanged;
        }

        ~CoreAccountConsent()
        {
            _manager.AccountChanged.onBackgroundThread -= OnAccountChanged;
        }

        public override Job RefreshLicense()
        {
            return _manager.JobLocker(() => Job.Run(RefreshLicenseRunner, "Refresh licence"));
        }

        public override Job SaveLicenseAgreement()
        {
            if (!_manager.IsAuthenticated)
            {
                throw GDF.Exceptions.NotAuthenticated;
            }

            return _manager.JobLocker(() => Job.Run(handler =>
            {
                // Get consent
                string url = _manager.GenerateURL(_manager.storage.Country, $"/api/v1/accounts/{_manager.Reference}/consents");
                List<GDFAccountConsent> consents = _manager.Get<List<GDFAccountConsent>>(handler.Split(), url);

                // Update consent
                consents[0].ConsentName = _License.Name;
                consents[0].ConsentVersion = _License.Version;
                consents[0].Consent = AgreedToLicense;

                // Save consent
                url = _manager.GenerateURL(_manager.storage.Country, $"/api/v1/accounts/{_manager.Reference}/consents");
                _manager.Post<int>(handler.Split(), url, consents[0]);

            }, "Save licence agreement"));
        }

        public override Job<bool> CheckLicenseAgreementValidity()
        {
            return _manager.JobLocker(() => Job<bool>.Run(handler =>
            {
                // Check licence cache
                RefreshLicenseRunner(handler);

                // Check validity
                return AgreedToLicense;
            }, "Check licence agreement"));
        }

        private void RefreshLicenseRunner(IJobHandler handler)
        {
            string url = _manager.GenerateURL(Country.US, $"/api/v1/information/license");
            _license = _manager.Get<LicenseInformation>(handler, url);
        }

    }
}