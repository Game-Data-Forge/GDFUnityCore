using GDFEditor;

namespace GDFUnity.Editor
{
    public class EditorAccountConsent : CoreAccountConsent<EditorAccountManager>, IEditorAccountManager.IEditorConsent
    {
        public EditorAccountConsent(EditorAccountManager manager) : base(manager)
        {
            
        }
    }
}