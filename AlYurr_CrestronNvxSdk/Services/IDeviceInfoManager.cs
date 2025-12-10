using AlYurr_CrestronNvxSdk.Models.DeviceInfo;
using AlYurr_CrestronNvxSdk.Services;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Interface for managing device information operations.
/// </summary>
public interface IDeviceInfoManager
{
    /// <summary>
    /// Gets the current device information state.
    /// </summary>
    DeviceInfoState State { get; }

    /// <summary>
    /// Retrieves device information asynchronously.
    /// </summary>
    /// <returns>The device information state.</returns>
    /// <exception cref="CrestronNvxSdkException">Thrown when an error occurs during the operation.</exception>
    Task<DeviceInfoState> GetAsync();

    /// <summary>
    /// Refreshes device information by polling the device.
    /// </summary>
    /// <returns>The updated device information state.</returns>
    Task<DeviceInfoState> RefreshAsync();
}
