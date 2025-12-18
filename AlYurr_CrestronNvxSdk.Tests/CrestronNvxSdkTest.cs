using AlYurr_CrestronNvxSdk.State;
using Xunit;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.XUnit;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace AlYurr_CrestronNvxSdk.Tests;
public class CrestronNvxSdkTest
{
    private readonly ILogger _logger;

    // Loaded from user secrets: "NvxUsername" and "NvxPassword"
    private readonly string _nvxUsername;
    private readonly string _nvxPassword;

    public CrestronNvxSdkTest(ITestOutputHelper output)
    {
        _logger = new LoggerConfiguration()
        .WriteTo.TestOutput(output, outputTemplate:
            "[{SourceContext} - {Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
        .MinimumLevel.Verbose()
        .CreateLogger();

        var config = new ConfigurationBuilder()
            .AddUserSecrets<CrestronNvxSdkTest>()
            .Build();

        _nvxUsername = config["NvxUsername"] ?? string.Empty;
        _nvxPassword = config["NvxPassword"] ?? string.Empty;
    }

    [Fact]
    public async Task TestNvxConnect()
    {
        var driver = new CrestronNvxSdkBuilder()
            .WithDevice("192.168.0.147",_nvxUsername,_nvxPassword)
            .Build();
        await driver.ConnectAsync();
        await driver.DisconnectAsync();
        Assert.Equal("DM-NVX-350-00107FA18B57", driver.DeviceInfo.State.Name);
        Assert.True(driver.DeviceCapabilities.State.IsLogFileUploadSupported);
        Assert.Equal(2,driver.AudioVideoInputOutput.State.Inputs.Count);
        Assert.Equal(1, driver.AudioVideoInputOutput.State.Outputs.Count);
        Assert.Equal("System", driver.AudioVideoInputOutput.State.GlobalConfig.GlobalEdidType);
    }
    //[Fact]
    //public async Task TestPreviewMessageEvent()
    //{
    //    var driver = new CrestronNvxSdk("192.168.0.147", _nvxUsername, _nvxPassword, _logger);
    //    await driver.Connect();
    //    await driver.SetPreviewImageState(true);
    //    await Task.Delay(2000);
    //    await driver.SetPreviewImageState(false);
    //    var result = await HelperFunctions.ReceiveDataWithTimeout<Preview>(driver);
    //    Assert.That(result.ImageList["Image1"].Name, Is.EqualTo(""));
    //    await driver.Disconnect();
    //}
    //[Fact]
    //public async Task TestStreamSetupEvent()
    //{
    //    var driver = new CrestronNvxSdk("192.168.0.106", _nvxUsername, _nvxPassword, _logger);
    //    await driver.Connect();
    //    await Task.Delay(1000);
    //    await driver.SetStreamLocation("");
    //    await Task.Delay(2000);
    //    await driver.SetStreamLocation("rtsp://192.168.0.147:554/live.sdp");
    //    var result = await HelperFunctions.ReceiveDataWithTimeout<StreamReceive>(driver);
    //    Assert.Multiple(() =>
    //    {
    //        Assert.That(result, Is.Not.Null);
    //        Assert.That(result.Streams[0].StreamLocation, Is.EqualTo("rtsp://192.168.0.147:554/live.sdp"));
    //    });
    //    await driver.Disconnect();
    //}
    //[Fact]
    //public async Task TestGetPreviewImage()
    //{
    //    var driver = new CrestronNvxSdk("192.168.0.147", _nvxUsername, _nvxPassword, _logger);
    //    await driver.Connect();
    //    var image = await driver.GetPreviewImage(PreviewImageSize.Small);
    //    Assert.That(image.Length, Is.GreaterThan(0));
    //    await driver.Disconnect();
    //}
}