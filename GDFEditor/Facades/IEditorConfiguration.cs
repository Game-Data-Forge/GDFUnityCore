using System.Collections.Generic;
using GDFFoundation;
using GDFRuntime;

namespace GDFEditor
{
    /// <summary>
    /// Provides configuration settings for the editor, extending functionality of
    /// <see cref="IRuntimeConfiguration"/>.
    /// </summary>
    public interface IEditorConfiguration : IRuntimeConfiguration
    {
        /// <summary>
        /// Represents the channel configuration used for identifying the associated
        /// channel in the editor configuration context.
        /// </summary>
        /// <remarks>
        /// The <see cref="Channel"/> property is utilized to manage the assignment
        /// of specific channel identifiers to the editor instance. It plays a key
        /// role in facilitating the proper configuration and runtime behavior of the
        /// editor environment.
        /// </remarks>
        /// <seealso cref="GDFUnity.Editor.EditorConfiguration"/>
        /// <seealso cref="GDFEditor.IEditorConfiguration"/>
        /// <seealso cref="GDFRuntime.IRuntimeConfiguration"/>
        /// <seealso cref="GDFUnity.Editor.GlobalSettingsProvider"/>
        public new short Channel { get; set; }

        /// <summary>
        /// Gets or sets the Dashboard property, which holds the address or URL of the dashboard
        /// related to the current editor configuration. This property is utilized to manage and validate
        /// the dashboard data within the <see cref="EditorConfiguration"/> and other dependent services.
        /// </summary>
        /// <remarks>
        /// The validity of the <see cref="Dashboard"/> property can be subject to validation in
        /// classes such as <see cref="EditorConfigurationEngine"/>, where the configuration is checked
        /// to ensure accurate setups. An invalid <see cref="Dashboard"/> value can lead to exceptions
        /// like <see cref="GDFException"/> with proper diagnostic information.
        /// </remarks>
        /// <seealso cref="EditorConfiguration"/>
        /// <seealso cref="EditorConfigurationEngine"/>
        /// <seealso cref="IEditorConfiguration"/>
        public string Dashboard { get; }

        /// <summary>
        /// Gets the credentials information mapped to the associated
        /// <see cref="ProjectEnvironment"/> enums within the editor configuration context.
        /// </summary>
        /// <remarks>
        /// The dictionary maps each <see cref="ProjectEnvironment"/> to its corresponding
        /// <see cref="GDFProjectMinimalCredentials"/> object, which contains the minimal required
        /// details for project access or configuration validation.
        /// </remarks>
        /// <returns>
        /// A <see cref="System.Collections.Generic.Dictionary{TKey, TValue}"/> where the key is of type
        /// <see cref="ProjectEnvironment"/> and the value is of type
        /// <see cref="GDFProjectMinimalCredentials"/>.
        /// </returns>
        public Dictionary<ProjectEnvironment, GDFProjectMinimalCredentials> Credentials { get; }

        /// <summary>
        /// Gets or sets a dictionary representing the available channels and their corresponding identifiers.
        /// Each key in the dictionary represents the channel name as a <see cref="string"/> and each value represents
        /// the channel identifier as a <see cref="short"/>.
        /// </summary>
        /// <remarks>
        /// This property is used to manage and validate configurations related to channels within
        /// an implementation of <see cref="IEditorConfiguration"/>.
        /// </remarks>
        /// <seealso cref="IEditorConfiguration"/>
        public Dictionary<string, short> Channels { get; }
    }
}