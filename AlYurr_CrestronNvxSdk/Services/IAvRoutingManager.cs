using AlYurr_CrestronNvxSdk.Models.AvRouting;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Interface for managing audio/video routing operations.
/// </summary>
public interface IAvRoutingManager
{
    /// <summary>
    /// Gets the current routing state.
    /// </summary>
    AvRoutingState State { get; }

    /// <summary>
    /// Retrieves routing configuration asynchronously.
    /// </summary>
    /// <returns>The routing state.</returns>
    /// <exception cref="CrestronNvxSdkException">Thrown when an error occurs during the operation.</exception>
    Task<AvRoutingState> GetAsync();

    /// <summary>
    /// Refreshes routing configuration by polling the device.
    /// </summary>
    /// <returns>The updated routing state.</returns>
    Task<AvRoutingState> RefreshAsync();

    /// <summary>
    /// Sets the audio source for a route.
    /// </summary>
    /// <param name="audioSourceUuid">The UUID of the audio source input.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetAudioSourceAsync(string audioSourceUuid);

    /// <summary>
    /// Sets the video source for a route.
    /// </summary>
    /// <param name="videoSourceUuid">The UUID of the video source input.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetVideoSourceAsync(string videoSourceUuid);

    /// <summary>
    /// Creates a new route.
    /// </summary>
    /// <param name="route">The route to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateRouteAsync(RouteDto route);

    /// <summary>
    /// Updates an existing route.
    /// </summary>
    /// <param name="routeId">The unique identifier of the route to update.</param>
    /// <param name="route">The updated route data.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateRouteAsync(string routeId, RouteDto route);

    /// <summary>
    /// Deletes a route.
    /// </summary>
    /// <param name="routeId">The unique identifier of the route to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteRouteAsync(string routeId);
}
