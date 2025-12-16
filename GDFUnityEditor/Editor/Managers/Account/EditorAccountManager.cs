using System;
using GDFEditor;
using GDFFoundation;

namespace GDFUnity.Editor
{
    [Dependency(typeof(IEditorConfigurationEngine), typeof(IEditorThreadManager), typeof(IEditorEnvironmentManager))]
    [FullLockers(typeof(IEditorConfigurationEngine), typeof(IEditorEnvironmentManager))]
    [JobLockers(typeof(IEditorPlayerDataManager), typeof(IEditorPlayerPersistanceManager))]
    public class EditorAccountManager : CoreAccountManager<EditorAccountAuthentication, EditorAccountCredentials, EditorAccountConsent>, IEditorAccountManager
    {
        IEditorAccountManager.IEditorAuthentication IEditorAccountManager.Authentication => _authentication;
        IEditorAccountManager.IEditorCredentials IEditorAccountManager.Credentials => _credentials;
        IEditorAccountManager.IEditorConsent IEditorAccountManager.Consent => _consent;

        MemoryJwtToken IEditorAccountManager.Token => Token;

        protected override Type JobLokerType => typeof(IEditorAccountManager);


        public EditorAccountManager(IEditorEngine engine) : base(engine)
        {
            _authentication = new EditorAccountAuthentication(engine, this);
            _credentials = new EditorAccountCredentials(this);
            _consent = new EditorAccountConsent(engine, this, (RuntimeLicenseManager)engine.LicenseManager);
        }
    }
}