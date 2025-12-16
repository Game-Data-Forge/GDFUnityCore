using GDFRuntime;

namespace GDFUnity
{
    public class RuntimeAccountConsent : CoreAccountConsent<RuntimeAccountManager>
    {
        public RuntimeAccountConsent(IRuntimeEngine engine, RuntimeAccountManager manager, RuntimeLicenseManager license) : base(engine, manager, license)
        {
            
        }
    }
}