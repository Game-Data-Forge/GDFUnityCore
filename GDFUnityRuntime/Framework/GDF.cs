using System;
using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    static public class GDF
    {
        static public class Exceptions
        {
            static public GDFException EngineCannotChangeInstance => new GDFException("ENG", 1, "Cannot changed engine instance ! Instance already set...");
            static public GDFException BuilderMissing => new GDFException("ENG", 2, "Cannot fetch engine ! Instance missing...");
            static public GDFException NotLaunched => new GDFException("ENG", 3, "The engine is not launched ! Feature inaccessible...");
            static public GDFException LaunchFailed => new GDFException("ENG", 4, "The engine failed to lauch ! Feature inaccessible...");
            static public GDFException NotAuthenticated => new GDFException("ENG", 5, "Not connected to an account ! Feature inaccessible...");
        }

        static private Func<IRuntimeEngine> _instance = null;
        static public Func<IRuntimeEngine> Instance
        {
            set
            {
                if (_instance != null)
                {
                    throw Exceptions.EngineCannotChangeInstance;
                }
                _instance = value;
            }
        }

        static private IRuntimeEngine Engine
        {
            get
            {
                if (_instance == null)
                {
                    throw Exceptions.BuilderMissing;
                }

                return _instance();
            }
        }

        static private IRuntimeEngine StartedEngine
        {
            get
            {
                IRuntimeEngine engine = Engine;
                if (!engine.Launch.IsDone)
                {
                    throw Exceptions.NotLaunched;
                }
                
                if (engine.Launch.State != JobState.Success)
                {
                    throw Exceptions.LaunchFailed;
                }

                return engine;
            }
        }

        static private IRuntimeEngine AuthenticatedEngine
        {
            get
            {
                IRuntimeEngine engine = StartedEngine;
                if (!engine.AccountManager.IsAuthenticated)
                {
                    throw Exceptions.NotAuthenticated;
                }

                return engine;
            }
        }

        static public Job Launch => Engine.Launch;

        static public IRuntimeConfiguration Configuration => Engine.Configuration;
        
        static public IRuntimeThreadManager Thread => StartedEngine.Get<IRuntimeThreadManager>();
        static public IRuntimeEnvironmentManager Environment => StartedEngine.Get<IRuntimeEnvironmentManager>();
        static public IRuntimeLicenseManager License => StartedEngine.Get<IRuntimeLicenseManager>();
        static public IRuntimeDeviceManager Device => StartedEngine.Get<IRuntimeDeviceManager>();
        static public IRuntimeAccountManager Account => StartedEngine.Get<IRuntimeAccountManager>();
        static public IRuntimePlayerDataManager Player => AuthenticatedEngine.Get<IRuntimePlayerDataManager>();

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
