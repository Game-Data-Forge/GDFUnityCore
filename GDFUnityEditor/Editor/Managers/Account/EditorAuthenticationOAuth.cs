using GDFEditor;
using GDFFoundation;

namespace GDFUnity.Editor
{
    public class EditorAuthenticationOAuth : CoreAuthenticationOAuth<IEditorEngine>, IEditorAccountManager.IEditorAuthentication.IEditorOAuth
    {
        public EditorAuthenticationOAuth(IEditorEngine engine, EditorAccountManager manager, GDFOAuthKind kind) : base(engine, manager, kind)
        {
            
        }
    }
}