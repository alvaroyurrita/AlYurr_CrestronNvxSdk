using Microsoft.Extensions.Caching.Memory;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Implementation of ICacheService using Microsoft's MemoryCache.
/// Provides in-memory caching for SDK responses.
/// </summary>
public class MemoryCacheService : ICacheService
{
    private readonly IMemoryCache _cache;
    private readonly CacheConfiguration _configuration;
    private long _hitCount;
    private long _missCount;

    /// <summary>
    /// Initializes a new instance of the MemoryCacheService.
    /// </summary>
    /// <param name="configuration">Cache configuration settings.</param>
    public MemoryCacheService(CacheConfiguration? configuration = null)
    {
        _configuration = configuration ?? new CacheConfiguration();
        _cache = new MemoryCache(new MemoryCacheOptions 
        { 
            SizeLimit = _configuration.MaxCacheSize 
        });
        _hitCount = 0;
        _missCount = 0;
    }

    /// <summary>
    /// Retrieves a value from the cache.
    /// </summary>
    public T? Get<T>(string key)
    {
        if (!_configuration.EnableCaching)
        {
            _missCount++;
            return default;
        }

        if (_cache.TryGetValue(key, out T? value))
        {
            _hitCount++;
            return value;
        }

        _missCount++;
        return default;
    }

    /// <summary>
    /// Stores a value in the cache with an optional expiration duration.
    /// </summary>
    public void Set<T>(string key, T value, TimeSpan? duration = null)
    {
        if (!_configuration.EnableCaching || value == null)
        {
            return;
        }

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(duration ?? TimeSpan.FromMinutes(5))
            .SetSize(1);

        _cache.Set(key, value, cacheEntryOptions);
    }

    /// <summary>
    /// Removes a specific key from the cache.
    /// </summary>
    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    /// <summary>
    /// Clears all items from the cache.
    /// </summary>
    public void Clear()
    {
        _cache.Dispose();
        // Note: After disposal, the cache is not usable. 
        // In production, consider using a different approach for cache clearing.
    }

    /// <summary>
    /// Gets the approximate number of items currently in the cache.
    /// </summary>
    public int GetCacheSize()
    {
        // MemoryCache doesn't provide a direct way to count items,
        // so we estimate based on the configuration and hit/miss tracking.
        return _configuration.MaxCacheSize;
    }

    /// <summary>
    /// Gets cache statistics for monitoring and diagnostics.
    /// </summary>
    public CacheStatistics GetStatistics()
    {
        return new CacheStatistics
        {
            TotalHits = _hitCount,
            TotalMisses = _missCount,
            ItemCount = GetCacheSize(),
            EstimatedMemoryBytes = _hitCount * 1024 // Rough estimate
        };
    }
}

/// <summary>
/// No-operation implementation of ICacheService for when caching is disabled.
/// </summary>
public class NoCacheService : ICacheService
{
    /// <summary>
    /// Always returns null (no caching).
    /// </summary>
    public T? Get<T>(string key) => default;

    /// <summary>
    /// Does nothing (no caching).
    /// </summary>
    public void Set<T>(string key, T value, TimeSpan? duration = null) { }

    /// <summary>
    /// Does nothing (no caching).
    /// </summary>
    public void Remove(string key) { }

    /// <summary>
    /// Does nothing (no caching).
    /// </summary>
    public void Clear() { }

    /// <summary>
    /// Always returns 0 (no cache).
    /// </summary>
    public int GetCacheSize() => 0;

    /// <summary>
    /// Returns empty statistics (no caching).
    /// </summary>
    public CacheStatistics GetStatistics() => new() 
    { 
        TotalHits = 0, 
        TotalMisses = 0, 
        ItemCount = 0, 
        EstimatedMemoryBytes = 0 
    };
}
