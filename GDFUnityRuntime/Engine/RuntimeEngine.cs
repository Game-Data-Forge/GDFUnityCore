using GDFRuntime;
using UnityEngine;
using GDFFoundation;

namespace GDFUnity
{
    public class RuntimeEngine : IRuntimeEngine
    {
        static private readonly object _lock = new object();
        static private RuntimeEngine _instance = null;
        static public RuntimeEngine Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new RuntimeEngine();
                    }
                }

                return _instance;
            }
        }

#if !UNITY_EDITOR
        [RuntimeInitializeOnLoadMethod]
        static private void RuntimeStart()
        {
            string _ = FileStorage.ROOT;
            GDF.Instance = () => Instance;
        }
#endif

        private GameObject _gameObject = null;

        private Job _launch = null;
        public IRuntimeConfiguration _configuration;
        
        public Job Launch => _launch;
        public IRuntimeConfiguration Configuration => _configuration;

        public IRuntimeThreadManager ThreadManager => GDFManagers.UnsafeGet<IRuntimeThreadManager>();
        public IRuntimeServerManager ServerManager => GDFManagers.UnsafeGet<IRuntimeServerManager>();
        public IRuntimeLicenseManager LicenseManager => GDFManagers.UnsafeGet<IRuntimeLicenseManager>();
        public IRuntimeEnvironmentManager EnvironmentManager => GDFManagers.UnsafeGet<IRuntimeEnvironmentManager>();
        public IRuntimeDeviceManager DeviceManager => GDFManagers.UnsafeGet<IRuntimeDeviceManager>();
        public IRuntimeAccountManager AccountManager => GDFManagers.UnsafeGet<IRuntimeAccountManager>();
        public IRuntimePlayerDataManager PlayerDataManager => GDFManagers.UnsafeGet<IRuntimePlayerDataManager>();
        public IRuntimeTypeManager TypeManager => GDFManagers.UnsafeGet<IRuntimeTypeManager>();
        public IRuntimePlayerPersistanceManager PersistanceManager => GDFManagers.UnsafeGet<IRuntimePlayerPersistanceManager>();

        private RuntimeEngine()
        {
            _configuration = RuntimeConfigurationEngine.Instance.Load();

            GDFManagers.Start();

            GDFManagers.AddSingleton(RuntimeConfigurationEngine.Instance);
            
            _gameObject = new GameObject("GDF Engine");
            GameObject.DontDestroyOnLoad(_gameObject);

            GDFManagers.AddSingleton<IRuntimeThreadManager, RuntimeThreadManager>(_gameObject.AddComponent<RuntimeThreadManager>());
            GDFManagers.AddSingleton<IRuntimeServerManager, RuntimeServerManager>();
            GDFManagers.AddSingleton<IRuntimeLicenseManager, RuntimeLicenseManager>();
            GDFManagers.AddSingleton<IRuntimeEnvironmentManager, RuntimeEnvironmentManager>();
            GDFManagers.AddSingleton<IRuntimeDeviceManager, RuntimeDeviceManager>();
            GDFManagers.AddSingleton<IRuntimeAccountManager, RuntimeAccountManager>();
            GDFManagers.AddSingleton<IRuntimeTypeManager, RuntimeTypeManager>();
            GDFManagers.AddSingleton<IRuntimePlayerPersistanceManager, RuntimePlayerPersistanceManager>();
            GDFManagers.AddSingleton<IRuntimePlayerDataManager, RuntimePlayerDataManager>();

            GDFManagers.Build<IRuntimeEngine>(this);

            _launch = Job.Run((handler) => {
                GDFManagers.UnsafeGet<IRuntimeTypeManager>().LoadRunner(handler);
            }, "Start engine");
            _launch.Pool = null; // Makes sure the launch job is never recycled.
        }

        public T Get<T>()
        {
            return GDFManagers.Get<T>();
        }

        public Job Stop()
        {
            GameObject.Destroy(_gameObject);
            return Job.Run(async handler =>
            {
                handler.StepAmount = 2;
                try
                {
                    await PlayerDataManager.Stop();
                    handler.Step();

                    await AccountManager.Stop();
                    handler.Step();
                }
                finally
                {
                    _launch.Dispose();
                    _instance = null;
                }
            }, "Stop engine");
        }

        public void Kill()
        {
            _instance = null;
            GameObject.Destroy(_gameObject);
        }
    }
}
