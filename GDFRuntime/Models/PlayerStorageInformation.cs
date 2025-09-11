using System;

namespace GDFRuntime
{
    /// <summary>
    /// Represents storage information for a player, including game save data and synchronization details.
    /// </summary>
    public class PlayerStorageInformation
    {
        /// <summary>
        /// Represents a reference identifier associated with a <see cref="GDFRuntime.PlayerStorageInformation"/> instance.
        /// </summary>
        /// <remarks>
        /// The <see cref="Reference"/> property is used as a primary key and uniquely identifies
        /// an instance of <see cref="GDFRuntime.PlayerStorageInformation"/>. This property is also utilized
        /// in database operations such as table creation, selection, and updates.
        /// </remarks>
        public byte Reference { get; set; }

        /// <summary>
        /// Represents the date and time of the last synchronization operation.
        /// This property is updated whenever new data, such as player reference
        /// or game-related information, is synchronized from a source.
        /// </summary>
        /// <remarks>
        /// The <see cref="SyncDate"/> property is primarily utilized in tracking the
        /// most recent synchronization operation for player data in the
        /// <see cref="PlayerStorageInformation"/> class. It is referenced in systems
        /// such as the <see cref="RuntimePlayerDataManager"/> to determine the appropriate
        /// state of synchronization during interactions with server-side data or local cache.
        /// </remarks>
        /// <seealso cref="DateTime"/>
        /// <seealso cref="PlayerStorageInformation"/>
        /// <seealso cref="RuntimePlayerDataManager"/>
        public DateTime SyncDate { get; set; }

        /// <summary>
        /// Gets the number of game saves available in the <see cref="GameSaves"/> instance.
        /// This property provides a count of elements managed by the <see cref="GameSaves"/> object
        /// associated with this instance of <see cref="PlayerStorageInformation"/>.
        /// </summary>
        public int Count => GameSaves.Count;

        /// <summary>
        /// Represents a collection of saved game data.
        /// This class provides functionalities for managing and iterating saved game entries.
        /// </summary>
        /// <remarks>
        /// The <see cref="GameSaves"/> class is utilized for storing, adding, and retrieving game save data.
        /// It is maintained as a property within <see cref="PlayerStorageInformation"/> for centralized use.
        /// </remarks>
        /// <seealso cref="IEnumerable{T}"/>
        /// <seealso cref="PlayerStorageInformation"/>
        public GameSaves GameSaves = new GameSaves();
    }
}