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
    .WithMaxRetries(3)
    .WithLogger(logger)
    .Build();

try
{
    await mvxSDK.ConnectAsync();
}
catch (Exception ex)
{
    logger.Error("Failed to connect to NVX device at {IpAddress}. Exception: {ExceptionMessage}", ipAddress, ex.Message);
    return;
}

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
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.BuildDate)}: {mvxSDK.DeviceInfo.State.BuildDate}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.Category)}: {mvxSDK.DeviceInfo.State.Category}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.DeviceCertId)}: {mvxSDK.DeviceInfo.State.DeviceCertId}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.DeviceId)}: {mvxSDK.DeviceInfo.State.DeviceId}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.DeviceKey)}: {mvxSDK.DeviceInfo.State.DeviceKey}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.DeviceVersion)}: {mvxSDK.DeviceInfo.State.DeviceVersion}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.MacAddress)}: {mvxSDK.DeviceInfo.State.MacAddress}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.Manufacturer)}: {mvxSDK.DeviceInfo.State.Manufacturer}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.Model)}: {mvxSDK.DeviceInfo.State.Model}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.ModelId)}: {mvxSDK.DeviceInfo.State.ModelId}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.ModelSubType)}: {mvxSDK.DeviceInfo.State.ModelSubType}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.Name)}: {mvxSDK.DeviceInfo.State.Name}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.PufVersion)}: {mvxSDK.DeviceInfo.State.PufVersion}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.RebootReason)}: {mvxSDK.DeviceInfo.State.RebootReason}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.SerialNumber)}: {mvxSDK.DeviceInfo.State.SerialNumber}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceInfo.State.Version)}: {mvxSDK.DeviceInfo.State.Version}");
}

