using GDFEditor;

namespace GDFUnity.Editor
{
    [Dependency(typeof(IEditorConfigurationEngine), typeof(IEditorAccountManager))]
    [FullLockers(typeof(IEditorConfigurationEngine), typeof(IEditorEnvironmentManager), typeof(IEditorDeviceManager), typeof(IEditorAccountManager))]
    [JobLockers(typeof(IEditorPlayerPersistanceManager))]
    public class EditorPlayerDataManager : RuntimePlayerDataManager, IEditorPlayerDataManager
    {
        public EditorPlayerDataManager(IEditorEngine engine) : base(engine)
        {
            
        }
    }
}