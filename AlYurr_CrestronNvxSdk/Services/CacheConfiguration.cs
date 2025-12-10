namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Configuration for the SDK caching layer.
/// </summary>
public class CacheConfiguration
{
    /// <summary>
    /// Gets or sets whether caching is enabled.
    /// Default: true
    /// </summary>
    public bool EnableCaching { get; set; } = true;

    /// <summary>
    /// Gets or sets the cache duration for device information queries.
    /// Default: 5 minutes
    /// </summary>
    public TimeSpan DeviceInfoCacheDuration { get; set; } = TimeSpan.FromMinutes(5);

    /// <summary>
    /// Gets or sets the cache duration for device capabilities queries.
    /// Default: 10 minutes (capabilities rarely change)
    /// </summary>
    public TimeSpan DeviceCapabilitiesCacheDuration { get; set; } = TimeSpan.FromMinutes(10);

    /// <summary>
    /// Gets or sets the cache duration for routing information.
    /// Default: 2 minutes (routing can change frequently)
    /// </summary>
    public TimeSpan RoutingCacheDuration { get; set; } = TimeSpan.FromMinutes(2);

    /// <summary>
    /// Gets or sets the cache duration for audio/video input/output information.
    /// Default: 1 minute (A/V status changes frequently)
    /// </summary>
    public TimeSpan AudioVideoInputOutputCacheDuration { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Gets or sets the maximum number of items to store in the cache.
    /// Default: 1000
    /// </summary>
    public int MaxCacheSize { get; set; } = 1000;

    /// <summary>
    /// Gets the appropriate cache duration for the given data type.
    /// </summary>
    /// <param name="dataType">The type of data being cached.</param>
    /// <returns>The appropriate cache duration.</returns>
    public TimeSpan GetCacheDuration(CacheDataType dataType) => dataType switch
    {
        CacheDataType.DeviceInfo => DeviceInfoCacheDuration,
        CacheDataType.DeviceCapabilities => DeviceCapabilitiesCacheDuration,
        CacheDataType.Routing => RoutingCacheDuration,
        CacheDataType.AudioVideoInputOutput => AudioVideoInputOutputCacheDuration,
        _ => TimeSpan.FromMinutes(5)
    };
}

/// <summary>
/// Types of data that can be cached.
/// </summary>
public enum CacheDataType
{
    /// <summary>Device information data.</summary>
    DeviceInfo,

    /// <summary>Device capabilities data.</summary>
    DeviceCapabilities,

    /// <summary>Audio/video routing data.</summary>
    Routing,

    /// <summary>Audio/video input/output data.</summary>
    AudioVideoInputOutput
}
