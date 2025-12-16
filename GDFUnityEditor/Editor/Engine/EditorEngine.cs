using GDFEditor;
using GDFRuntime;
using UnityEditor;
using GDFFoundation;
using System;
using UnityEngine;

namespace GDFUnity.Editor
{
    public class EditorEngine : IEditorEngine
    {
        public enum State
        {
            Stopped = 0,
            Starting = 1,
            Started = 2,
            Stopping = 3
        }

        static private DateTime nextCheck = DateTime.MinValue;
        static private readonly object _lock = new object();
        static private EditorEngine _instance = null;
        static private EditorEngine Instance
        {
            get
            {
                try
                {
                    lock (_lock)
                    {
                        DateTime now = GDFDatetime.Now;
                        if (_instance == null)
                        {
                            nextCheck = now.AddSeconds(10);

                            IEditorConfiguration configuration = EditorConfigurationEngine.Instance.Load();
                            _instance = new EditorEngine(configuration);
                        }
                        else if (nextCheck < now)
                        {
                            nextCheck = now.AddSeconds(10);
                            EditorConfigurationEngine.Instance.Load();
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    if (_instance != null)
                    {
                        _instance = null;
                    }
                    throw;
                }

                return _instance;
            }
        }
        static internal EditorEngine UnsafeInstance => _instance;

        static private State _state = State.Stopped;

        static public State state
        {
            get => _state;
            set
            {
                _state = value;
            }
        }

        [InitializeOnLoadMethod]
        static private void EditorStart()
        {
            GDFLogger.SetWriter(new GDFLoggerUnityEditor(GDFLogLevel.Trace));

            GDF.Instance = () => Instance;
            GDFEditor.Instance = () => Instance;
        }

        private Job _launch = null;
        public IEditorConfiguration _configuration;

        public Job Launch => _launch;
        public IEditorConfiguration Configuration => _configuration;

        public IEditorThreadManager ThreadManager => GDFManagers.UnsafeGet<IEditorThreadManager>();
        public IEditorServerManager ServerManager => GDFManagers.UnsafeGet<IEditorServerManager>();
        public IEditorLicenseManager LicenseManager => GDFManagers.UnsafeGet<IEditorLicenseManager>();
        public IEditorEnvironmentManager EnvironmentManager => GDFManagers.UnsafeGet<IEditorEnvironmentManager>();
        public IEditorDeviceManager DeviceManager => GDFManagers.UnsafeGet<IEditorDeviceManager>();
        public IEditorAccountManager AccountManager => GDFManagers.UnsafeGet<IEditorAccountManager>();
        public IEditorPlayerDataManager PlayerDataManager => GDFManagers.UnsafeGet<IEditorPlayerDataManager>();
        public IEditorTypeManager TypeManager => GDFManagers.UnsafeGet<IEditorTypeManager>();
        public IEditorPlayerPersistanceManager PersistanceManager => GDFManagers.UnsafeGet<IEditorPlayerPersistanceManager>();
        
        IRuntimeConfiguration IRuntimeEngine.Configuration => Configuration;

        IRuntimeThreadManager IRuntimeEngine.ThreadManager => ThreadManager;
        IRuntimeServerManager IRuntimeEngine.ServerManager => ServerManager;
        IRuntimeLicenseManager IRuntimeEngine.LicenseManager => LicenseManager;
        IRuntimeEnvironmentManager IRuntimeEngine.EnvironmentManager => EnvironmentManager;
        IRuntimeDeviceManager IRuntimeEngine.DeviceManager => DeviceManager;
        IRuntimeAccountManager IRuntimeEngine.AccountManager => AccountManager;
        IRuntimePlayerDataManager IRuntimeEngine.PlayerDataManager => PlayerDataManager;
        IRuntimeTypeManager IRuntimeEngine.TypeManager => TypeManager;
        IRuntimePlayerPersistanceManager IRuntimeEngine.PersistanceManager => PersistanceManager;

        private EditorEngine(IEditorConfiguration configuration)
        {
            state = State.Starting;
            _configuration = configuration;

            GDFManagers.Start();
        
            GDFManagers.AddSingleton(EditorConfigurationEngine.Instance);
            GDFManagers.AddSingleton<IEditorThreadManager, EditorThreadManager>();
            GDFManagers.AddSingleton<IEditorServerManager, EditorServerManager>();
            GDFManagers.AddSingleton<IEditorLicenseManager, EditorLicenseManager>();
            GDFManagers.AddSingleton<IEditorEnvironmentManager, EditorEnvironmentManager>();
            GDFManagers.AddSingleton<IEditorDeviceManager, EditorDeviceManager>();
            GDFManagers.AddSingleton<IEditorAccountManager, EditorAccountManager>();
            GDFManagers.AddSingleton<IEditorTypeManager, EditorTypeManager>();
            GDFManagers.AddSingleton<IEditorPlayerPersistanceManager, EditorPlayerPersistanceManager>();
            GDFManagers.AddSingleton<IEditorPlayerDataManager, EditorPlayerDataManager>();

            GDFManagers.Build<IEditorEngine>(this);

            _launch = Job.Run((handler) =>
            {
                TypeManager.LoadRunner(handler);
                state = State.Started;
            }, "Start engine");
            _launch.Pool = null; // Makes sure the launch job is never recycled.
        }

        public T Get<T>()
        {
            return GDFManagers.Get<T>();
        }

        public Job Stop()
        {
            state = State.Stopping;
            return Job.Run(async handler =>
            {
                handler.StepAmount = 3;

                await _launch;

                try
                {
                    await PlayerDataManager.Stop();
                    handler.Step();

                    await AccountManager.Stop();
                    handler.Step();

                    await EnvironmentManager.Stop();
                    handler.Step();
                }
                finally
                {
                    _launch.Dispose();
                    state = State.Stopped;
                    _instance = null;
                }
            }, "Stop engine");
        }

        public void Kill()
        {
            PlayerDataManager.Kill();
            AccountManager.Kill();
            EnvironmentManager.Kill();

            state = State.Stopped;
            _instance = null;
        }
    }
}
