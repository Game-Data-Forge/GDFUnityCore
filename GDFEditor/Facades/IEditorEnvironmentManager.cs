using GDFFoundation;
using GDFRuntime;

namespace GDFEditor
{
    /// <summary>
    /// Interface defining the manager for handling editor environment operations.
    /// This interface provides mechanisms for controlling and managing the current editor environment,
    /// including asynchronous operations and notifications.
    /// </summary>
    /// <remarks>
    /// The <see cref="IEditorEnvironmentManager"/> serves as a key component for managing the state of
    /// editor environments, integrating functionality from both <see cref="IRuntimeEnvironmentManager"/> and
    /// <see cref="IAsyncManager"/>. It plays a critical role in handling environment configuration, state
    /// transitions, and event notifications.
    /// </remarks>
    public interface IEditorEnvironmentManager : IRuntimeEnvironmentManager, IAsyncManager
    {
        /// <summary>
        /// Gets a <see cref="Notification{T}"/> of type <see cref="ProjectEnvironment"/> that triggers when the environment is in the process of being changed.
        /// This property allows subscribers to observe notifications related to an upcoming change in the project environment.
        /// </summary>
        public Notification<ProjectEnvironment> EnvironmentChanging { get; }

        /// <summary>
        /// Gets a <see cref="Notification{T}"/> of type <see cref="ProjectEnvironment"/> that triggers after the environment has been successfully changed.
        /// This property allows subscribers to observe notifications related to a completed change in the project environment.
        /// </summary>
        public Notification<ProjectEnvironment> EnvironmentChanged { get; }

        /// <summary>
        /// Configures the application environment with the specified project environment information.
        /// </summary>
        /// <param name="environment">The project environment of type <see cref="ProjectEnvironment"/> to be set, containing specific settings and parameters.</param>
        /// <returns>A <see cref="Job{ProjectEnvironment}"/> indicating the outcome of the environment configuration process.</returns>
        public Job<ProjectEnvironment> SetEnvironment(ProjectEnvironment environment);
    }
}