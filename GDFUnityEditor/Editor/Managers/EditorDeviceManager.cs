using System;
using System.Collections.Generic;
using GDFEditor;
using GDFFoundation;
using UnityEngine;

namespace GDFUnity.Editor
{
    [Dependency(typeof(IEditorConfigurationEngine))]
    [FullLockers(typeof(IEditorConfigurationEngine))]
    public class EditorDeviceManager : RuntimeDeviceManager, IEditorDeviceManager
    {
        static internal class Exceptions
        {
            static public GDFException NotDeleteException => new GDFException("DEV", 1, "The device could not be deleted !");
        }
        private class DeviceConfiguration
        {
            public List<Device> Devices { get; set; } = new List<Device>();
            public int Current { get; set; } = -1;
        }

        static private GDFExchangeDevice CurrentOS()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.OSXEditor:
                case RuntimePlatform.OSXPlayer:
                case RuntimePlatform.OSXServer:
                    return GDFExchangeDevice.Macos;
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                case RuntimePlatform.WindowsServer:
                    return GDFExchangeDevice.Windows;
                case RuntimePlatform.LinuxEditor:
                case RuntimePlatform.LinuxPlayer:
                case RuntimePlatform.LinuxServer:
                case RuntimePlatform.EmbeddedLinuxArm32:
                case RuntimePlatform.EmbeddedLinuxArm64:
                case RuntimePlatform.EmbeddedLinuxX64:
                case RuntimePlatform.EmbeddedLinuxX86:
                    return GDFExchangeDevice.Linux;
                case RuntimePlatform.Android:
                    return GDFExchangeDevice.Android;
                case RuntimePlatform.IPhonePlayer:
                    return GDFExchangeDevice.Ios;
                case RuntimePlatform.WebGLPlayer:
                    return GDFExchangeDevice.Web;
                default:
                    return GDFExchangeDevice.Unknown;

            }
        }

        private IEditorEngine _engine;
        private DeviceConfiguration _configuration;
        private Device _actualDevice;
        private Device _currentDevice;
        private List<Device> _cache = null;

        public event Action<Device> onDeviceChanged;
        public override string Id => Current.Id;
        public override string Name => Current.Name;

        public Device Current
        {
            get
            {
                if (_configuration.Current < 0)
                {
                    return _actualDevice;
                }
                return _configuration.Devices[_configuration.Current];
            }
        }

        public List<Device> Devices
        {
            get
            {
                if (_cache == null)
                {
                    _cache = new List<Device>();
                    _cache.Add(_actualDevice);
                    _cache.AddRange(_configuration.Devices);
                }
                return _cache;
            }
        }

        public override GDFExchangeDevice OS => Current.OS;

        public EditorDeviceManager(IEditorEngine engine)
        {
            _engine = engine;
            _actualDevice = new Device
            {
                Name = SystemInfo.deviceName,
                Id = SystemInfo.deviceUniqueIdentifier,
                OS = CurrentOS()
            };
            
            long reference = _engine.Configuration.Reference;
            _configuration = GDFUserSettings.Instance.LoadOrDefault(new DeviceConfiguration());

            _currentDevice = _configuration.Current < 0 ? _actualDevice : _configuration.Devices[_configuration.Current];
        }

        public void Select(Device device)
        {
            _configuration.Current = _configuration.Devices.IndexOf(device);
            _currentDevice = _configuration.Current < 0 ? _actualDevice : _configuration.Devices[_configuration.Current];
            onDeviceChanged?.Invoke(device);
            Save();
        }

        public Device Add()
        {
            Device device = new Device()
            {
                Name = "New device",
                Id = $"{_actualDevice.Id}-{GDFSecurityTools.HashMd5(GDFRandom.RandomStringBase64(64))}"
            };
            _configuration.Devices.Add(device);
            _cache.Add(device);
            onDeviceChanged?.Invoke(device);
            Save();
            return device;
        }

        public bool Delete(Device device)
        {
            int index = _configuration.Devices.IndexOf(device);
            if (index < 0)
            {
                return false;
            }

            _configuration.Devices.RemoveAt(index);

            if (index == _configuration.Current)
            {
                _currentDevice = _actualDevice;
                _configuration.Current = -1;
            }
            else if (index < _configuration.Current)
            {
                _configuration.Current--;
            }

            _cache.Remove(device);
            onDeviceChanged?.Invoke(Current);
            Save();

            return true;
        }

        public bool IsDefault(Device device)
        {
            return device == _actualDevice;
        }

        public void Save()
        {
            long reference = _engine.Configuration.Reference;
            _configuration.Current = _configuration.Devices.IndexOf(_currentDevice);
            GDFUserSettings.Instance.Save(_configuration);
        }
    }
}