static void PrintAudioVideoInputOutput(CrestronNvxSdk mvxSDK)
{
    Console.WriteLine();
    Console.WriteLine("-------AudioVideoInputOutput--------");
    Console.WriteLine($"{nameof(mvxSDK.AudioVideoInputOutput.State.Version)}: {mvxSDK.AudioVideoInputOutput.State.Version}");
    Console.WriteLine($"Global Config");
    Console.WriteLine($"    {nameof(mvxSDK.AudioVideoInputOutput.State.GlobalConfig.GlobalEdid)}: {mvxSDK.AudioVideoInputOutput.State.GlobalConfig?.GlobalEdid}");
    Console.WriteLine($"    {nameof(mvxSDK.AudioVideoInputOutput.State.GlobalConfig.GlobalEdidType)}: {mvxSDK.AudioVideoInputOutput.State.GlobalConfig?.GlobalEdidType}");
    Console.WriteLine($"Inputs {nameof(mvxSDK.AudioVideoInputOutput.State.Inputs)}: {mvxSDK.AudioVideoInputOutput.State.Inputs.Count}");
    foreach (var input in mvxSDK.AudioVideoInputOutput.State.Inputs)
    {
        Console.WriteLine($"  Input: {input.Name} (UUID: {input.Uuid})");
        Console.WriteLine($"    {nameof(input.Name)}: {input.Name}");
        Console.WriteLine($"    {nameof(input.EndpointExists)}: {input.EndpointExists}");
        Console.WriteLine($"    {nameof(input.EndpointId)}: {input.EndpointId}");
        Console.WriteLine($"    {nameof(input.Uuid)}: {input.Uuid}");
        Console.WriteLine($"    {nameof(input.VideoPortTypeSelect)}: {input.VideoPortTypeSelect}");
        Console.WriteLine($"    {nameof(input.Version)}: {input.Version}");
        if (input.Ports != null && input.Ports.Count > 0)
        {
            Console.WriteLine($"    Ports:");
            foreach (var port in input.Ports)
            {
                Console.WriteLine($"      Audio:");
                Console.WriteLine($"          {nameof(port.Audio.AudioTypeSelect)}: {port.Audio?.AudioTypeSelect}");
                Console.WriteLine($"          {nameof(port.Audio.Mute)}: {port.Audio?.Mute}");
                Console.WriteLine($"          {nameof(port.Audio.Volume)}: {port.Audio?.Volume}");
                Console.WriteLine($"          {nameof(port.Audio.Bass)}: {port.Audio?.Bass}");
                Console.WriteLine($"          {nameof(port.Audio.Treble)}: {port.Audio?.Treble}");
                Console.WriteLine($"          Digital:");
                Console.WriteLine($"              {nameof(port.Audio.Digital.Channels)}: {port.Audio?.Digital?.Channels}");
                Console.WriteLine($"              {nameof(port.Audio.Digital.Format)}: {port.Audio?.Digital?.Format}");
                Console.WriteLine($"      {nameof(port.AspectRatio)}: {port.AspectRatio}");
                Console.WriteLine($"      {nameof(port.ColorDepth)}: {port.ColorDepth}");
                Console.WriteLine($"      {nameof(port.ColorSpace)}: {port.ColorSpace}");
                Console.WriteLine($"      EDID:");
                Console.WriteLine($"          {nameof(port.Edid.CurrentEdid)}: {port.Edid?.CurrentEdid}");
                var edids = port.Edid?.EdidList?.Select(edid => $"              {nameof(edid.Name)}: {edid.Name}. {nameof(edid.Type)}: {edid.Type}") ?? new List<string>();
                Console.WriteLine($"          Supported Edids: \n{string.Join(",\n", edids)}");
                Console.WriteLine($"      {nameof(port.FramesPerSecond)}: {port.FramesPerSecond}");
                Console.WriteLine($"      {nameof(port.HorizontalResolution)}: {port.HorizontalResolution}");
                Console.WriteLine($"      HDMI:");
                Console.WriteLine($"          {nameof(port.Hdmi.HdcpReceiverCapability)}: {port.Hdmi?.HdcpReceiverCapability}");
                Console.WriteLine($"          {nameof(port.Hdmi.HdcpState)}: {port.Hdmi?.HdcpState}");
                Console.WriteLine($"          {nameof(port.Hdmi.IsCecErrorDetected)}: {port.Hdmi?.IsCecErrorDetected}");
                Console.WriteLine($"          {nameof(port.Hdmi.IsSourceHdcpActive)}: {port.Hdmi?.IsSourceHdcpActive}");
                Console.WriteLine($"          {nameof(port.Hdmi.ReceiveCecMessage)}: {port.Hdmi?.ReceiveCecMessage}");
                Console.WriteLine($"          {nameof(port.Hdmi.Name)}: {port.Hdmi?.Name}");
                Console.WriteLine($"          {nameof(port.Hdmi.ReceiveCecMessage)}: {port.Hdmi?.ReceiveCecMessage}");
                Console.WriteLine($"          {nameof(port.Hdmi.Status3D)}: {port.Hdmi?.Status3D}");
                Console.WriteLine($"          {nameof(port.Hdmi.TransmitCecMessage)}: {port.Hdmi?.TransmitCecMessage}");
                Console.WriteLine($"          {nameof(port.Hdmi.UnsupportedVideoDetected)}: {port.Hdmi?.UnsupportedVideoDetected}");
                Console.WriteLine($"      {nameof(port.IsInterlacedDetected)}: {port.IsInterlacedDetected}");
                Console.WriteLine($"      {nameof(port.IsSourceDetected)}: {port.IsSourceDetected}");
                Console.WriteLine($"      {nameof(port.IsSyncDetected)}: {port.IsSyncDetected}");
                Console.WriteLine($"      {nameof(port.PortType)}: {port.PortType}");
                Console.WriteLine($"      Power:");
                Console.WriteLine($"          {nameof(port.Power.DmInputType)}: {port.Power?.DmInputType}");
                Console.WriteLine($"      {nameof(port.PortType)}: {port.PortType}");
                Console.WriteLine($"      {nameof(port.Uuid)}: {port.Uuid}");
                Console.WriteLine($"      {nameof(port.VerticalResolution)}: {port.VerticalResolution}");
            }
        }
    }

    Console.WriteLine($"Outputs Count: {mvxSDK.AudioVideoInputOutput.State.Outputs.Count}");
    foreach (var output in mvxSDK.AudioVideoInputOutput.State.Outputs)
    {
        Console.WriteLine($"  Output: {output.Name} (UUID: {output.Uuid})");
        Console.WriteLine($"    {nameof(output.Uuid)}: {output.Uuid}");
        Console.WriteLine($"    {nameof(output.EndpointExists)}: {output.EndpointExists}");
        Console.WriteLine($"    {nameof(output.VideoPortTypeSelect)}: {output.VideoPortTypeSelect}");
        Console.WriteLine($"    {nameof(output.Name)}: {output.Name}");
        if (output.Ports != null && output.Ports.Count > 0)
        {
            Console.WriteLine($"    Ports:");
            foreach (var port in output.Ports)
            {
                Console.WriteLine($"      {nameof(port.PortType)}: {port.PortType} ({nameof(port.Uuid)}: {port.Uuid})");
                Console.WriteLine($"      {nameof(port.AspectRatio)}: {port.AspectRatio}");
                Console.WriteLine($"      {nameof(port.AspectRatioMode)}: {port.AspectRatioMode}");
                Console.WriteLine($"      Audio:");
                Console.WriteLine($"          {nameof(port.Audio.AudioTypeSelect)}: {port.Audio?.AudioTypeSelect}");
                Console.WriteLine($"          {nameof(port.Audio.Mute)}: {port.Audio?.Mute}");
                Console.WriteLine($"          {nameof(port.Audio.Volume)}: {port.Audio?.Volume}");
                Console.WriteLine($"          {nameof(port.Audio.Bass)}: {port.Audio?.Bass}");
                Console.WriteLine($"          {nameof(port.Audio.Treble)}: {port.Audio?.Treble}");
                Console.WriteLine($"          Digital:");
                Console.WriteLine($"              {nameof(port.Audio.Digital.Channels)}: {port.Audio?.Digital?.Channels}");
                Console.WriteLine($"              {nameof(port.Audio.Digital.Format)}: {port.Audio?.Digital?.Format}");
                Console.WriteLine($"      {nameof(port.ColorDepth)}: {port.ColorDepth}");
                Console.WriteLine($"      {nameof(port.ColorSpace)}: {port.ColorSpace}");
                Console.WriteLine($"      {nameof(port.ColorSpaceMode)}: {port.ColorSpaceMode}");
                Console.WriteLine($"      {nameof(port.FramesPerSecond)}: {port.FramesPerSecond}");
                Console.WriteLine($"      HDMI:");
                Console.WriteLine($"          {nameof(port.Hdmi.DisabledByHdcp)}: {port.Hdmi?.DisabledByHdcp}");
                Console.WriteLine($"          {nameof(port.Hdmi.HdcpState)}: {port.Hdmi?.HdcpState}");
                Console.WriteLine($"          {nameof(port.Hdmi.HdcpTransmitterMode)}: {port.Hdmi?.HdcpTransmitterMode}");
                Console.WriteLine($"          {nameof(port.Hdmi.IsBlankingDisabled)}: {port.Hdmi?.IsBlankingDisabled}");
                Console.WriteLine($"          {nameof(port.Hdmi.IsCecInError)}: {port.Hdmi?.IsCecInError}");
                Console.WriteLine($"          {nameof(port.Hdmi.IsOutputDisabled)}: {port.Hdmi?.IsOutputDisabled}");
                Console.WriteLine($"          {nameof(port.Hdmi.Name)}: {port.Hdmi?.Name}");
                Console.WriteLine($"          {nameof(port.Hdmi.ReceiveCecMessage)}: {port.Hdmi?.ReceiveCecMessage}");
                Console.WriteLine($"          {nameof(port.Hdmi.Transmitting)}: {port.Hdmi?.Transmitting}");
                Console.WriteLine($"          {nameof(port.Hdmi.TransmitCecMessage)}: {port.Hdmi?.TransmitCecMessage}");
                Console.WriteLine($"          {nameof(port.Hdmi.UnsupportedVideoDetected)}: {port.Hdmi?.UnsupportedVideoDetected}");
                Console.WriteLine($"      {nameof(port.HorizontalBezelCompensation)}: {port.HorizontalBezelCompensation}");
                Console.WriteLine($"      {nameof(port.HorizontalResolution)}: {port.HorizontalResolution}");
                Console.WriteLine($"      {nameof(port.IsInterlacedDetected)}: {port.IsInterlacedDetected}");
                Console.WriteLine($"      {nameof(port.IsSinkConnected)}: {port.IsSinkConnected}");
                Console.WriteLine($"      {nameof(port.IsSourceDetected)}: {port.IsSourceDetected}");
                Console.WriteLine($"      {nameof(port.IsVideoTimeoutEnabled)}: {port.IsVideoTimeoutEnabled}");
                Console.WriteLine($"      {nameof(port.MaxColorDepth)}: {port.MaxColorDepth}");
                Console.WriteLine($"      {nameof(port.Orientation)}: {port.Orientation}");
                Console.WriteLine($"      {nameof(port.PortType)}: {port.PortType}");
                Console.WriteLine($"      {nameof(port.Resolution)}: {port.Resolution}");
                Console.WriteLine($"      {nameof(port.ResolutionOptionsName)}: {port.ResolutionOptionsName}");
                Console.WriteLine($"      {nameof(port.ResolutionOptionsVersion)}: {port.ResolutionOptionsVersion}");
                Console.WriteLine($"      {nameof(port.Underscan)}: {port.Underscan}");
                Console.WriteLine($"      {nameof(port.Uuid)}: {port.Uuid}");
                Console.WriteLine($"      {nameof(port.VerticalBezelCompensation)}: {port.VerticalBezelCompensation}");
                Console.WriteLine($"      {nameof(port.VerticalResolution)}: {port.VerticalResolution}");
                Console.WriteLine($"      {nameof(port.VideoTimeout)}: {port.VideoTimeout}");
            }
        }
    }
}

