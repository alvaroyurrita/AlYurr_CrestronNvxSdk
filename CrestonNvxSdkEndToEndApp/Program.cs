// See https://aka.ms/new-console-template for more information
using AlYurr_CrestronNvxSdk;
using Microsoft.Extensions.Configuration;
using Serilog;

var builder = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
var ipAddress = builder["IpAddress"];
var username = builder["NvxUsername"];
var password = builder["NvxPassword"];

var logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

var mvxSDK = new CrestronNvxSdkBuilder()
    .WithDevice(ipAddress, username, password)
    .WithLogger(logger)
    .Build();

await mvxSDK.ConnectAsync();

Console.WriteLine("Connected to NVX device.");

do 
{
    Console.WriteLine("Menu:");
    Console.WriteLine("1. Print Device Info");
    Console.WriteLine("2. Print AudioVideoInputOutput");
    Console.WriteLine("3. Print Device Capabilities");
    Console.WriteLine("Q. Quit");
    Console.Write("Select an option: ");
    var input = Console.ReadLine();
    if (input?.ToUpper() == "Q")
    {
        break;
    }
    switch (input)
    {
        case "1":
            PrintDeviceInfo(mvxSDK);
            break;
        case "2":
            PrintAudioVideoInputOutput(mvxSDK);
            break;
        case "3":
            PrintDeviceCapabilities(mvxSDK);
            break;
        default:
            Console.WriteLine("Invalid choice.");
            break;
    }
    Console.WriteLine("Press any key to continue...");
    var _ = Console.ReadKey();
    Console.Clear();
} while (true);

await mvxSDK.DisconnectAsync();

static void PrintDeviceInfo(CrestronNvxSdk mvxSDK)
{
    Console.WriteLine("-----------Device Info------------");
    Console.WriteLine($"Build Date: {mvxSDK.DeviceInfo.State.BuildDate}");
    Console.WriteLine($"Category: {mvxSDK.DeviceInfo.State.Category}");
    Console.WriteLine($"Device Cert ID: {mvxSDK.DeviceInfo.State.DeviceCertId}");
    Console.WriteLine($"Device ID: {mvxSDK.DeviceInfo.State.DeviceId}");
    Console.WriteLine($"Device Key: {mvxSDK.DeviceInfo.State.DeviceKey}");
    Console.WriteLine($"Device Version: {mvxSDK.DeviceInfo.State.DeviceVersion}");
    Console.WriteLine($"MAC Address: {mvxSDK.DeviceInfo.State.MacAddress}");
    Console.WriteLine($"Manufacturer: {mvxSDK.DeviceInfo.State.Manufacturer}");
    Console.WriteLine($"Model: {mvxSDK.DeviceInfo.State.Model}");
    Console.WriteLine($"Model ID: {mvxSDK.DeviceInfo.State.ModelId}");
    Console.WriteLine($"Model Sub Type: {mvxSDK.DeviceInfo.State.ModelSubType}");
    Console.WriteLine($"Name: {mvxSDK.DeviceInfo.State.Name}");
    Console.WriteLine($"PUF Version: {mvxSDK.DeviceInfo.State.PufVersion}");
    Console.WriteLine($"Reboot Reason: {mvxSDK.DeviceInfo.State.RebootReason}");
    Console.WriteLine($"Serial Number: {mvxSDK.DeviceInfo.State.SerialNumber}");
    Console.WriteLine($"Version: {mvxSDK.DeviceInfo.State.Version}");
}

