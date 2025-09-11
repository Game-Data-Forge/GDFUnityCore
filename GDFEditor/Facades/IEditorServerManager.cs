using GDFRuntime;

namespace GDFEditor
{
    /// <summary>
    /// Defines the contract for an editor-specific server manager, extending the functionality provided by <see cref="IRuntimeServerManager"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="IEditorServerManager"/> interface is designed to provide additional functionality
    /// and customization specific to the editor environment.
    /// It serves as a specialized extension of the <see cref="IRuntimeServerManager"/> interface, enabling
    /// enhanced server management capabilities within tools and environments used for development.
    /// </remarks>
    /// <seealso cref="IRuntimeServerManager"/>
    public interface IEditorServerManager : IRuntimeServerManager
    {
        
    }
}