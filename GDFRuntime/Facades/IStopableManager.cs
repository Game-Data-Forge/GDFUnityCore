using System.Threading.Tasks;

namespace GDFRuntime
{
    public interface IStopableManager
    {
        public Task Stop ();
        public void Kill ();
    }
}
