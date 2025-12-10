using AlYurr_CrestronNvxSdk.Models.DeviceInfo;
using AlYurr_CrestronNvxSdk.Exceptions;
using Microsoft.Extensions.Logging;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Manager for device information operations.
/// </summary>
public class DeviceInfoManager : IDeviceInfoManager
{
    private readonly IHttpService _httpService;
    private readonly ILogger? _logger;
    private readonly DeviceInfoState _state = new();

    /// <summary>
    /// Gets the current device information state.
    /// </summary>
    public DeviceInfoState State => _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceInfoManager"/> class.
    /// </summary>
    /// <param name="httpService">The HTTP service for API communication.</param>
    /// <param name="logger">Optional logger for debugging.</param>
    public DeviceInfoManager(IHttpService httpService, ILogger? logger = null)
    {
        _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
        _logger = logger;
    }

    /// <summary>
    /// Retrieves device information asynchronously.
    /// </summary>
    /// <returns>The device information state.</returns>
    /// <exception cref="CrestronNvxSdkException">Thrown when an error occurs during the operation.</exception>
    public async Task<DeviceInfoState> GetAsync()
    {
        try
        {
            _logger?.LogDebug("Fetching device information");
            var result = await _httpService.GetAsync<DeviceInfoDto>("/api/v1/device/info");
            
            if (result != null)
            {
                _state.Data = result;
                _logger?.LogInformation("Device information retrieved successfully");
            }
            
            return _state;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to fetch device information");
            throw new DeviceException("DEVICE_INFO_ERROR", "Failed to retrieve device information", ex);
        }
    }

    /// <summary>
    /// Refreshes device information by polling the device.
    /// </summary>
    /// <returns>The updated device information state.</returns>
    public async Task<DeviceInfoState> RefreshAsync()
    {
        return await GetAsync();
    }
}
