#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj SimpleHandler.cs create at 2025/05/15 11:05:03
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System.Threading.Tasks;

#endregion

namespace GDFFoundation
{
    public class SimpleHandler : IJobHandler
    {
        #region Instance fields and properties

        private bool _cancelled = false;

        #region From interface IJobHandler

        public bool IsCanceled => _cancelled;

        public Pool Pool { get; set; }

        public int StepAmount { get; set; }

        #endregion

        #endregion

        #region Instance constructors and destructors

        public SimpleHandler()
        {
            OnPooled();
        }

        #endregion

        #region Instance methods

        #region From interface IJobHandler

        public void Cancel()
        {
            _cancelled = true;
        }

        public void Dispose()
        {
        }

        public void OnPooled()
        {
        }

        public void OnReleased()
        {
        }

        public IJobHandler Split(int steps = 1)
        {
            return this;
        }

        public float Step()
        {
            return 0;
        }

        public void ThrowIfCancelled()
        {
            if (_cancelled)
            {
                throw new TaskCanceledException();
            }
        }

        #endregion

        #endregion
    }
}