#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFVolatileData.cs create at 2025/03/26 17:03:12
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;

#endregion

namespace GDFFoundation
{
    /// <summary>
    ///     The GDFVolatileData class represents a volatile data object in the GDFFoundation namespace.
    ///     This class is an abstract class and cannot be instantiated directly.
    /// </summary>
    [Serializable]
    public abstract class GDFVolatileData : IGDFDbStorage, IGDFRangedData, IWritableFieldAccount, IGDFWritableLongReference
    {
        #region Static methods

        /// <summary>
        ///     Calculates the hash value for an GDFAccount.
        /// </summary>
        /// <param name="sAccount">The GDFAccount object.</param>
        /// <returns>The hash value as a string.</returns>
        static public string Hash(GDFAccount sAccount)
        {
            return Hash(sAccount.Reference, sAccount.Project);
        }

        /// <summary>
        ///     Generates a hash for the given account reference and project ID.
        /// </summary>
        /// <param name="sAccountReference">The account reference.</param>
        /// <param name="sProjectReference">The project ID.</param>
        /// <returns>The generated hash.</returns>
        static public string Hash(long sAccountReference, long sProjectReference)
        {
            return GDFSecurityTools.GenerateSha(sAccountReference.ToString() + " " + sProjectReference.ToString());
        }

        #endregion

        #region Instance fields and properties

        private long _reference;
        private long _reference1;

        /// <summary>
        ///     Represents an anonymous unique identity.
        /// </summary>
        /// <remarks>
        ///     This property stores a string value that represents an anonymous unique identity. It is used for anonymizing data and ensuring privacy.
        /// </remarks>
        /// <value>The anonymous unique identity.</value>
        public string Anonymous { set; get; } = string.Empty; // Who ? => anonymous

        /// <summary>
        ///     Gets or sets the BundleId associated with the GDFVolatileData object.
        /// </summary>
        /// <remarks>
        ///     The BundleId represents the unique identifier for the bundle or application that the GDFVolatileData belongs to.
        /// </remarks>
        /// <value>The BundleId of the GDFVolatileData object.</value>
        public string BundleId { set; get; } = string.Empty;

        /// <summary>
        ///     Represents a category of data in the GDFFoundation system.
        /// </summary>
        public string Category { set; get; } = string.Empty; // Why ? => category

        /// <summary>
        ///     Represents a data track in the GDFVolatileData class.
        /// </summary>
        public Int64 DataTrack { set; get; }

        /// <summary>
        ///     Represents a device used in the application.
        /// </summary>
        public string Device { set; get; } = string.Empty;

        /// <summary>
        ///     Represents the version of the engine.
        /// </summary>
        /// <remarks>
        ///     The engine version represents the version of the software engine being used.
        ///     This property is used to track the version of the software for compatibility and debugging purposes.
        /// </remarks>
        public string EngineVersion { set; get; } = string.Empty;

        /// <summary>
        ///     Represents a group of volatile data objects.
        /// </summary>
        public string Group { internal set; get; } = string.Empty;

        /// <summary>
        ///     Gets or sets the Harvest version of the data.
        /// </summary>
        public string HarvestVersion { set; get; } = string.Empty;

        /// <summary>
        ///     Represents additional information related to a volatile data model.
        /// </summary>
        public string Information { set; get; } = string.Empty; // How ? => information

        /// <summary>
        ///     Represents the language of a volatile data object.
        /// </summary>
        public string Language { set; get; } = string.Empty;

        /// <summary>
        ///     Represents the origin of the GDF exchange.
        /// </summary>
        public GDFExchangeOrigin Origin { set; get; } = GDFExchangeOrigin.Unknown;

        /// <summary>
        ///     Represents a property that stores the path of a file or directory.
        /// </summary>
        public string Path { set; get; } = string.Empty;

        /// <summary>
        ///     Represents session information for a specific user or device.
        /// </summary>
        public string Session { set; get; } = string.Empty;

        public DateTime SyncDateTime { get; set; }

        /// <summary>
        ///     Represents the timestamp of a volatile data object.
        /// </summary>
        public long Timestamp { set; get; } = 0; // When ? => DateTime

        #region From interface IGDFDbStorage

        public long Project { get; set; }
        public long RowId { get; set; }

        #endregion

        #region From interface IGDFRangedData

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public DateTime Creation { get; set; }

        public long DataVersion { get; set; }
        public DateTime Modification { get; set; }
        public int Range { get; set; }
        public bool Trashed { get; set; }

        #endregion

        #region From interface IGDFWritableAccountData

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public long Account { get; set; }

        #endregion

        #region From interface IGDFWritableLongReference

        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public long Reference { get; set; }

        #endregion

        #endregion

        #region Instance constructors and destructors

        /// <summary>
        ///     The GDFVolatileData class represents volatile data which can be stored in the database.
        /// </summary>
        public GDFVolatileData()
        {
        }

        /// <summary>
        ///     Represents a volatile data for GDFFoundation.
        /// </summary>
        public GDFVolatileData(IGDFVolatileAgent sVolatileManager, Enum sCategory, string sInformation = "")
        {
            Init(sVolatileManager);
            Category = sCategory.ToString();
            Information = sInformation;
        }

        /// <summary>
        ///     Represents volatile data used in GDFFoundation.
        /// </summary>
        public GDFVolatileData(IGDFVolatileAgent sVolatileManager, string sCategory, string sInformation = "")
        {
            Init(sVolatileManager);
            Category = sCategory;
            Information = sInformation;
        }

        #endregion

        #region Instance methods

        /// <summary>
        ///     Initializes the GDFVolatileData object with the information provided by the volatile manager.
        /// </summary>
        /// <param name="sVolatileManager">The volatile manager that provides the session information.</param>
        public void Init(IGDFVolatileAgent sVolatileManager)
        {
            Timestamp = GDFTimestamp.Timestamp();
            EngineVersion = LibrariesWorkflow.GetForFoundation().Version();
            if (sVolatileManager != null)
            {
                Session = sVolatileManager.GetSession();
                Device = sVolatileManager.GetDevice();
                Language = sVolatileManager.GetLanguage();
                HarvestVersion = sVolatileManager.GetHarvestVersion();
                Project = sVolatileManager.GetProjectReference();
                Account = sVolatileManager.GetAccount();
                DataTrack = sVolatileManager.GetDataTrack();
                Origin = sVolatileManager.GetOrigin();
                BundleId = sVolatileManager.GetBundleId();
                Path = sVolatileManager.GetPath();
                Anonymous = Hash(Account, Project);
            }
        }

        #region From interface IGDFRangedData

        public object GetReference() => Reference;

        #endregion

        #endregion
    }
}