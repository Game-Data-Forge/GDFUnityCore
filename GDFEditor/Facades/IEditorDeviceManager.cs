using System;
using System.Collections.Generic;
using GDFRuntime;

namespace GDFEditor
{
    /// <summary>
    /// Interface for managing editor devices in the <see cref="IEditorEngine"/> environment.
    /// </summary>
    public interface IEditorDeviceManager : IRuntimeDeviceManager
    {
        /// <summary>
        /// An event that is triggered whenever there is a change in the state of a <see cref="Device"/>.
        /// This event is part of the <see cref="IEditorDeviceManager"/> interface and is raised to notify
        /// subscribers of any updates to the current device or device list.
        /// </summary>
        public event Action<Device> onDeviceChanged;

        /// <summary>
        /// Gets the list of all available devices managed by the current instance of
        /// <see cref="IEditorDeviceManager"/>.
        /// </summary>
        /// <remarks>
        /// Each device in the list is represented by an instance of <see cref="Device"/>.
        /// This property allows enumeration or direct access to the devices managed by
        /// the editor's device management system.
        /// </remarks>
        public List<Device> Devices { get; }

        /// <summary>
        /// Gets the currently selected <see cref="Device"/> instance managed by the <see cref="IEditorDeviceManager"/>.
        /// </summary>
        /// <remarks>
        /// This property provides access to the active <see cref="Device"/> in the context of the editor's device management system.
        /// The value of the property may change based on user actions or internal logic triggering device selection updates.
        /// </remarks>
        /// <seealso cref="IEditorDeviceManager"/>
        /// <seealso cref="Device"/>
        public Device Current { get; }

        /// <summary>
        /// Selects a specified device as the current device.
        /// </summary>
        /// <param name="device">The <see cref="Device"/> to be selected.</param>
        public void Select(Device device);

        /// <summary>
        /// Adds a new device to the device manager and returns the created instance.
        /// </summary>
        /// <returns>
        /// The newly created <see cref="Device"/> instance.
        /// </returns>
        public Device Add();

        /// <summary>
        /// Deletes the specified <see cref="Device"/> from the collection of devices managed by the <see cref="IEditorDeviceManager"/>.
        /// </summary>
        /// <param name="device">The <see cref="Device"/> instance to be deleted.</param>
        /// <returns>
        /// A boolean value indicating whether the <see cref="Device"/> was successfully deleted. Returns <c>true</c> if successfully deleted; otherwise, <c>false</c>.
        /// </returns>
        public bool Delete(Device device);

        /// <summary>
        /// Determines if the specified <see cref="Device"/> is the default device.
        /// </summary>
        /// <param name="device">The <see cref="Device"/> to check if it is the default device.</param>
        /// <returns>A boolean value indicating whether the specified <see cref="Device"/> is the default device.</returns>
        public bool IsDefault(Device device);

        /// <summary>
        /// Saves the current device configuration by updating the selected device
        /// index in the configuration and storing the configuration using
        /// <see cref="GDFUnity.GDFUserSettings"/>.
        /// </summary>
        /// <remarks>
        /// The method updates the device index in <see cref="DeviceConfiguration"/>,
        /// reflecting the currently selected device, and then persists this configuration
        /// using <see cref="GDFUnity.GDFUserSettings"/>. It interacts with
        /// <see cref="GDFEditor.IEditorEngine"/> to access the configuration reference.
        /// </remarks>
        public void Save();
    }
}