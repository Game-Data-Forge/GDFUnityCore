using GDFEditor;
using GDFRuntime;

namespace GDFUnity.Editor
{
    [Dependency(typeof(IEditorConfigurationEngine))]
    [FullLockers(typeof(IEditorConfigurationEngine))]
    public class EditorServerManager : RuntimeServerManager, IEditorServerManager
    {
        public EditorServerManager(IRuntimeEngine engine) : base(engine)
        {
        }
    }
}