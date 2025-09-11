using System.IO;
using GDFFoundation;
using GDFRuntime;
using Newtonsoft.Json;

namespace GDFUnity.Editor
{
    public class UserSettings : FileStorage<UserSettings>
    {
        private const string _CONTAINER = "UserSettings";

        static public string ProjectContainer(IRuntimeEngine engine)
        {
            return engine.Configuration.Reference.ToString();
        }

        static public string EnvironmentContainer(IRuntimeEngine engine)
        {
            return Path.Combine(ProjectContainer(engine), engine.EnvironmentManager.Environment.ToLongString());
        }

        static public string AccountContainer(IRuntimeEngine engine)
        {
            return Path.Combine(EnvironmentContainer(engine), engine.AccountManager.Identity);
        }

        protected override Formatting _Formatting => Formatting.Indented;

        public string GetDataPath(long projectReference)
        {
            return GenerateContainerName(projectReference.ToString());
        }

        public override string GenerateContainerName(string container)
        {
            if (container == null)
            {
                container = _CONTAINER;
            }
            else
            {
                container = Path.Combine(_CONTAINER, container);
            }
            return base.GenerateContainerName(container);
        }

        public void DeleteProject(long reference)
        {
            string container = GenerateContainerName(reference.ToString());
            if (Directory.Exists(container))
            {
                Directory.Delete(container, true);
            }
        }
    }
}
