using System;
using GDFEditor;
using GDFFoundation;
using GDFRuntime;
using UnityEditor;

namespace GDFUnity.Editor
{
    [Dependency(typeof(IEditorConfigurationEngine), typeof(IEditorThreadManager))]
    [FullLockers(typeof(IEditorConfigurationEngine))]
    public class EditorEnvironmentManager : AsyncManager, IEditorEnvironmentManager
    {
        private class EnvironmentConfiguration
        {
            public ProjectEnvironment Environment { get; set; } = ProjectEnvironment.Development;
        }

        private Notification<ProjectEnvironment> _EnvironmentChangingEvent { get; }
        private Notification<ProjectEnvironment> _EnvironmentChangedEvent { get; }
        private EnvironmentConfiguration _environment;
        private IEditorEngine _engine;

        public Notification<ProjectEnvironment> EnvironmentChanging => _EnvironmentChangingEvent;
        public Notification<ProjectEnvironment> EnvironmentChanged => _EnvironmentChangedEvent;
        public ProjectEnvironment Environment => _environment.Environment;

        private GDFException canceledByUserException => new GDFException("ENV", 01, "Operation canceled by user !");

        protected override Type JobLokerType => typeof(IEditorEnvironmentManager);

        public EditorEnvironmentManager(IEditorEngine engine)
        {
            _engine = engine;
            _environment = GDFUserSettings.Instance.LoadOrDefault(new EnvironmentConfiguration(), container: GDFUserSettings.ProjectContainer(_engine));
            _EnvironmentChangingEvent = new Notification<ProjectEnvironment>(engine.ThreadManager);
            _EnvironmentChangedEvent = new Notification<ProjectEnvironment>(engine.ThreadManager);
        }

        public Job<ProjectEnvironment> SetEnvironment(ProjectEnvironment environment)
        {
            return JobLocker(() => ChangeEnvironmentJob(environment));
        }

        private Job<ProjectEnvironment> ChangeEnvironmentJob(ProjectEnvironment environment)
        {
            string taskName = "Switch environment";
            if (environment == _environment.Environment)
            {
                return Job<ProjectEnvironment>.Success(environment, taskName);
            }

            if (GDF.Account.IsAuthenticated)
            {
                if (!EditorUtility.DisplayDialog("Account conflict", "You are trying to change the environment while connected to an account"+
                "\nProceeding will disconnect the current account.", "Ok", "Cancel"))
                {
                    return Job<ProjectEnvironment>.Failure(canceledByUserException, taskName);
                }
            }
            
            return JobLocker(() => Job<ProjectEnvironment>.Run(handler => {
                handler.StepAmount = 3;

                EnvironmentChanging.Invoke(handler.Split(), environment);

                _environment.Environment = environment;

                handler.Step();
                GDFUserSettings.Instance.Save(_environment, container: GDFUserSettings.ProjectContainer(_engine));

                EnvironmentChanged.Invoke(handler.Split(), environment);
                return environment;
            }, taskName));
        }
    }
}