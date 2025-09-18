using GDFFoundation;

namespace GDFRuntime
{
    public interface IRuntimeLicenseManager : IAsyncManager
    {
        public string URL { get; }
        public string Name { get; }
        public string Version { get; }

        public Job Refresh();
    }
}