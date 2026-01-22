using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    static public class GDFPlayerDataStorageExtension
    {
        static public void Initialize(this GDFPlayerDataStorage storage, IRuntimeEngine engine)
        {
            storage.Account = engine.AccountManager.Reference;
            storage.Range = engine.AccountManager.Range;
            storage.Project = engine.Configuration.Reference;
        }
        
        static public void Initialize(this GDFPlayerDataStorage storage, IRuntimeEngine engine, byte gameSave)
        {
            Initialize(storage, engine);
            storage.GameSave = gameSave;
        }
    }
}