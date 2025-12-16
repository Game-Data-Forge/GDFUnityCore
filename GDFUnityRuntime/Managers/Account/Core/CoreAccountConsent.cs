using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    public abstract class CoreAccountConsent : IRuntimeAccountManager.IRuntimeConsent
    {
        public abstract IRuntimeAccountManager.IRuntimeConsent.ILicenseAgreement LicenseAgreement { get; }
    }

    public abstract class CoreAccountConsent<T> : CoreAccountConsent
        where T : CoreAccountManager
    {
        private class Agreement : IRuntimeAccountManager.IRuntimeConsent.ILicenseAgreement
        {
            private CoreAccountConsent<T> _consent;
            private Notification<bool> _licenseAgreementChanged;

            public Notification<bool> LicenseAgreementChanged => _licenseAgreementChanged;

            public Agreement(IRuntimeEngine engine, CoreAccountConsent<T> consent)
            {
                _consent = consent;
                _licenseAgreementChanged = new Notification<bool>(engine.ThreadManager);
            }

            public Job<bool> Get()
            {
                return _consent._account.JobLocker(() => Job<bool>.Run(handler =>
                {
                    handler.StepAmount = 3;

                    // Refresh agreement
                    _consent._license.RefreshRunner(handler.Split());

                    string url = _consent._account.GenerateURL(_consent._account.storage.Country, $"/api/v1/accounts/{_consent._account.Reference}/consents");
                    GDFAccountConsent[] consents = _consent._account.Get<GDFAccountConsent[]>(handler.Split(), url);
                    if (consents == null || consents.Length == 0)
                    {
                        _licenseAgreementChanged.Invoke(handler.Split(), false);
                        return false;
                    }

                    GDFAccountConsent consent = null;
                    for (int i = 0; i < consents.Length; i++)
                    {
                        if (consents[i].ConsentName == _consent._license.Name)
                        {
                            consent = consents[i];
                        }
                    }

                    if (consent == null)
                    {
                        _licenseAgreementChanged.Invoke(handler.Split(), false);
                        return false;
                    }

                    // Check agreement
                    bool agreement = _consent._license.Version == consent.ConsentVersion && consent.Consent;
                    _licenseAgreementChanged.Invoke(handler.Split(), agreement);
                    return agreement;
                }, "Get licence agreement"));
            }

            public Job Set(bool agreeToLicense)
            {
                return _consent._account.JobLocker(() => Job.Run(handler =>
                {
                    handler.StepAmount = 2;
                    // Create consent
                    GDFAccountConsent consent = new GDFAccountConsent()
                    {
                        Account = _consent._account.Token.Account,
                        Project = _consent._account.Token.Project,
                        Range = _consent._account.Token.Range,
                        Channel = _consent._account.Token.Channel,
                        Consent = agreeToLicense,
                        ConsentName = _consent._license.Name,
                        ConsentVersion = _consent._license.Version,
                        Country = _consent._account.Token.Country
                    };

                    // Save consent
                    string url = _consent._account.GenerateURL(_consent._account.storage.Country, $"/api/v1/accounts/{_consent._account.Reference}/consents");
                    _consent._account.Post(handler.Split(), url, consent);

                    _licenseAgreementChanged.Invoke(handler.Split(), agreeToLicense);

                }, "Set licence agreement"));
            }
        }

        private Agreement _licenseAgreement;
        private T _account;
        private RuntimeLicenseManager _license;

        public override IRuntimeAccountManager.IRuntimeConsent.ILicenseAgreement LicenseAgreement
        {
            get
            {
                if (!_account.IsAuthenticated)
                {
                    throw GDF.Exceptions.NotAuthenticated;
                }

                return _licenseAgreement;
            }
        }

        public CoreAccountConsent(IRuntimeEngine engine, T account, RuntimeLicenseManager license)
        {
            _account = account;
            _license = license;

            _licenseAgreement = new Agreement(engine, this);
        }
    }
}