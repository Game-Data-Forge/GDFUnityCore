using GDFEditor;

namespace GDFUnity.Editor
{
    [Dependency(typeof(IEditorConfigurationEngine), typeof(IEditorAccountManager))]
    [FullLockers(typeof(IEditorConfigurationEngine), typeof(IEditorAccountManager), typeof(IEditorPlayerDataManager))]
    public class EditorPlayerPersistanceManager : RuntimePlayerPersistanceManager, IEditorPlayerPersistanceManager
    {
        public EditorPlayerPersistanceManager(IEditorEngine engine) : base(engine)
        {
            
        }
    }
}