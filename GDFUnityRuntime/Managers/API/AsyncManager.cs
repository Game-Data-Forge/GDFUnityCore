using System;
using System.Threading.Tasks;
using GDFFoundation;
using GDFRuntime;

namespace GDFUnity
{
    public abstract class AsyncManager : IAsyncManager
    {
        protected Job _job;
        protected abstract Type JobLokerType { get; }

        public virtual async Task Stop()
        {
            if (_job == null) return;

            if (!_job.IsDone)
            {
                _job.Cancel();
                await _job;
            }

            _job.Dispose();
        }

        public Job JobLocker(Func<Job> body)
        {
            _job = GDFManagers.Lock(JobLokerType, body);
            return _job;
        }

        public Job<T> JobLocker<T>(Func<Job<T>> body)
        {
            return (Job<T>)JobLocker(() => (Job)body?.Invoke());
        }
    }
}