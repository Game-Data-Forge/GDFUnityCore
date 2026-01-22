using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    public class RuntimeAuthenticationOAuth : CoreAuthenticationOAuth<IRuntimeEngine>
    {
        public RuntimeAuthenticationOAuth(IRuntimeEngine engine, RuntimeAccountManager manager, GDFOAuthKind kind) : base(engine, manager, kind)
        {
            
        }
    }
}