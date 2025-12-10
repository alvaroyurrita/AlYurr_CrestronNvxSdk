using AlYurr_CrestronNvxSdk.Models.DeviceCapabilities;
using AlYurr_CrestronNvxSdk.Exceptions;
using Microsoft.Extensions.Logging;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Manager for device capabilities operations.
/// </summary>
public class DeviceCapabilitiesManager : IDeviceCapabilitiesManager
{
    private readonly IHttpService _httpService;
    private readonly ILogger? _logger;
    private readonly DeviceCapabilitiesState _state = new();

    /// <summary>
    /// Gets the current device capabilities state.
    /// </summary>
    public DeviceCapabilitiesState State => _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="DeviceCapabilitiesManager"/> class.
    /// </summary>
    /// <param name="httpService">The HTTP service for API communication.</param>
    /// <param name="logger">Optional logger for debugging.</param>
    public DeviceCapabilitiesManager(IHttpService httpService, ILogger? logger = null)
    {
        _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
        _logger = logger;
    }

    /// <summary>
    /// Retrieves device capabilities asynchronously.
    /// </summary>
    /// <returns>The device capabilities state.</returns>
    /// <exception cref="CrestronNvxSdkException">Thrown when an error occurs during the operation.</exception>
    public async Task<DeviceCapabilitiesState> GetAsync()
    {
        try
        {
            _logger?.LogDebug("Fetching device capabilities");
            var result = await _httpService.GetAsync<DeviceCapabilitiesDto>("/api/v1/device/capabilities");
            
            if (result != null)
            {
                _state.Data = result;
                _logger?.LogInformation("Device capabilities retrieved successfully");
            }
            
            return _state;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to fetch device capabilities");
            throw new DeviceException("CAPABILITIES_ERROR", "Failed to retrieve device capabilities", ex);
        }
    }

    /// <summary>
    /// Refreshes device capabilities by polling the device.
    /// </summary>
    /// <returns>The updated device capabilities state.</returns>
    public async Task<DeviceCapabilitiesState> RefreshAsync()
    {
        return await GetAsync();
    }
}
