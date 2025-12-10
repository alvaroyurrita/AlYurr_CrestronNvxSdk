using Xunit;
using AlYurr_CrestronNvxSdk.Services;

namespace AlYurr_CrestronNvxSdk.Tests.Services;

/// <summary>
/// Tests for the caching layer functionality.
/// </summary>
public class CacheServiceTests
{
    [Fact]
    public void MemoryCacheService_CanBeCreated()
    {
        // Arrange & Act
        var cacheService = new MemoryCacheService();

        // Assert
        Assert.NotNull(cacheService);
    }

    [Fact]
    public void MemoryCacheService_WithConfiguration_CanBeCreated()
    {
        // Arrange
        var config = new CacheConfiguration { EnableCaching = true };

        // Act
        var cacheService = new MemoryCacheService(config);

        // Assert
        Assert.NotNull(cacheService);
    }

    [Fact]
    public void CacheService_Set_And_Get_Works()
    {
        // Arrange
        var cacheService = new MemoryCacheService();
        var testValue = "test-data";

        // Act
        cacheService.Set("test-key", testValue);
        var result = cacheService.Get<string>("test-key");

        // Assert
        Assert.NotNull(result);
        Assert.Equal(testValue, result);
    }

    [Fact]
    public void CacheService_Get_ReturnsNull_ForMissingKey()
    {
        // Arrange
        var cacheService = new MemoryCacheService();

        // Act
        var result = cacheService.Get<string>("non-existent-key");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void NoCacheService_AlwaysReturnsNull()
    {
        // Arrange
        var noCacheService = new NoCacheService();
        noCacheService.Set("test-key", "test-value");

        // Act
        var result = noCacheService.Get<string>("test-key");

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void CacheConfiguration_GetCacheDuration_ReturnsCorrectDuration()
    {
        // Arrange
        var config = new CacheConfiguration();

        // Act
        var deviceInfoDuration = config.GetCacheDuration(CacheDataType.DeviceInfo);
        var capabilitiesDuration = config.GetCacheDuration(CacheDataType.DeviceCapabilities);
        var routingDuration = config.GetCacheDuration(CacheDataType.Routing);
        var audioVideoDuration = config.GetCacheDuration(CacheDataType.AudioVideoInputOutput);

        // Assert
        Assert.Equal(TimeSpan.FromMinutes(5), deviceInfoDuration);
        Assert.Equal(TimeSpan.FromMinutes(10), capabilitiesDuration);
        Assert.Equal(TimeSpan.FromMinutes(2), routingDuration);
        Assert.Equal(TimeSpan.FromMinutes(1), audioVideoDuration);
    }

    [Fact]
    public void CacheService_GetStatistics_ReturnsValidStats()
    {
        // Arrange
        var cacheService = new MemoryCacheService();

        // Act
        var stats = cacheService.GetStatistics();

        // Assert
        Assert.NotNull(stats);
        Assert.Equal(0, stats.TotalHits);
        Assert.Equal(0, stats.TotalMisses);
    }
}
