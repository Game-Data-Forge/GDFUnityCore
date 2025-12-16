using GDFEditor;
using GDFFoundation;

namespace GDFUnity.Editor
{
    public class EditorAccountAuthentication : CoreAccountAuthentication<EditorAuthenticationLocal, EditorAuthenticationDevice, EditorAuthenticationEmailPassword, EditorAuthenticationLastSession>, IEditorAccountManager.IEditorAuthentication
    {
        IEditorAccountManager.IEditorAuthentication.IEditorLocal IEditorAccountManager.IEditorAuthentication.Local => _local;
        IEditorAccountManager.IEditorAuthentication.IEditorDevice IEditorAccountManager.IEditorAuthentication.Device => _device;
        IEditorAccountManager.IEditorAuthentication.IEditorEmailPassword IEditorAccountManager.IEditorAuthentication.EmailPassword => _emailPassword;
        IEditorAccountManager.IEditorAuthentication.IEditorLastSession IEditorAccountManager.IEditorAuthentication.LastSession => _reSign;

        public EditorAccountAuthentication(IEditorEngine engine, EditorAccountManager manager) : base(manager)
        {
            _local = new EditorAuthenticationLocal(engine, manager);
            _device = new EditorAuthenticationDevice(engine, manager);
            _emailPassword = new EditorAuthenticationEmailPassword(engine, manager);
            _reSign = new EditorAuthenticationLastSession(engine, manager);

            engine.EnvironmentManager.EnvironmentChanged.onBackgroundThread += OnEnvironmentChanged;
        }

        private void OnEnvironmentChanged(IJobHandler handler, ProjectEnvironment environment)
        {
            SignOutRunner(handler);
        }
    }
}