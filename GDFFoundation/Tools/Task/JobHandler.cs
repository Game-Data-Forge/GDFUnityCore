#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj JobHandler.cs create at 2025/05/15 11:05:03
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    public class JobHandler : IJobHandler
    {
        #region Static fields and properties

        static private Pool<JobHandler> _pool = new Pool<JobHandler>();

        #endregion

        #region Static methods

        static public JobHandler Get(Job task)
        {
            JobHandler handler = _pool.Get();
            handler._task = task;
            handler._minGlobal = 0;
            handler._maxGlobal = 1;
            return handler;
        }

        #endregion

        #region Instance fields and properties

        private JobHandler _handler = null;
        private float _maxGlobal;
        private float _minGlobal;
        private int _step;
        private int _stepAmount;
        private Job _task;

        #region From interface IJobHandler

        public bool IsCanceled => _task.IsCancelled;

        public Pool Pool { get; set; }

        public int StepAmount
        {
            get => _stepAmount;
            set => _stepAmount = value;
        }

        #endregion

        #endregion

        #region Instance methods

        private float Lerp(float min, float max, float value)
        {
            float result = min + (max - min) * value;
            if (result < min)
            {
                return min;
            }

            if (result > max)
            {
                return max;
            }

            return result;
        }

        #region From interface IJobHandler

        public void Cancel()
        {
            _task.Cancel();
        }

        public void Dispose()
        {
            _handler?.Dispose();
            PoolItem.Release(this);
        }

        public void OnPooled()
        {
            _minGlobal = 0;
            _maxGlobal = 1;
            _step = 0;
            _stepAmount = 0;
        }

        public void OnReleased()
        {
            CheckValidity(this);

            _task = null;
            _handler = null;
        }

        public IJobHandler Split(int steps = 1)
        {
            ThrowIfCancelled();

            if (steps <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(steps), "Cannot be inferior or equal to 0 !");
            }

            if (_handler == null)
            {
                _handler = _pool.Get();
                _handler._task = _task;
            }
            else
            {
                CheckValidity(_handler);
            }

            _handler._minGlobal = Lerp(_minGlobal, _maxGlobal, (float)_step / _stepAmount);
            _handler._maxGlobal = Lerp(_minGlobal, _maxGlobal, (float)(_step + steps) / _stepAmount);
            _handler._step = 0;

            _step += steps;

            return _handler;
        }

        public float Step()
        {
            ThrowIfCancelled();

            _step++;
            _task.progress = Lerp(_minGlobal, _maxGlobal, (float)_step / _stepAmount);
            return _task.progress;
        }

        public void ThrowIfCancelled()
        {
            _task.source.Token.ThrowIfCancellationRequested();
        }

        private void CheckValidity(JobHandler handler)
        {
                if (handler._step < handler._stepAmount - 1)
                {
                    GDFLogger.Warning(() => $"The job {_task.Name} is missing steps !\nPlease, make sure every method in the job has the right amount of StepAmount and Step().");
                }

                if (handler._step > handler.StepAmount)
                {
                    GDFLogger.Warning(() => $"The job {_task.Name} has too many steps !\nPlease, make sure every method in the job has the right amount of StepAmount and Step().");
                }

                _task.progress = handler._maxGlobal;
        }

        #endregion

        #endregion
    }
}