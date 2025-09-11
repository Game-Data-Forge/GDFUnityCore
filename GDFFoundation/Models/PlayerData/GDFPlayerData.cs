#region Copyright

// Game-Data-Forge Solution
// Written by CONTART Jean-François & BOULOGNE Quentin
// GDFFoundation.csproj GDFPlayerData.cs create at 2025/04/03 09:04:09
// ©2024-2025 idéMobi SARL FRANCE

#endregion

#region

using System;
using Newtonsoft.Json;

#endregion

namespace GDFFoundation
{
    /// The GDFPlayerData class is an abstract class that represents common properties and methods for player data in a game.
    /// It extends the GDFBasicData class.
    /// @inherit GDFBasicData
    /// @modifiers [Serializable]
    public abstract class GDFPlayerData : IFieldAccount, IGDFSyncableData, IGDFStringReference
    {
        #region Instance fields and properties

        private long _account;
        private DateTime _creation;
        private long _dataVersion;
        private byte _gameSave;
        private DateTime _modification;

        private string _reference;
        private long _syncCommit;
        private DateTime _syncTime;


        /// <summary>
        ///     Represents a game save for a player.
        /// </summary>
        [JsonIgnore]
        public byte GameSave => _gameSave;

        /// <summary>
        ///     Represents a player data in the system.
        /// </summary>
        [JsonIgnore]
        public GDFPlayerDataProcessKind Process { set; get; } = GDFPlayerDataProcessKind.None;

        #region From interface IGDFAccountData

        /// <summary>
        ///     Represents an account in the game.
        /// </summary>
        [JsonIgnore]
        public long Account => _account;

        [JsonIgnore]
        [GDFDbAccess(updateAccess = GDFDbColumnAccess.Deny)]
        public DateTime Creation => _creation;

        [JsonIgnore] public long DataVersion => _dataVersion;

        [JsonIgnore] public DateTime Modification => _modification;

        [JsonIgnore] public bool Trashed { get; set; }

        #endregion

        #region From interface IGDFStringReference

        [JsonIgnore] public string Reference => _reference;

        #endregion

        #region From interface IGDFSyncableData

        /// <summary>
        ///     The channel the data is accessible from.
        /// </summary>
        [JsonIgnore]
        public int Channels { get; set; } = 0;

        /// <summary>
        ///     Represents a property indicating whether the commit is synchronized or not based on timestamp.
        /// </summary>
        [JsonIgnore]
        public long SyncCommit => _syncCommit;

        /// <summary>
        ///     Gets or sets the synchronization datetime of the player data.
        /// </summary>
        [JsonIgnore]
        public DateTime SyncDateTime => _syncTime;

        #endregion

        #endregion

        #region Instance constructors and destructors

        public GDFPlayerData()
        {
        }

        #endregion

        #region Instance methods

        /// <summary>
        ///     Copies the data from another GDFPlayerData object.
        /// </summary>
        /// <param name="sOther">The GDFPlayerData object to copy the data from.</param>
        public void CopyFrom(GDFPlayerData sOther)
        {
            // Channel = sOther.Channel;
            Process = sOther.Process;
            Trashed = sOther.Trashed;

            _reference = sOther._reference;
            _account = sOther._account;
            _gameSave = sOther._gameSave;
            _syncTime = sOther._syncTime;
            _syncCommit = sOther._syncCommit;
            _dataVersion = sOther._dataVersion;
            _creation = sOther._creation;
            _modification = sOther._modification;
        }

        #region From interface IGDFAccountData

        public object GetReference()
        {
            return Reference;
        }

        #endregion

        #endregion
    }
}