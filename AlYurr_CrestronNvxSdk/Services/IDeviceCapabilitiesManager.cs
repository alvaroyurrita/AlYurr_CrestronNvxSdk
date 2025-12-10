using AlYurr_CrestronNvxSdk.Models.DeviceCapabilities;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Interface for managing device capabilities operations.
/// </summary>
public interface IDeviceCapabilitiesManager
{
    /// <summary>
    /// Gets the current device capabilities state.
    /// </summary>
    DeviceCapabilitiesState State { get; }

    /// <summary>
    /// Retrieves device capabilities asynchronously.
    /// </summary>
    /// <returns>The device capabilities state.</returns>
    /// <exception cref="CrestronNvxSdkException">Thrown when an error occurs during the operation.</exception>
    Task<DeviceCapabilitiesState> GetAsync();

    /// <summary>
    /// Refreshes device capabilities by polling the device.
    /// </summary>
    /// <returns>The updated device capabilities state.</returns>
    Task<DeviceCapabilitiesState> RefreshAsync();
}
