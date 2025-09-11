using GDFFoundation;

namespace GDFEditor
{
    /// <summary>
    /// Represents a device with a name, unique identifier, and operating system.
    /// Implements the <see cref="ICredentials"/> interface.
    /// </summary>
    public class Device : ICredentials
    {
        /// <summary>
        /// Gets or sets the name of the <see cref="Device"/>.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the device.
        /// This property is used to uniquely identify an instance of <see cref="Device"/>.
        /// </summary>
        /// <seealso cref="Device"/>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the operating system of the device.
        /// </summary>
        /// <value>
        /// A value of type <see cref="GDFExchangeDevice"/> that represents the platform or operating system
        /// associated with the current device.
        /// </value>
        public GDFExchangeDevice OS { get; set; }
    }
}
