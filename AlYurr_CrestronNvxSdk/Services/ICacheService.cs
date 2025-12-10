namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Interface for a generic caching service used by the SDK to reduce API calls.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Retrieves a value from the cache.
    /// </summary>
    /// <typeparam name="T">The type of value to retrieve.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <returns>The cached value, or null if not found or expired.</returns>
    T? Get<T>(string key);

    /// <summary>
    /// Stores a value in the cache with an optional expiration duration.
    /// </summary>
    /// <typeparam name="T">The type of value to cache.</typeparam>
    /// <param name="key">The cache key.</param>
    /// <param name="value">The value to cache.</param>
    /// <param name="duration">Optional cache duration. If null, uses default TTL from configuration.</param>
    void Set<T>(string key, T value, TimeSpan? duration = null);

    /// <summary>
    /// Removes a specific key from the cache.
    /// </summary>
    /// <param name="key">The cache key to remove.</param>
    void Remove(string key);

    /// <summary>
    /// Clears all items from the cache.
    /// </summary>
    void Clear();

    /// <summary>
    /// Gets the approximate number of items currently in the cache.
    /// </summary>
    /// <returns>The number of cached items.</returns>
    int GetCacheSize();

    /// <summary>
    /// Gets cache statistics for monitoring and diagnostics.
    /// </summary>
    /// <returns>Cache statistics including hit/miss rates.</returns>
    CacheStatistics GetStatistics();
}

/// <summary>
/// Statistics about cache performance.
/// </summary>
public class CacheStatistics
{
    /// <summary>
    /// Total number of cache hits.
    /// </summary>
    public long TotalHits { get; set; }

    /// <summary>
    /// Total number of cache misses.
    /// </summary>
    public long TotalMisses { get; set; }

    /// <summary>
    /// Current number of items in cache.
    /// </summary>
    public int ItemCount { get; set; }

    /// <summary>
    /// Estimated memory usage in bytes.
    /// </summary>
    public long EstimatedMemoryBytes { get; set; }

    /// <summary>
    /// Cache hit rate as a percentage (0-100).
    /// </summary>
    public double HitRate => TotalHits + TotalMisses > 0 
        ? (TotalHits / (double)(TotalHits + TotalMisses)) * 100 
        : 0;
}
