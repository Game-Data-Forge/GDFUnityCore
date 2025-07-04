﻿using GDFEditor;
using GDFRuntime;
using UnityEditor;
using GDFFoundation;
using System;
using UnityEngine;

namespace GDFUnity.Editor
{
    public class EditorEngine : IEditorEngine
    {
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
                catch
                {
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

        [InitializeOnLoadMethod]
        static private void EditorStart()
        {
            GDF.Instance = () => Instance;
            GDFEditor.Instance = () => Instance;
        }

        private Job _launch = null;
        private IEditorConfiguration _configuration;
        
        private IEditorThreadManager _threadManager;
        private IEditorServerManager _serverManager;
        private IEditorEnvironmentManager _environmentManager;
        private IEditorDeviceManager _deviceManager;
        private IEditorAccountManager _accountManager;
        private IEditorPlayerDataManager _playerDataManager;
        private IEditorTypeManager _typeManager;
        private IEditorPlayerPersistanceManager _persistanceManager;

        public Job Launch => _launch;
        public IEditorConfiguration Configuration => _configuration;

        public IEditorThreadManager ThreadManager => _threadManager;
        public IEditorServerManager ServerManager => _serverManager;
        public IEditorEnvironmentManager EnvironmentManager => _environmentManager;
        public IEditorDeviceManager DeviceManager => _deviceManager;
        public IEditorAccountManager AccountManager => _accountManager;
        public IEditorPlayerDataManager PlayerDataManager => _playerDataManager;
        public IEditorTypeManager TypeManager => _typeManager;
        public IEditorPlayerPersistanceManager PersistanceManager => _persistanceManager;
        
        IRuntimeConfiguration IRuntimeEngine.Configuration => Configuration;

        IRuntimeThreadManager IRuntimeEngine.ThreadManager => ThreadManager;
        IRuntimeServerManager IRuntimeEngine.ServerManager => ServerManager;
        IRuntimeEnvironmentManager IRuntimeEngine.EnvironmentManager => EnvironmentManager;
        IRuntimeDeviceManager IRuntimeEngine.DeviceManager => DeviceManager;
        IRuntimeAccountManager IRuntimeEngine.AccountManager => AccountManager;
        IRuntimePlayerDataManager IRuntimeEngine.PlayerDataManager => PlayerDataManager;
        IRuntimeTypeManager IRuntimeEngine.TypeManager => TypeManager;
        IRuntimePlayerPersistanceManager IRuntimeEngine.PersistanceManager => PersistanceManager;

        private EditorEngine(IEditorConfiguration configuration)
        {
            _configuration = configuration;

            _threadManager = new EditorThreadManager();
            _serverManager = new EditorServerManager(this);
            _environmentManager = new EditorEnvironmentManager(this);
            _deviceManager = new EditorDeviceManager(this);
            _accountManager = new EditorAccountManager(this);
            _typeManager = new EditorTypeManager();
            _persistanceManager = new EditorPlayerPersistanceManager(this);
            _playerDataManager = new EditorPlayerDataManager(this);

            _launch = Job.Run((handler) => {
                _typeManager.LoadRunner(handler);
            }, "Start engine");
            _launch.Pool = null; // Makes the launch task is never recycled.
        }

        public Job Stop()
        {
            return Job.Run(async handler => {
                handler.StepAmount = 3;
                try
                {
                    await _playerDataManager.Stop();
                    handler.Step();
                    
                    await _accountManager.Stop();
                    handler.Step();

                    await _environmentManager.Stop();
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
        }
    }
}
