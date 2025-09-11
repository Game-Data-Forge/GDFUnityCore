#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj IJobHandler.cs create at 2025/05/15 11:05:03
// ©2024-2025 idéMobi SARL FRANCE

#endregion

namespace GDFFoundation
{
    public interface IJobHandler : IPoolItem
    {
        #region Instance fields and properties

        public bool IsCanceled { get; }
        public int StepAmount { get; set; }

        #endregion

        #region Instance methods

        public void Cancel();

        public IJobHandler Split(int steps = 1);

        public float Step();
        public void ThrowIfCancelled();

        #endregion
    }
}