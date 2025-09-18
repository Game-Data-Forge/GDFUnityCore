namespace GDFUnity
{
    public class RuntimeAccountConsent : CoreAccountConsent<RuntimeAccountManager>
    {
        public RuntimeAccountConsent(RuntimeAccountManager manager, RuntimeLicenseManager license) : base(manager, license)
        {
            
        }
    }
}