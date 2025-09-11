#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFVolatileDataStorage.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     Represents a volatile data storage model.
    /// </summary>
    [Serializable]
    public class GDFVolatileDataStorage : GDFDataBasicDataStorage, IGDFDataTrack, IGDFWritableLongReference
    {
        #region Instance fields and properties

        #region From interface IGDFAccountDependence

        /// <summary>
        ///     Represents a volatile data storage for an account.
        /// </summary>
        public long Account { set; get; }

        /// <summary>
        ///     Represents a range value.
        /// </summary>
        public short Range { set; get; }

        #endregion

        #region From interface IGDFDataTrack

        /// <summary>
        ///     Represents a data track that can be used to track changes in a data storage.
        /// </summary>
        public Int64 DataTrack { set; get; }

        #endregion

        #region From interface IGDFWritableLongReference

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public long Reference { get; set; }

        #endregion

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     This class represents a volatile data storage model.
        /// </summary>
        public GDFVolatileDataStorage()
        {
        }

        #endregion

        #region Instance methods

        /// <summary>
        ///     Copies the values of the given GDFVolatileDataStorage object to this GDFVolatileDataStorage object.
        /// </summary>
        /// <param name="sOther">The GDFVolatileDataStorage object to copy from.</param>
        public void CopyFrom(GDFVolatileDataStorage sOther)
        {
            base.CopyFrom(sOther);
            DataTrack = sOther.DataTrack;
            Account = sOther.Account;
            Range = sOther.Range;
        }

        #endregion
    }
}