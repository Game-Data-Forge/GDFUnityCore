using GDFFoundation;

namespace GDFRuntime
{
    public interface IRuntimeEngine
    {
        public Job Launch { get; }

        public IRuntimeConfiguration Configuration { get; }

        public IRuntimeThreadManager ThreadManager { get; }
        public IRuntimeServerManager ServerManager { get; }
        public IRuntimeLicenseManager LicenseManager { get; }
        public IRuntimeEnvironmentManager EnvironmentManager { get; }
        public IRuntimeDeviceManager DeviceManager { get; }
        public IRuntimeAccountManager AccountManager { get; }
        public IRuntimePlayerDataManager PlayerDataManager { get; }
        public IRuntimeTypeManager TypeManager { get; }
        public IRuntimePlayerPersistanceManager PersistanceManager { get; }

        public T Get<T>();

        public Job Stop();
        public void Kill();
    }
}
