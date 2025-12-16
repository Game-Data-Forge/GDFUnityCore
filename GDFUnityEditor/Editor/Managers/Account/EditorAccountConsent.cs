using GDFEditor;
using GDFRuntime;

namespace GDFUnity.Editor
{
    public class EditorAccountConsent : CoreAccountConsent<EditorAccountManager>, IEditorAccountManager.IEditorConsent
    {
        public EditorAccountConsent(IRuntimeEngine engine, EditorAccountManager manager, RuntimeLicenseManager license) : base(engine, manager, license)
        {
            
        }
    }
}