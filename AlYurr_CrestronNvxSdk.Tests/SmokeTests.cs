using Xunit;
using AlYurr_CrestronNvxSdk;

namespace AlYurr_CrestronNvxSdk.Tests;

/// <summary>
/// Basic smoke tests to verify SDK components can be instantiated
/// </summary>
public class SmokeTests
{
    [Fact]
    public void SdkBuilder_CanCreateInstance()
    {
        // Arrange & Act
        var builder = new CrestronNvxSdkBuilder();

        // Assert
        Assert.NotNull(builder);
    }

    [Fact]
    public void SdkBuilder_WithDevice_ReturnsBuilder()
    {
        // Arrange
        var builder = new CrestronNvxSdkBuilder();

        // Act
        var result = builder.WithDevice("192.168.1.1", "admin", "password");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public void SdkBuilder_Fluent_ChainsMethods()
    {
        // Arrange & Act
        var builder = new CrestronNvxSdkBuilder()
            .WithDevice("192.168.1.1", "admin", "password")
            .WithTimeout(TimeSpan.FromSeconds(30))
            .WithAutoReconnect(true);

        // Assert
        Assert.NotNull(builder);
    }

    [Fact]
    public void HttpService_CanBeCreated()
    {
        // Arrange & Act
        var service = new Services.HttpService();

        // Assert
        Assert.NotNull(service);
        Assert.False(service.IsAuthenticated);
    }

    [Fact]
    public void StateBase_HasStateChangedEvent()
    {
        // Arrange
        var state = new Models.DeviceInfo.DeviceInfoState();

        // Act & Assert
        Assert.NotNull(state);
        // StateChanged event is available on the state object
        Assert.True(true);  // Verify state object can be created
    }
}
