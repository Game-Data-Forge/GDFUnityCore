using GDFEditor;

namespace GDFUnity.Editor
{
    public class EditorLicenseManager : RuntimeLicenseManager, IEditorLicenseManager
    {
        public EditorLicenseManager(IEditorEngine engine) : base(engine)
        {
        }
    }
}