using System;
using GDFEditor;
using GDFFoundation;

namespace GDFUnity.Editor
{
    static public class GDFEditor
    {
        static private Func<IEditorEngine> _instance = null;
        static public Func<IEditorEngine> Instance
        {
            set
            {
                if (_instance != null)
                {
                    throw GDF.Exceptions.EngineCannotChangeInstance;
                }
                _instance = value;
            }
        }

        static private IEditorEngine Engine
        {
            get
            {
                if (_instance == null)
                {
                    throw GDF.Exceptions.BuilderMissing;
                }

                return _instance();
            }
        }

        static private IEditorEngine StartedEngine
        {
            get
            {
                IEditorEngine engine = Engine;
                if (!engine.Launch.IsDone)
                {
                    throw GDF.Exceptions.NotLaunched;
                }
                
                if (engine.Launch.State != JobState.Success)
                {
                    throw GDF.Exceptions.LaunchFailed;
                }

                return engine;
            }
        }

        static private IEditorEngine AuthenticatedEngine
        {
            get
            {
                IEditorEngine engine = StartedEngine;
                if (!engine.AccountManager.IsAuthenticated)
                {
                    throw GDF.Exceptions.NotAuthenticated;
                }

                return engine;
            }
        }

        static public Job Launch => Engine.Launch;

        static public IEditorConfiguration Configuration => Engine.Configuration;
        
        static public IEditorThreadManager Thread => StartedEngine.Get<IEditorThreadManager>();
        static public IEditorEnvironmentManager Environment => StartedEngine.Get<IEditorEnvironmentManager>();
        static public IEditorDeviceManager Device => StartedEngine.Get<IEditorDeviceManager>();
        static public IEditorTypeManager Types => StartedEngine.Get<IEditorTypeManager>();
        static public IEditorAccountManager Account => StartedEngine.Get<IEditorAccountManager>();
        static public IEditorPlayerDataManager Player => AuthenticatedEngine.Get<IEditorPlayerDataManager>();

        static public Job Stop()
        {
            return Engine.Stop();
        }

        static public void Kill()
        {
            Engine.Kill();
        }
    }
}