static void PrintDeviceCapabilities(CrestronNvxSdk mvxSDK)
{
    Console.WriteLine();
    Console.WriteLine("-------Device Capabilities--------");
    Console.WriteLine($"{nameof(mvxSDK.DeviceCapabilities.State.Version)}: {mvxSDK.DeviceCapabilities.State.Version}");

    Console.WriteLine($"{nameof(mvxSDK.DeviceCapabilities.State.IsAncillaryDeviceFeatureSupported)}: {mvxSDK.DeviceCapabilities.State.IsAncillaryDeviceFeatureSupported}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceCapabilities.State.AncillaryDevicesSupportedCount)}: {mvxSDK.DeviceCapabilities.State.AncillaryDevicesSupportedCount}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceCapabilities.State.CCDFrameworkVersionSupported)}: {mvxSDK.DeviceCapabilities.State.CCDFrameworkVersionSupported}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceCapabilities.State.IsConfigFileUploadSupported)}: {mvxSDK.DeviceCapabilities.State.IsConfigFileUploadSupported}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceCapabilities.State.IsDeviceMonitoringSupported)}: {mvxSDK.DeviceCapabilities.State.IsDeviceMonitoringSupported}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceCapabilities.State.IsFeatureEnablementSupported)}: {mvxSDK.DeviceCapabilities.State.IsFeatureEnablementSupported}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceCapabilities.State.IsLogFileUploadSupported)}: {mvxSDK.DeviceCapabilities.State.IsLogFileUploadSupported}");
    Console.WriteLine($"{nameof(mvxSDK.DeviceCapabilities.State.MonitoringDevicesSupportedCount)}: {mvxSDK.DeviceCapabilities.State.MonitoringDevicesSupportedCount}");

    Console.WriteLine("Port Config");
    Console.WriteLine($"    {nameof(mvxSDK.DeviceCapabilities.State.PortConfig.NumberOfDmInputs)}: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfDmInputs}");
    Console.WriteLine($"    {nameof(mvxSDK.DeviceCapabilities.State.PortConfig.NumberOfDmOutputs)}: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfDmOutputs}");
    Console.WriteLine($"    {nameof(mvxSDK.DeviceCapabilities.State.PortConfig.NumberOfEthernetAdapters)}: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfEthernetAdapters}");
    Console.WriteLine($"    {nameof(mvxSDK.DeviceCapabilities.State.PortConfig.NumberOfHdmiInputs)}: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfHdmiInputs}");
    Console.WriteLine($"    {nameof(mvxSDK.DeviceCapabilities.State.PortConfig.NumberOfHdmiOutputs)}: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfHdmiOutputs}");
    Console.WriteLine($"    {nameof(mvxSDK.DeviceCapabilities.State.PortConfig.NumberOfUsbCInputs)}: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfUsbCInputs}");
    Console.WriteLine($"    {nameof(mvxSDK.DeviceCapabilities.State.PortConfig.NumberOfUsbDevicePorts)}: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfUsbDevicePorts}");
    Console.WriteLine($"    {nameof(mvxSDK.DeviceCapabilities.State.PortConfig.NumberOfUsbHostPorts)}: {mvxSDK.DeviceCapabilities.State.PortConfig?.NumberOfUsbHostPorts}");
}