static void PrintAudioVideoInputOutput(CrestronNvxSdk mvxSDK)
{
    Console.WriteLine();
    Console.WriteLine("-------AudioVideoInputOutput--------");
    Console.WriteLine($"Version: {mvxSDK.AudioVideoInputOutput.State.Version}");
    Console.WriteLine($"Global Config");
    Console.WriteLine($"    Global EDID: {mvxSDK.AudioVideoInputOutput.State.GlobalConfig?.GlobalEdid}");
    Console.WriteLine($"    Global EDID Type: {mvxSDK.AudioVideoInputOutput.State.GlobalConfig?.GlobalEdidType}");
    Console.WriteLine($"Inputs Count: {mvxSDK.AudioVideoInputOutput.State.Inputs.Count}");
    foreach (var input in mvxSDK.AudioVideoInputOutput.State.Inputs)
    {
        Console.WriteLine($"  Input: {input.Name} (UUID: {input.Uuid})");
        Console.WriteLine($"    Endpoint Exists: {input.EndpointExists}");
        Console.WriteLine($"    Endpoint ID: {input.EndpointId}");
        Console.WriteLine($"    Video Port Type Select: {input.VideoPortTypeSelect}");
        Console.WriteLine($"    Version: {input.Version}");
        if (input.Ports != null && input.Ports.Count > 0)
        {
            Console.WriteLine($"    Ports:");
            foreach (var port in input.Ports)
            {
                Console.WriteLine($"      Type: {port.PortType} (UUID: {port.Uuid})");
                Console.WriteLine($"      Audio:");
                Console.WriteLine($"          AudioTypeSelect: {port.Audio?.AudioTypeSelect}");
                Console.WriteLine($"          Mute: {port.Audio?.Mute}");
                Console.WriteLine($"          Volume: {port.Audio?.Volume}");
                Console.WriteLine($"          Bass: {port.Audio?.Bass}");
                Console.WriteLine($"          Treble: {port.Audio?.Treble}");
                Console.WriteLine($"          Digital:");
                Console.WriteLine($"              Channels: {port.Audio?.Digital?.Channels}");
                Console.WriteLine($"              Format: {port.Audio?.Digital?.Format}");
                Console.WriteLine($"      Aspect Ratio: {port.AspectRatio}");
                Console.WriteLine($"      Color Depth: {port.ColorDepth}");
                Console.WriteLine($"      Color Space: {port.ColorSpace}");
                Console.WriteLine($"      EDID:");
                Console.WriteLine($"          Current Edid: {port.Edid?.CurrentEdid}");
                var edids = port.Edid?.EdidList?.Select(edid => $"              Name: {edid.Name}. Type: {edid.Type}") ?? new List<string>();
                Console.WriteLine($"          Supported Edids: \n{string.Join(",\n", edids)}"); 
                Console.WriteLine($"      Frames Per Second: {port.FramesPerSecond}");
                Console.WriteLine($"      Horizontal Resolution: {port.HorizontalResolution}");
                Console.WriteLine($"      HDMI:");
                Console.WriteLine($"          Hdcp Receiver Capability: {port.Hdmi?.HdcpReceiverCapability}");
                Console.WriteLine($"          Hdcp State: {port.Hdmi?.HdcpState}");
                Console.WriteLine($"          Is Cec ErrorDetected: {port.Hdmi?.IsCecErrorDetected}");
                Console.WriteLine($"          Is Source Hdcp Active: {port.Hdmi?.IsSourceHdcpActive}");
                Console.WriteLine($"          Receive Cec Message: {port.Hdmi?.ReceiveCecMessage}");
                Console.WriteLine($"          Name: {port.Hdmi?.Name}");
                Console.WriteLine($"          Receive Cec Message: {port.Hdmi?.ReceiveCecMessage}");
                Console.WriteLine($"          Status 3D: {port.Hdmi?.Status3D}");
                Console.WriteLine($"          Transmit Cec Message: {port.Hdmi?.TransmitCecMessage}");
                Console.WriteLine($"          Unsupported Video Detected: {port.Hdmi?.UnsupportedVideoDetected}");
                Console.WriteLine($"      Is Interlaced Detected: {port.IsInterlacedDetected}");
                Console.WriteLine($"      Is Source Detected: {port.IsSourceDetected}");
                Console.WriteLine($"      Is Sync Detected: {port.IsSyncDetected}");
                Console.WriteLine($"      Port Type: {port.PortType}");
                Console.WriteLine($"      Power:");
                Console.WriteLine($"          DM Input Type: {port.Power?.DmInputType}");
                Console.WriteLine($"      Vertical Resolution: {port.VerticalResolution}");
            }
        }
    }

    Console.WriteLine($"Outputs Count: {mvxSDK.AudioVideoInputOutput.State.Outputs.Count}");
    foreach (var output in mvxSDK.AudioVideoInputOutput.State.Outputs)
    {
        Console.WriteLine($"  Input: {output.Name} (UUID: {output.Uuid})");
        Console.WriteLine($"    Endpoint Exists: {output.EndpointExists}");
        Console.WriteLine($"    Video Port Type Select: {output.VideoPortTypeSelect}");
        if (output.Ports != null && output.Ports.Count > 0)
        {
            Console.WriteLine($"    Ports:");
            foreach (var port in output.Ports)
            {
                Console.WriteLine($"      Type: {port.PortType} (UUID: {port.Uuid})");
                Console.WriteLine($"      Aspect Ratio: {port.AspectRatio}");
                Console.WriteLine($"      Aspect Ratio Mode: {port.AspectRatioMode}");
                Console.WriteLine($"      Audio:");
                Console.WriteLine($"          AudioTypeSelect: {port.Audio?.AudioTypeSelect}");
                Console.WriteLine($"          Mute: {port.Audio?.Mute}");
                Console.WriteLine($"          Volume: {port.Audio?.Volume}");
                Console.WriteLine($"          Bass: {port.Audio?.Bass}");
                Console.WriteLine($"          Treble: {port.Audio?.Treble}");
                Console.WriteLine($"          Digital:");
                Console.WriteLine($"              Channels: {port.Audio?.Digital?.Channels}");
                Console.WriteLine($"              Format: {port.Audio?.Digital?.Format}");
                Console.WriteLine($"      Color Depth: {port.ColorDepth}");
                Console.WriteLine($"      Color Space: {port.ColorSpace}");
                Console.WriteLine($"      Color Space Mode: {port.ColorSpaceMode}");
                Console.WriteLine($"      Frames Per Second: {port.FramesPerSecond}");
                Console.WriteLine($"      HDMI:");
                Console.WriteLine($"          Hdcp Receiver Capability: {port.Hdmi?.DisabledByHdcp}");
                Console.WriteLine($"          Hdcp State: {port.Hdmi?.HdcpState}");
                Console.WriteLine($"          Is Cec ErrorDetected: {port.Hdmi?.HdcpTransmitterMode}");
                Console.WriteLine($"          Is Source Hdcp Active: {port.Hdmi?.IsBlankingDisabled}");
                Console.WriteLine($"          Is Source Hdcp Active: {port.Hdmi?.IsCecInError}");
                Console.WriteLine($"          Is Source Hdcp Active: {port.Hdmi?.IsOutputDisabled}");
                Console.WriteLine($"          Name: {port.Hdmi?.Name}");
                Console.WriteLine($"          Receive Cec Message: {port.Hdmi?.ReceiveCecMessage}");
                Console.WriteLine($"          Receive Cec Message: {port.Hdmi?.Transmitting}");
                Console.WriteLine($"          Transmit Cec Message: {port.Hdmi?.TransmitCecMessage}");
                Console.WriteLine($"          Unsupported Video Detected: {port.Hdmi?.UnsupportedVideoDetected}");
                Console.WriteLine($"      Horizontal Bezel Compensation: {port.HorizontalBezelCompensation}");
                Console.WriteLine($"      Horizontal Resolution: {port.HorizontalResolution}");
                Console.WriteLine($"      Is Interlaced Detected: {port.IsInterlacedDetected}");
                Console.WriteLine($"      Is Sink Connected: {port.IsSinkConnected}");
                Console.WriteLine($"      Is Source Detected: {port.IsSourceDetected}");
                Console.WriteLine($"      Is Video Timeout Enabled: {port.IsVideoTimeoutEnabled}");
                Console.WriteLine($"      Max Color Depth: {port.MaxColorDepth}");
                Console.WriteLine($"      Orientation: {port.Orientation}");
                Console.WriteLine($"      PortType: {port.PortType}");
                Console.WriteLine($"      Resolution: {port.Resolution}");
                Console.WriteLine($"      Resolution Options Name: {port.ResolutionOptionsName}");
                Console.WriteLine($"      Resolution Options Version: {port.ResolutionOptionsVersion}");
                Console.WriteLine($"      Underscan: {port.Underscan}");
                Console.WriteLine($"      Vertical Bezel Compensation: {port.VerticalBezelCompensation}");
                Console.WriteLine($"      Vertical Resolution: {port.VerticalResolution}");
                Console.WriteLine($"      Video Timeout: {port.VideoTimeout}");
            }
        }
    }
}

