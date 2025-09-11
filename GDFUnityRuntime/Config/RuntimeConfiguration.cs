using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    public class RuntimeConfiguration : IRuntimeConfiguration
    {
        public long Reference { get; set; }
        public string Name { get; set; }
        public string Organization { get; set; }
        public ProjectEnvironment Environment { get; set; }
        public string PublicToken { get; set; }
        public string PrivateToken { get; set; }
        public short Channel { get; set; }
        public CloudConfiguration CloudConfig { get; set; }
    }
}
