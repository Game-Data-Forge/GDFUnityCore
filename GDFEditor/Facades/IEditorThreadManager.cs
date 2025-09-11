using GDFRuntime;

namespace GDFEditor
{
    /// <summary>
    /// Defines the interface for managing thread operations specific to editor environments.
    /// </summary>
    /// <remarks>
    /// This interface extends the <see cref="IRuntimeThreadManager"/> to provide additional
    /// threading functionality tailored for editor-specific usage. Implementations of this
    /// interface are responsible for executing and coordinating tasks on the appropriate threads
    /// within an editor context.
    /// </remarks>
    /// <seealso cref="IRuntimeThreadManager"/>
    public interface IEditorThreadManager : IRuntimeThreadManager
    {
        
    }
}