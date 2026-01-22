using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    public class RuntimeAccountAuthentication : CoreAccountAuthentication<RuntimeAuthenticationLocal, RuntimeAuthenticationDevice, RuntimeAuthenticationEmailPassword, RuntimeAuthenticationOAuth, RuntimeAuthenticationLastSession>
    {
        public RuntimeAccountAuthentication(IRuntimeEngine engine, RuntimeAccountManager manager) : base(manager)
        {
            _local = new RuntimeAuthenticationLocal(engine, manager);
            _device = new RuntimeAuthenticationDevice(engine, manager);
            _emailPassword = new RuntimeAuthenticationEmailPassword(engine, manager);
            _apple = new RuntimeAuthenticationOAuth(engine, manager, GDFOAuthKind.Apple);
            _discord = new RuntimeAuthenticationOAuth(engine, manager, GDFOAuthKind.Discord);
            _facebook = new RuntimeAuthenticationOAuth(engine, manager, GDFOAuthKind.Facebook);
            _google = new RuntimeAuthenticationOAuth(engine, manager, GDFOAuthKind.Google);
            _linkedIn = new RuntimeAuthenticationOAuth(engine, manager, GDFOAuthKind.LinkedIn);
            _reSign = new RuntimeAuthenticationLastSession(engine, manager);
        }
    }
}