static void PrintDeviceCapabilities(CrestronNvxSdk mvxSDK)
{
    Console.WriteLine();
    Console.WriteLine("-------Device Capabilities--------");
    Console.WriteLine($"Version: {mvxSDK.DeviceCapabilities.State.Version}");

    Console.WriteLine("Port Config");
    Console.WriteLine($"    Number Of DM Inputs: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfDmInputs}");
    Console.WriteLine($"    Number Of DM Outputs: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfDmOutputs}");
    Console.WriteLine($"    Number Of Ethernet Adapters: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfEthernetAdapters}");
    Console.WriteLine($"    Number Of HDMI Inputs: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfHdmiInputs}");
    Console.WriteLine($"    Number Of HDMI Outputs: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfHdmiOutputs}");
    Console.WriteLine($"    Number Of USB-C Inputs: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfUsbCInputs}");
    Console.WriteLine($"    Number Of USB Device Ports: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfUsbDevicePorts}");
    Console.WriteLine($"    Number Of USB Host Ports: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfUsbHostPorts}");

    Console.WriteLine("Capabilities");
    Console.WriteLine($"    Ancillary Device Feature Supported: {mvxSDK.DeviceCapabilities.State.IsAncillaryDeviceFeatureSupported}");
    Console.WriteLine($"    Ancillary Devices Supported Count: {mvxSDK.DeviceCapabilities.State.AncillaryDevicesSupportedCount}");
    Console.WriteLine($"    CCD Framework Version Supported: {mvxSDK.DeviceCapabilities.State.CCDFrameworkVersionSupported}");
    Console.WriteLine($"    Config File Upload Supported: {mvxSDK.DeviceCapabilities.State.IsConfigFileUploadSupported}");
    Console.WriteLine($"    Device Monitoring Supported: {mvxSDK.DeviceCapabilities.State.IsDeviceMonitoringSupported}");
    Console.WriteLine($"    Feature Enablement Supported: {mvxSDK.DeviceCapabilities.State.IsFeatureEnablementSupported}");
    Console.WriteLine($"    Log File Upload Supported: {mvxSDK.DeviceCapabilities.State.IsLogFileUploadSupported}");
}