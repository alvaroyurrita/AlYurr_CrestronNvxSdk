using AlYurr_CrestronNvxSdk.State;

namespace AlYurr_CrestronNvxSdk.Models.DeviceCapabilities;

/// <summary>
/// Represents the port configuration of a device.
/// </summary>
public class PortConfigDto
{
    /// <summary>
    /// Gets or sets the number of DM inputs.
    /// </summary>
    public short NumberOfDmInputs { get; set; }

    /// <summary>
    /// Gets or sets the number of DM outputs.
    /// </summary>
    public ushort NumberOfDmOutputs { get; set; }

    /// <summary>
    /// Gets or sets the number of Ethernet adapters.
    /// </summary>
    public ushort NumberOfEthernetAdapters { get; set; }

    /// <summary>
    /// Gets or sets the number of HDMI inputs.
    /// </summary>
    public ushort NumberOfHdmiInputs { get; set; }

    /// <summary>
    /// Gets or sets the number of HDMI outputs.
    /// </summary>
    public ushort NumberOfHdmiOutputs { get; set; }

    /// <summary>
    /// Gets or sets the number of USB-C inputs.
    /// </summary>
    public ushort NumberOfUsbCInputs { get; set; }

    /// <summary>
    /// Gets or sets the number of USB device ports.
    /// </summary>
    public ushort NumberOfUsbDevicePorts { get; set; }

    /// <summary>
    /// Gets or sets the number of USB host ports.
    /// </summary>
    public ushort NumberOfUsbHostPorts { get; set; }
}

/// <summary>
/// Data Transfer Object for Device Capabilities response.
/// </summary>
public class DeviceCapabilitiesDto
{
    /// <summary>
    /// Gets or sets the port configuration.
    /// </summary>
    public PortConfigDto? PortConfig { get; set; }

    /// <summary>
    /// Gets or sets whether configuration file upload is supported.
    /// </summary>
    public bool IsConfigFileUploadSupported { get; set; }

    /// <summary>
    /// Gets or sets whether log file upload is supported.
    /// </summary>
    public bool IsLogFileUploadSupported { get; set; }

    /// <summary>
    /// Gets or sets whether ancillary device feature is supported.
    /// </summary>
    public bool IsAncillaryDeviceFeatureSupported { get; set; }

    /// <summary>
    /// Gets or sets the number of ancillary devices supported.
    /// </summary>
    public ushort AncillaryDevicesSupportedCount { get; set; }

    /// <summary>
    /// Gets or sets whether device monitoring is supported.
    /// </summary>
    public bool IsDeviceMonitoringSupported { get; set; }

    /// <summary>
    /// Gets or sets the number of monitored devices supported.
    /// </summary>
    public ushort MonitoringDevicesSupportedCount { get; set; }

    /// <summary>
    /// Gets or sets the maximum CCD Framework version supported.
    /// </summary>
    public string? CCDFrameworkVersionSupported { get; set; }

    /// <summary>
    /// Gets or sets whether feature enablement (new licensing) is supported.
    /// </summary>
    public bool IsFeatureEnablementSupported { get; set; }

    /// <summary>
    /// Gets or sets the API version.
    /// </summary>
    public string? Version { get; set; }
}

/// <summary>
/// State object for Device Capabilities.
/// </summary>
public class DeviceCapabilitiesState : StateBase
{
    private DeviceCapabilitiesDto _data = new();

    /// <summary>
    /// Gets or sets the capabilities data.
    /// </summary>
    public DeviceCapabilitiesDto Data
    {
        get => _data;
        set
        {
            if (_data != value)
            {
                var oldValue = _data;
                _data = value;
                RaiseStateChanged(CreateStateChangedEventArgs("DeviceCapabilities", "Data", oldValue, value));
            }
        }
    }

    /// <summary>
    /// Gets the port configuration.
    /// </summary>
    public PortConfigDto? PortConfig => _data.PortConfig;

    /// <summary>
    /// Gets whether config file upload is supported.
    /// </summary>
    public bool IsConfigFileUploadSupported => _data.IsConfigFileUploadSupported;

    /// <summary>
    /// Gets whether log file upload is supported.
    /// </summary>
    public bool IsLogFileUploadSupported => _data.IsLogFileUploadSupported;

    /// <summary>
    /// Gets whether ancillary devices are supported.
    /// </summary>
    public bool IsAncillaryDeviceFeatureSupported => _data.IsAncillaryDeviceFeatureSupported;

    /// <summary>
    /// Gets the number of ancillary devices supported.
    /// </summary>
    public ushort AncillaryDevicesSupportedCount => _data.AncillaryDevicesSupportedCount;

    /// <summary>
    /// Gets whether device monitoring is supported.
    /// </summary>
    public bool IsDeviceMonitoringSupported => _data.IsDeviceMonitoringSupported;

    /// <summary>
    /// Gets the number of monitored devices supported.
    /// </summary>
    public ushort MonitoringDevicesSupportedCount => _data.MonitoringDevicesSupportedCount;

    /// <summary>
    /// Gets the maximum CCD Framework version supported.
    /// </summary>
    public string? CCDFrameworkVersionSupported => _data.CCDFrameworkVersionSupported;

    /// <summary>
    /// Gets whether feature enablement is supported.
    /// </summary>
    public bool IsFeatureEnablementSupported => _data.IsFeatureEnablementSupported;

    /// <summary>
    /// Gets the API version.
    /// </summary>
    public string? Version => _data.Version;
}
