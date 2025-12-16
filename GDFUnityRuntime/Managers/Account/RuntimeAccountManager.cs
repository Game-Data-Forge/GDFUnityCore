using System;
using GDFRuntime;

namespace GDFUnity
{
    [Dependency(typeof(IRuntimeConfigurationEngine), typeof(IRuntimeThreadManager), typeof(IRuntimeLicenseManager))]
    [JobLockers(typeof(IRuntimePlayerDataManager), typeof(IRuntimePlayerPersistanceManager))]
    public class RuntimeAccountManager : CoreAccountManager<RuntimeAccountAuthentication, RuntimeAccountCredentials, RuntimeAccountConsent>
    {
        protected override Type JobLokerType => typeof(IRuntimeAccountManager);

        public RuntimeAccountManager(IRuntimeEngine engine) : base(engine)
        {
            _authentication = new RuntimeAccountAuthentication(engine, this);
            _credentials = new RuntimeAccountCredentials(this);
            _consent = new RuntimeAccountConsent(engine, this, (RuntimeLicenseManager)engine.LicenseManager);
        }
    }
}