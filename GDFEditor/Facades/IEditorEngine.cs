using GDFRuntime;

namespace GDFEditor
{
    /// <summary>
    /// Represents the editor engine interface, extending the functionality of <see cref="IRuntimeEngine"/> with specific editor-related components.
    /// </summary>
    public interface IEditorEngine : IRuntimeEngine
    {
        /// <summary>
        /// Gets the editor-specific configuration settings.
        /// This property provides an instance of <see cref="IEditorConfiguration"/>,
        /// enabling access to configuration parameters specific to the editor environment.
        /// Inherits and extends functionality from <see cref="IRuntimeConfiguration"/>.
        /// </summary>
        /// <remarks>
        /// This property is typically used in conjunction with engine-related classes implementing
        /// <see cref="IEditorEngine"/> to manage editor configuration details such as channels,
        /// credentials, and other environment-dependent settings.
        /// </remarks>
        public new IEditorConfiguration Configuration { get; }

        /// <summary>
        /// Represents the thread manager responsible for managing threading functionality
        /// in the editor environment. This property provides access to the threading-related
        /// operations specific to the <see cref="IEditorEngine"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="ThreadManager"/> property is an implementation of the
        /// <see cref="IEditorThreadManager"/> interface, which extends the
        /// <see cref="IRuntimeThreadManager"/> interface to provide editor-specific threading features.
        /// </remarks>
        /// <seealso cref="IEditorThreadManager"/>
        public new IEditorThreadManager ThreadManager { get; }

        /// <summary>
        /// Gets the instance of the server manager for the editor.
        /// </summary>
        /// <remarks>
        /// This property returns an implementation of <see cref="IEditorServerManager"/>,
        /// which provides functionality specific to server management within the editor environment.
        /// It extends the capabilities of <see cref="IRuntimeServerManager"/> with additional editor-specific features.
        /// </remarks>
        /// <seealso cref="IEditorServerManager"/>
        /// <seealso cref="IRuntimeServerManager"/>
        public new IEditorServerManager ServerManager { get; }

        /// <summary>
        /// Provides access to the environment manager for the editor engine.
        /// Represents an instance of <see cref="IEditorEnvironmentManager"/> associated with the <see cref="IEditorEngine"/>.
        /// This property allows interaction with the environment's configuration, state changes, and management.
        /// </summary>
        /// <remarks>
        /// The <see cref="IEditorEnvironmentManager"/> manages notifications for environment changes,
        /// handles asynchronous tasks involving the environment, and integrates with the runtime environment engine.
        /// </remarks>
        public new IEditorEnvironmentManager EnvironmentManager { get; }

        /// <summary>
        /// Provides access to the device management system specific to the <see cref="IEditorEngine"/> environment.
        /// This property returns an instance of the <see cref="IEditorDeviceManager"/> interface,
        /// allowing operations such as adding, deleting, and managing devices in the editor context.
        /// </summary>
        /// <seealso cref="IEditorDeviceManager"/>
        public new IEditorDeviceManager DeviceManager { get; }

        /// <summary>
        /// Represents the manager responsible for handling editor-specific account operations in the context of the engine.
        /// This property returns an instance of <see cref="IEditorAccountManager"/>, which extends the features of
        /// <see cref="IRuntimeAccountManager"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="AccountManager"/> property provides access to functionalities such as authentication,
        /// credentials management, and token handling specifically tailored for the editor environment.
        /// </remarks>
        public new IEditorAccountManager AccountManager { get; }

        /// <summary>
        /// Represents the property responsible for managing player data within the editor engine.
        /// This property returns an instance of <see cref="IEditorPlayerDataManager"/>,
        /// which extends the functionality of <see cref="IRuntimePlayerDataManager"/> for editor-specific operations.
        /// </summary>
        public new IEditorPlayerDataManager PlayerDataManager { get; }

        /// <summary>
        /// Provides a specialized type management interface for the editor. This property returns an instance of
        /// <see cref="IEditorTypeManager"/>, which extends the functionality of <see cref="IRuntimeTypeManager"/>
        /// with additional editor-specific features.
        /// </summary>
        /// <remarks>
        /// The <see cref="TypeManager"/> property is typically used in scenarios where advanced type management features
        /// are required in the editor environment, such as handling type hierarchies or resolving types dynamically.
        /// It supports features like retrieving type information and processing player-specific type hierarchies
        /// through the <see cref="IEditorTypeManager"/> implementation.
        /// </remarks>
        /// <returns>
        /// An instance of <see cref="IEditorTypeManager"/>.
        /// </returns>
        public new IEditorTypeManager TypeManager { get; }

        /// <summary>
        /// Provides access to the persistence management functionality specific to the editor environment.
        /// This property returns an instance of <see cref="IEditorPlayerPersistanceManager"/> that enables
        /// managing player data persistence operations in the editor, including loading, saving, and synchronizing
        /// data in the context of the editor runtime.
        /// </summary>
        /// <remarks>
        /// The <see cref="PersistanceManager"/> property overrides the corresponding property in
        /// <see cref="IRuntimeEngine"/> and provides editor-specific implementations of persistence management.
        /// It is vital for interacting with player data storage and synchronization in the editor environment.
        /// </remarks>
        /// <seealso cref="IEditorPlayerPersistanceManager"/>
        /// <seealso cref="IEditorEngine"/>
        /// <seealso cref="IRuntimePlayerPersistanceManager"/>
        public new IEditorPlayerPersistanceManager PersistanceManager { get; }
    }
}
