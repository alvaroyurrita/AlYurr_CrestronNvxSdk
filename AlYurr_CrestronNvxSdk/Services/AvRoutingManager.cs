using AlYurr_CrestronNvxSdk.Models.AvRouting;
using AlYurr_CrestronNvxSdk.Exceptions;
using Microsoft.Extensions.Logging;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Manager for audio/video routing operations.
/// </summary>
public class AvRoutingManager : IAvRoutingManager
{
    private readonly IHttpService _httpService;
    private readonly ILogger? _logger;
    private readonly AvRoutingState _state = new();

    /// <summary>
    /// Gets the current routing state.
    /// </summary>
    public AvRoutingState State => _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="AvRoutingManager"/> class.
    /// </summary>
    /// <param name="httpService">The HTTP service for API communication.</param>
    /// <param name="logger">Optional logger for debugging.</param>
    public AvRoutingManager(IHttpService httpService, ILogger? logger = null)
    {
        _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
        _logger = logger;
    }

    /// <summary>
    /// Retrieves routing configuration asynchronously.
    /// </summary>
    /// <returns>The routing state.</returns>
    public async Task<AvRoutingState> GetAsync()
    {
        try
        {
            _logger?.LogDebug("Fetching routing configuration");
            var result = await _httpService.GetAsync<AvRoutingDto>("/api/v1/routing");
            
            if (result != null)
            {
                _state.Data = result;
                _logger?.LogInformation("Routing configuration retrieved successfully");
            }
            
            return _state;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to fetch routing configuration");
            throw new DeviceException("ROUTING_ERROR", "Failed to retrieve routing configuration", ex);
        }
    }

    /// <summary>
    /// Refreshes routing configuration by polling the device.
    /// </summary>
    /// <returns>The updated routing state.</returns>
    public async Task<AvRoutingState> RefreshAsync()
    {
        return await GetAsync();
    }

    /// <summary>
    /// Sets the audio source for a route.
    /// </summary>
    /// <param name="audioSourceUuid">The UUID of the audio source input.</param>
    public async Task SetAudioSourceAsync(string audioSourceUuid)
    {
        try
        {
            _logger?.LogDebug("Setting audio source to {AudioSourceUuid}", audioSourceUuid);
            
            var payload = new { audioSourceUuid };
            await _httpService.PostAsync<object>("/api/v1/routing/audioSource", payload);
            
            _logger?.LogInformation("Audio source set successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to set audio source");
            throw new DeviceException("ROUTING_ERROR", "Failed to set audio source", ex);
        }
    }

    /// <summary>
    /// Sets the video source for a route.
    /// </summary>
    /// <param name="videoSourceUuid">The UUID of the video source input.</param>
    public async Task SetVideoSourceAsync(string videoSourceUuid)
    {
        try
        {
            _logger?.LogDebug("Setting video source to {VideoSourceUuid}", videoSourceUuid);
            
            var payload = new { videoSourceUuid };
            await _httpService.PostAsync<object>("/api/v1/routing/videoSource", payload);
            
            _logger?.LogInformation("Video source set successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to set video source");
            throw new DeviceException("ROUTING_ERROR", "Failed to set video source", ex);
        }
    }

    /// <summary>
    /// Creates a new route.
    /// </summary>
    /// <param name="route">The route to create.</param>
    public async Task CreateRouteAsync(RouteDto route)
    {
        try
        {
            if (route == null)
                throw new ArgumentNullException(nameof(route));
            
            _logger?.LogDebug("Creating route");
            await _httpService.PostAsync<object>("/api/v1/routing/routes", route);
            
            _logger?.LogInformation("Route created successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to create route");
            throw new DeviceException("ROUTING_ERROR", "Failed to create route", ex);
        }
    }

    /// <summary>
    /// Updates an existing route.
    /// </summary>
    /// <param name="routeId">The unique identifier of the route to update.</param>
    /// <param name="route">The updated route data.</param>
    public async Task UpdateRouteAsync(string routeId, RouteDto route)
    {
        try
        {
            if (string.IsNullOrEmpty(routeId))
                throw new ArgumentNullException(nameof(routeId));
            if (route == null)
                throw new ArgumentNullException(nameof(route));
            
            _logger?.LogDebug("Updating route {RouteId}", routeId);
            await _httpService.PostAsync<object>($"/api/v1/routing/routes/{routeId}", route);
            
            _logger?.LogInformation("Route updated successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to update route {RouteId}", routeId);
            throw new DeviceException("ROUTING_ERROR", "Failed to update route", ex);
        }
    }

    /// <summary>
    /// Deletes a route.
    /// </summary>
    /// <param name="routeId">The unique identifier of the route to delete.</param>
    public async Task DeleteRouteAsync(string routeId)
    {
        try
        {
            if (string.IsNullOrEmpty(routeId))
                throw new ArgumentNullException(nameof(routeId));
            
            _logger?.LogDebug("Deleting route {RouteId}", routeId);
            await _httpService.PostAsync<object>($"/api/v1/routing/routes/{routeId}/delete", new { });
            
            _logger?.LogInformation("Route deleted successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to delete route {RouteId}", routeId);
            throw new DeviceException("ROUTING_ERROR", "Failed to delete route", ex);
        }
    }
}
