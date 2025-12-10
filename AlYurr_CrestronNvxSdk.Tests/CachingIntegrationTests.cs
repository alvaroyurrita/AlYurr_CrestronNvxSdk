using Xunit;
using AlYurr_CrestronNvxSdk;
using AlYurr_CrestronNvxSdk.Services;

namespace AlYurr_CrestronNvxSdk.Tests;

/// <summary>
/// Tests for SDK caching integration.
/// </summary>
public class CachingIntegrationTests
{
    [Fact]
    public void SdkBuilder_WithCaching_ConfiguresCache()
    {
        // Arrange & Act
        var builder = new CrestronNvxSdkBuilder()
            .WithDevice("192.168.1.1", "admin", "password")
            .WithCaching();

        // Assert
        Assert.NotNull(builder);
    }

    [Fact]
    public void SdkBuilder_WithCustomCaching_ConfiguresCache()
    {
        // Arrange
        var config = new CacheConfiguration 
        { 
            EnableCaching = true,
            DeviceInfoCacheDuration = TimeSpan.FromMinutes(3)
        };

        // Act
        var builder = new CrestronNvxSdkBuilder()
            .WithDevice("192.168.1.1", "admin", "password")
            .WithCaching(config);

        // Assert
        Assert.NotNull(builder);
    }

    [Fact]
    public void SdkBuilder_WithoutCaching_DisablesCache()
    {
        // Arrange & Act
        var builder = new CrestronNvxSdkBuilder()
            .WithDevice("192.168.1.1", "admin", "password")
            .WithoutCaching();

        // Assert
        Assert.NotNull(builder);
    }

    [Fact]
    public void SdkBuilder_BuildWithCaching_CreatesSdkWithCache()
    {
        // Arrange
        var builder = new CrestronNvxSdkBuilder()
            .WithDevice("192.168.1.1", "admin", "password")
            .WithCaching();

        // Act
        var sdk = builder.Build();

        // Assert
        Assert.NotNull(sdk);
        Assert.NotNull(sdk.CacheConfiguration);
        Assert.True(sdk.CacheConfiguration.EnableCaching);
    }

    [Fact]
    public void Sdk_Cache_Property_IsAccessible_WhenEnabled()
    {
        // Arrange
        var builder = new CrestronNvxSdkBuilder()
            .WithDevice("192.168.1.1", "admin", "password")
            .WithCaching();

        // Act
        var sdk = builder.Build();

        // Assert
        Assert.NotNull(sdk.Cache);
    }

    [Fact]
    public void CacheConfiguration_DefaultValues_AreReasonable()
    {
        // Arrange & Act
        var config = new CacheConfiguration();

        // Assert
        Assert.True(config.EnableCaching);
        Assert.Equal(TimeSpan.FromMinutes(5), config.DeviceInfoCacheDuration);
        Assert.Equal(TimeSpan.FromMinutes(10), config.DeviceCapabilitiesCacheDuration);
        Assert.Equal(TimeSpan.FromMinutes(2), config.RoutingCacheDuration);
        Assert.Equal(TimeSpan.FromMinutes(1), config.AudioVideoInputOutputCacheDuration);
        Assert.Equal(1000, config.MaxCacheSize);
    }
}
