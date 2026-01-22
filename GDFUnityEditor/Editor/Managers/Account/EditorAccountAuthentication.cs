using GDFEditor;
using GDFFoundation;

namespace GDFUnity.Editor
{
    public class EditorAccountAuthentication : CoreAccountAuthentication<EditorAuthenticationLocal, EditorAuthenticationDevice, EditorAuthenticationEmailPassword, EditorAuthenticationOAuth, EditorAuthenticationLastSession>, IEditorAccountManager.IEditorAuthentication
    {
        IEditorAccountManager.IEditorAuthentication.IEditorLocal IEditorAccountManager.IEditorAuthentication.Local => _local;
        IEditorAccountManager.IEditorAuthentication.IEditorDevice IEditorAccountManager.IEditorAuthentication.Device => _device;
        IEditorAccountManager.IEditorAuthentication.IEditorEmailPassword IEditorAccountManager.IEditorAuthentication.EmailPassword => _emailPassword;
        IEditorAccountManager.IEditorAuthentication.IEditorOAuth IEditorAccountManager.IEditorAuthentication.Apple => _apple;
        IEditorAccountManager.IEditorAuthentication.IEditorOAuth IEditorAccountManager.IEditorAuthentication.Discord => _discord;
        IEditorAccountManager.IEditorAuthentication.IEditorOAuth IEditorAccountManager.IEditorAuthentication.Facebook => _facebook;
        IEditorAccountManager.IEditorAuthentication.IEditorOAuth IEditorAccountManager.IEditorAuthentication.Google => _google;
        IEditorAccountManager.IEditorAuthentication.IEditorOAuth IEditorAccountManager.IEditorAuthentication.LinkedIn => _linkedIn;
        IEditorAccountManager.IEditorAuthentication.IEditorLastSession IEditorAccountManager.IEditorAuthentication.LastSession => _reSign;

        public EditorAccountAuthentication(IEditorEngine engine, EditorAccountManager manager) : base(manager)
        {
            _local = new EditorAuthenticationLocal(engine, manager);
            _device = new EditorAuthenticationDevice(engine, manager);
            _emailPassword = new EditorAuthenticationEmailPassword(engine, manager);
            _apple = new EditorAuthenticationOAuth(engine, manager, GDFOAuthKind.Apple);
            _discord = new EditorAuthenticationOAuth(engine, manager, GDFOAuthKind.Discord);
            _facebook = new EditorAuthenticationOAuth(engine, manager, GDFOAuthKind.Facebook);
            _google = new EditorAuthenticationOAuth(engine, manager, GDFOAuthKind.Google);
            _linkedIn = new EditorAuthenticationOAuth(engine, manager, GDFOAuthKind.LinkedIn);
            _reSign = new EditorAuthenticationLastSession(engine, manager);

            engine.EnvironmentManager.EnvironmentChanged.onBackgroundThread += OnEnvironmentChanged;
        }

        private void OnEnvironmentChanged(IJobHandler handler, ProjectEnvironment environment)
        {
            SignOutRunner(handler);
        }
    }
}