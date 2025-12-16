#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj Job.cs create at 2025/05/15 11:05:03
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace GDFFoundation
{
    public class Job : IJob
    {
        #region Static fields and properties

        static public bool silentExceptions = false;

        static private Pool<Job> _pool = new Pool<Job>();

        #endregion

        #region Static methods

        static public Job Failure(Exception error, [CallerMemberName] string name = "Unknown")
        {
            Job task = _pool.Get();
            task._name = name;
            task._error = error;
            task._state = JobState.Failure;
            return task;
        }

        static public Job Run(Action<IJobHandler> action, [CallerMemberName] string name = "Unknown")
        {
            Job task = _pool.Get();
            task._name = name;

            Task.Run(() => task.Process(action));
            return task;
        }

        static public Job Run(Func<IJobHandler, Task> action, [CallerMemberName] string name = "Unknown")
        {
            Job task = _pool.Get();
            task._name = name;

            task.Process(action);
            return task;
        }

        static public Job Success([CallerMemberName] string name = "Unknown")
        {
            Job task = _pool.Get();
            task._name = name;
            task._state = JobState.Success;
            return task;
        }

        #endregion

        #region Instance fields and properties

        protected Exception _error;
        protected string _name;
        protected JobState _state;
        internal float progress;

        internal CancellationTokenSource source;

        #region From interface IJob

        public Exception Error => _error;
        public bool IsCancelled => source.IsCancellationRequested;
        public bool IsDone => _state >= JobState.Success;

        public string Name => _name;

        public Pool Pool { get; set; }

        public float Progress
        {
            get => progress;
        }

        public JobState State
        {
            get => _state;
        }

        #endregion

        #endregion

        #region Instance methods

        private void Process(Action<IJobHandler> action)
        {
            using (IJobHandler handler = JobHandler.Get(this))
            {
                try
                {
                    _state = JobState.Running;
                    action?.Invoke(handler);
                    handler.ThrowIfCancelled();
                    progress = 1;
                    _state = JobState.Success;
                }
                catch (TaskCanceledException)
                {
                    _state = JobState.Cancelled;
                }
                catch (OperationCanceledException)
                {
                    _state = JobState.Cancelled;
                }
                catch (Exception e)
                {
                    GDFLogger.Error(e);

                    _error = e;
                    _state = JobState.Failure;
                }
            }
        }

        private async void Process(Func<IJobHandler, Task> action)
        {
            using (IJobHandler handler = JobHandler.Get(this))
            {
                try
                {
                    _state = JobState.Running;
                    await action?.Invoke(handler);
                    handler.ThrowIfCancelled();
                    progress = 1;
                    _state = JobState.Success;
                }
                catch (TaskCanceledException)
                {
                    _state = JobState.Cancelled;
                }
                catch (OperationCanceledException)
                {
                    _state = JobState.Cancelled;
                }
                catch (Exception e)
                {
                    GDFLogger.Error(e);

                    _error = e;
                    _state = JobState.Failure;
                }
            }
        }

        #region From interface IJob

        public void Cancel()
        {
            source.Cancel();
        }

        public void Dispose()
        {
            PoolItem.Release(this);
        }

        public TaskAwaiter GetAwaiter()
        {
            return Task.Run(Wait).GetAwaiter();
        }

        public virtual void OnPooled()
        {
            source = new CancellationTokenSource();
            _state = JobState.Pending;
            progress = 0;
            _error = null;
        }

        public void OnReleased()
        {
            source?.Dispose();
            source = null;
        }

        public void Wait()
        {
            while (!IsDone)
            {
            }

            if (silentExceptions) return;

            if (State == JobState.Failure)
            {
                throw Error;
            }
        }

        #endregion

        #endregion
    }

    public class Job<T> : Job, IJob<T>
    {
        #region Static fields and properties

        static private Pool<Job<T>> _pool = new Pool<Job<T>>();

        #endregion

        #region Static methods

        static public new Job<T> Failure(Exception error, [CallerMemberName] string name = "Unknown")
        {
            Job<T> task = _pool.Get();
            task._name = name;
            task._error = error;
            task._result = default;
            task._state = JobState.Failure;
            return task;
        }

        static public Job<T> Run(Func<IJobHandler, T> action, [CallerMemberName] string name = "Unknown")
        {
            Job<T> task = _pool.Get();
            task._name = name;

            Task.Run(() => task.Process(action));
            return task;
        }

        static public Job<T> Run(Func<IJobHandler, Task<T>> action, [CallerMemberName] string name = "Unknown")
        {
            Job<T> task = _pool.Get();
            task._name = name;

            task.Process(action);
            return task;
        }

        static public Job<T> Success(T value = default, [CallerMemberName] string name = "Unknown")
        {
            Job<T> task = _pool.Get();
            task._name = name;
            task._result = value;
            task._state = JobState.Success;
            return task;
        }

        #endregion

        #region Instance fields and properties

        private T _result;

        #region From interface IJob<T>

        public T Result
        {
            get
            {
                return Wait();
            }
        }

        #endregion

        #endregion

        #region Instance methods

        private void Process(Func<IJobHandler, T> action)
        {
            using (IJobHandler handler = JobHandler.Get(this))
            {
                try
                {
                    _state = JobState.Running;
                    _result = action.Invoke(handler);
                    handler.ThrowIfCancelled();
                    progress = 1;
                    _state = JobState.Success;
                }
                catch (TaskCanceledException)
                {
                    _state = JobState.Cancelled;
                }
                catch (OperationCanceledException)
                {
                    _state = JobState.Cancelled;
                }
                catch (Exception e)
                {
                    GDFLogger.Error(e);

                    _error = e;
                    _state = JobState.Failure;
                }
            }
        }

        private async void Process(Func<IJobHandler, Task<T>> action)
        {
            using (IJobHandler handler = JobHandler.Get(this))
            {
                try
                {
                    _state = JobState.Running;
                    _result = await action.Invoke(handler);
                    handler.ThrowIfCancelled();
                    progress = 1;
                    _state = JobState.Success;
                }
                catch (TaskCanceledException)
                {
                    _state = JobState.Cancelled;
                }
                catch (OperationCanceledException)
                {
                    _state = JobState.Cancelled;
                }
                catch (Exception e)
                {
                    GDFLogger.Error(e);
                    
                    _error = e;
                    _state = JobState.Failure;
                }
            }
        }

        #region From interface IJob<T>

        public new TaskAwaiter<T> GetAwaiter()
        {
            return Task.Run(Wait).GetAwaiter();
        }

        public override void OnPooled()
        {
            base.OnPooled();
            _result = default;
        }

        public new T Wait()
        {
            base.Wait();
            return _result;
        }

        #endregion

        #endregion
    }
}