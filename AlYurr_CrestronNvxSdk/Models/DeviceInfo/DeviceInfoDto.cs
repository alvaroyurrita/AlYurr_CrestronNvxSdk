using AlYurr_CrestronNvxSdk.State;

namespace AlYurr_CrestronNvxSdk.Models.DeviceInfo;

/// <summary>
/// Data Transfer Object for Device Info response.
/// </summary>
public class DeviceInfoDto
{
    /// <summary>
    /// Gets or sets the device model name.
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Gets or sets the secondary device model name (if applicable).
    /// </summary>
    public string? ModelSubType { get; set; }

    /// <summary>
    /// Gets or sets the device category.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Gets or sets the device manufacturer.
    /// </summary>
    public string? Manufacturer { get; set; }

    /// <summary>
    /// Gets or sets the ID of the device model.
    /// </summary>
    public string? ModelId { get; set; }

    /// <summary>
    /// Gets or sets the unique ID of the device, based on the MAC address.
    /// </summary>
    public string? DeviceId { get; set; }

    /// <summary>
    /// Gets or sets the unique ID provided by a certificate on the device (as a GUID).
    /// </summary>
    public string? DeviceCertId { get; set; }

    /// <summary>
    /// Gets or sets the device serial number.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Gets or sets the device name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the firmware version running on the device.
    /// </summary>
    public string? DeviceVersion { get; set; }

    /// <summary>
    /// Gets or sets the PUF (Package Update File) version of the device.
    /// </summary>
    public string? PufVersion { get; set; }

    /// <summary>
    /// Gets or sets the build date of the device firmware.
    /// </summary>
    public string? BuildDate { get; set; }

    /// <summary>
    /// Gets or sets the unique device key used for generating licenses.
    /// </summary>
    public string? DeviceKey { get; set; }

    /// <summary>
    /// Gets or sets the device MAC address.
    /// </summary>
    public string? MacAddress { get; set; }

    /// <summary>
    /// Gets or sets the reason the device rebooted last.
    /// Possible values: "poweron", "watchdog", "manual", "unknown", "firmware"
    /// </summary>
    public string? RebootReason { get; set; }

    /// <summary>
    /// Gets or sets the object version.
    /// </summary>
    public string? Version { get; set; }
}

/// <summary>
/// State object for Device Info containing the DTO and change tracking.
/// </summary>
public class DeviceInfoState : StateBase
{
    private DeviceInfoDto _data = new();

    /// <summary>
    /// Gets or sets the device information data.
    /// </summary>
    public DeviceInfoDto Data
    {
        get => _data;
        set
        {
            if (_data != value)
            {
                var oldValue = _data;
                _data = value;
                RaiseStateChanged(CreateStateChangedEventArgs("DeviceInfo", "Data", oldValue, value));
            }
        }
    }

    /// <summary>
    /// Gets the device model name.
    /// </summary>
    public string? Model => _data.Model;

    /// <summary>
    /// Gets the secondary device model name.
    /// </summary>
    public string? ModelSubType => _data.ModelSubType;

    /// <summary>
    /// Gets the device category.
    /// </summary>
    public string? Category => _data.Category;

    /// <summary>
    /// Gets the device manufacturer.
    /// </summary>
    public string? Manufacturer => _data.Manufacturer;

    /// <summary>
    /// Gets the device model ID.
    /// </summary>
    public string? ModelId => _data.ModelId;

    /// <summary>
    /// Gets the unique device ID.
    /// </summary>
    public string? DeviceId => _data.DeviceId;

    /// <summary>
    /// Gets the device certificate ID.
    /// </summary>
    public string? DeviceCertId => _data.DeviceCertId;

    /// <summary>
    /// Gets the device serial number.
    /// </summary>
    public string? SerialNumber => _data.SerialNumber;

    /// <summary>
    /// Gets the device name.
    /// </summary>
    public string? Name => _data.Name;

    /// <summary>
    /// Gets the firmware version.
    /// </summary>
    public string? DeviceVersion => _data.DeviceVersion;

    /// <summary>
    /// Gets the PUF version.
    /// </summary>
    public string? PufVersion => _data.PufVersion;

    /// <summary>
    /// Gets the firmware build date.
    /// </summary>
    public string? BuildDate => _data.BuildDate;

    /// <summary>
    /// Gets the device key for licensing.
    /// </summary>
    public string? DeviceKey => _data.DeviceKey;

    /// <summary>
    /// Gets the MAC address.
    /// </summary>
    public string? MacAddress => _data.MacAddress;

    /// <summary>
    /// Gets the reason for the last reboot.
    /// </summary>
    public string? RebootReason => _data.RebootReason;

    /// <summary>
    /// Gets the API version.
    /// </summary>
    public string? Version => _data.Version;
}
