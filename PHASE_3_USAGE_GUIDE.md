# PHASE_3_USAGE_GUIDE.md

## Phase 3: Integration & Manager Usage Guide

Phase 3 is complete with full manager integration, WebSocket event routing, and state synchronization. This guide demonstrates how to use the SDK with the new manager-based API.

---

## Quick Start

### Basic Connection

```csharp
using AlYurr_CrestronNvxSdk;

// Create SDK instance
var sdk = new CrestronNvxSdk(
    ipAddress: "192.168.1.100",
    username: "admin",
    password: "password"
);

// Connect (authenticates HTTP and WebSocket, loads initial state)
await sdk.ConnectAsync();

try
{
    // SDK is now connected and managers are ready to use
}
finally
{
    await sdk.DisconnectAsync();
}
```

---

## Using the DeviceInfo Manager

The `DeviceInfo` manager provides access to device metadata and capabilities.

### Get Device Information

```csharp
// Retrieve device information
var deviceState = await sdk.DeviceInfo.GetAsync();

// Access device properties
Console.WriteLine($"Model: {deviceState.Model}");
Console.WriteLine($"Serial Number: {deviceState.SerialNumber}");
Console.WriteLine($"Device Version: {deviceState.DeviceVersion}");
Console.WriteLine($"MAC Address: {deviceState.MacAddress}");

// Monitor state changes via events
deviceState.StateChanged += (sender, e) =>
{
    Console.WriteLine($"Device state changed: {e.PropertyName}");
};

// Refresh device information from device
await sdk.DeviceInfo.RefreshAsync();
```

### Device Properties Available

- `Model` - Device model name
- `SerialNumber` - Device serial number
- `DeviceVersion` - Firmware version
- `PufVersion` - PUF version
- `DeviceId` - Unique device identifier
- `MacAddress` - Network MAC address
- `Name` - Friendly device name
- `Manufacturer` - Manufacturer name (Crestron)
- `Category` - Device category
- `RebootReason` - Last reboot reason

---

## Using the DeviceCapabilities Manager

The `DeviceCapabilities` manager provides information about device capabilities and port configuration.

### Get Device Capabilities

```csharp
// Retrieve capabilities
var capState = await sdk.DeviceCapabilities.GetAsync();

// Access port counts
var portConfig = capState.Data?.PortConfig;
if (portConfig != null)
{
    Console.WriteLine($"HDMI Outputs: {portConfig.NumberOfHdmiOutputs}");
    Console.WriteLine($"DM Inputs: {portConfig.NumberOfDmInputs}");
    Console.WriteLine($"Audio Inputs: {portConfig.NumberOfAudioInputs}");
}

// Check feature support
var supportsConfigUpload = capState.Data?.IsConfigFileUploadSupported ?? false;
Console.WriteLine($"Config Upload Supported: {supportsConfigUpload}");

// Monitor changes
capState.StateChanged += (sender, e) =>
{
    Console.WriteLine($"Capability changed: {e.PropertyName}");
};
```

### Capabilities Available

- `PortConfig` - Port configuration with input/output counts
- `IsConfigFileUploadSupported` - Whether config files can be uploaded
- `IsLogFileUploadSupported` - Whether log files can be uploaded
- `AncillaryDevicesSupportedCount` - Number of auxiliary devices supported
- `DmSwitchingSupportedCount` - DM switching support count

---

## Using the Audio/Video Manager

The `AudioVideo` manager provides comprehensive control over audio/video inputs and outputs.

### Get Inputs and Outputs

```csharp
// Retrieve all inputs and outputs
var avState = await sdk.AudioVideo.GetAsync();

// Access inputs
foreach (var inputState in avState.Inputs)
{
    Console.WriteLine($"Input: {inputState.Name} (UUID: {inputState.Uuid})");
    
    // Check if endpoint exists
    if (inputState.EndpointExists)
    {
        Console.WriteLine($"  Endpoint ID: {inputState.EndpointId}");
        
        // List ports
        if (inputState.Ports != null)
        {
            foreach (var port in inputState.Ports)
            {
                Console.WriteLine($"  - Port Type: {port.PortType}");
                Console.WriteLine($"    Resolution: {port.HorizontalResolution}x{port.VerticalResolution}@{port.FramesPerSecond}Hz");
                Console.WriteLine($"    Sync: {port.IsSyncDetected}, Source: {port.IsSourceDetected}");
            }
        }
    }
}

// Access outputs similarly
foreach (var outputState in avState._outputStates_internal)
{
    Console.WriteLine($"Output: {outputState.Name} (UUID: {outputState.Uuid})");
    
    if (outputState.Ports != null)
    {
        foreach (var port in outputState.Ports)
        {
            Console.WriteLine($"  - Sink Connected: {port.IsSinkConnected}");
            Console.WriteLine($"    Resolution: {port.Resolution}");
        }
    }
}
```

### Control Audio Output

```csharp
var outputUuid = "output-uuid-here";

// Set volume (-80 to 0 dB)
await sdk.AudioVideo.SetOutputVolumeAsync(outputUuid, volume: -20);

// Mute/unmute
await sdk.AudioVideo.SetOutputMuteAsync(outputUuid, mute: true);
await sdk.AudioVideo.SetOutputMuteAsync(outputUuid, mute: false);
```

### Control Video Output

```csharp
var outputUuid = "output-uuid-here";

// Set resolution
await sdk.AudioVideo.SetOutputResolutionAsync(outputUuid, "1920x1080@60Hz");

// Set color space
await sdk.AudioVideo.SetOutputColorSpaceAsync(outputUuid, "RGB");

// Set HDCP mode (Auto, FollowInput, Always, Never)
await sdk.AudioVideo.SetOutputHdcpAsync(outputUuid, "Auto");
```

### Send CEC Commands

```csharp
var outputUuid = "output-uuid-here";
var cecCommand = "30:42"; // Power On command in hex

await sdk.AudioVideo.SendCecCommandAsync(outputUuid, cecCommand);
```

### Get Specific Input/Output

```csharp
// Get a specific input
var input = await sdk.AudioVideo.GetInputAsync("input-uuid");
if (input != null)
{
    Console.WriteLine($"Input found: {input.Name}");
}

// Get a specific output
var output = await sdk.AudioVideo.GetOutputAsync("output-uuid");
if (output != null)
{
    Console.WriteLine($"Output found: {output.Name}");
}
```

---

## Using the Routing Manager

The `Routing` manager provides audio/video routing control.

### Get Routing Configuration

```csharp
// Retrieve current routing
var routingState = await sdk.Routing.GetAsync();

// Access route control options
var routeControl = routingState.Data?.RouteControl;
if (routeControl != null)
{
    Console.WriteLine($"Route Control Available: {routeControl.RouteControlAvailable}");
}

// List current routes
if (routingState.Data?.Routes != null)
{
    foreach (var route in routingState.Data.Routes)
    {
        Console.WriteLine($"Route ID: {route.Id}");
        Console.WriteLine($"  Audio Source: {route.AudioSource}");
        Console.WriteLine($"  Video Source: {route.VideoSource}");
    }
}
```

### Control Audio/Video Sources

```csharp
var audioSourceUuid = "input-uuid-for-audio";
var videoSourceUuid = "input-uuid-for-video";

// Set audio source
await sdk.Routing.SetAudioSourceAsync(audioSourceUuid);

// Set video source
await sdk.Routing.SetVideoSourceAsync(videoSourceUuid);
```

### Manage Routes (CRUD Operations)

```csharp
// Create a new route
var newRoute = new Models.AvRouting.RouteDto
{
    Name = "Conference Room Route",
    AudioSource = "input-1-uuid",
    VideoSource = "input-1-uuid"
};
await sdk.Routing.CreateRouteAsync(newRoute);

// Update an existing route
var updatedRoute = new Models.AvRouting.RouteDto
{
    Name = "Updated Conference Route",
    AudioSource = "input-2-uuid",
    VideoSource = "input-2-uuid"
};
await sdk.Routing.UpdateRouteAsync("route-id", updatedRoute);

// Delete a route
await sdk.Routing.DeleteRouteAsync("route-id");
```

---

## State Management & Change Notifications

All managers maintain state objects that inherit from `StateBase` and provide change notifications.

### Subscribe to State Changes

```csharp
// Device Info changes
sdk.DeviceInfo.State.StateChanged += (sender, e) =>
{
    Console.WriteLine($"Device property '{e.PropertyName}' changed");
    Console.WriteLine($"  Old: {e.OldValue}");
    Console.WriteLine($"  New: {e.NewValue}");
};

// Audio/Video changes
sdk.AudioVideo.State.StateChanged += (sender, e) =>
{
    Console.WriteLine($"Audio/Video state '{e.PropertyName}' changed");
};

// Routing changes
sdk.Routing.State.StateChanged += (sender, e) =>
{
    Console.WriteLine($"Routing state '{e.PropertyName}' changed");
};
```

### Automatic WebSocket Updates

When the device sends WebSocket updates:
1. The message is received by `IWebSocketService`
2. Message is parsed and routed to the appropriate manager
3. Manager's state object is updated
4. `StateChanged` event is fired with details about what changed
5. Subscribers are notified via event handlers

---

## Error Handling

All manager methods throw `DeviceException` on failure:

```csharp
try
{
    await sdk.DeviceInfo.GetAsync();
}
catch (DeviceException ex)
{
    Console.WriteLine($"Error Code: {ex.ErrorCode}");
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
}
catch (ConnectionException ex)
{
    Console.WriteLine($"Connection failed at {ex.IpAddress}: {ex.Message}");
}
catch (AuthenticationException ex)
{
    Console.WriteLine($"Authentication failed: {ex.Message}");
}
```

---

## Complete Example: Monitoring Device State

```csharp
using AlYurr_CrestronNvxSdk;
using AlYurr_CrestronNvxSdk.Exceptions;

class NvxMonitor
{
    private readonly CrestronNvxSdk _sdk;

    public NvxMonitor(string ipAddress, string username, string password)
    {
        _sdk = new CrestronNvxSdk(ipAddress, username, password);
    }

    public async Task RunAsync()
    {
        try
        {
            // Connect to device
            await _sdk.ConnectAsync();
            Console.WriteLine("Connected to device");

            // Subscribe to changes
            _sdk.DeviceInfo.State.StateChanged += OnDeviceInfoChanged;
            _sdk.AudioVideo.State.StateChanged += OnAudioVideoChanged;
            _sdk.Routing.State.StateChanged += OnRoutingChanged;

            // Load initial data
            var deviceInfo = await _sdk.DeviceInfo.GetAsync();
            Console.WriteLine($"Device: {deviceInfo.Name} ({deviceInfo.Model})");

            var avState = await _sdk.AudioVideo.GetAsync();
            Console.WriteLine($"Inputs: {avState.Inputs.Count}, Outputs: {avState._outputStates_internal.Count}");

            // Keep running and monitoring
            Console.WriteLine("Monitoring device (press Ctrl+C to exit)");
            while (true)
            {
                await Task.Delay(5000);
            }
        }
        catch (AuthenticationException ex)
        {
            Console.Error.WriteLine($"Authentication failed: {ex.Message}");
        }
        catch (ConnectionException ex)
        {
            Console.Error.WriteLine($"Connection failed: {ex.Message}");
        }
        finally
        {
            if (_sdk.IsConnected)
            {
                await _sdk.DisconnectAsync();
            }
        }
    }

    private void OnDeviceInfoChanged(object? sender, State.StateChangedEventArgs e)
    {
        Console.WriteLine($"[Device] {e.PropertyName} changed: {e.OldValue} -> {e.NewValue}");
    }

    private void OnAudioVideoChanged(object? sender, State.StateChangedEventArgs e)
    {
        Console.WriteLine($"[A/V] {e.PropertyName} changed");
    }

    private void OnRoutingChanged(object? sender, State.StateChangedEventArgs e)
    {
        Console.WriteLine($"[Routing] {e.PropertyName} changed");
    }
}

class Program
{
    static async Task Main()
    {
        var monitor = new NvxMonitor("192.168.1.100", "admin", "password");
        await monitor.RunAsync();
    }
}
```

---

## Architecture Summary: Phase 3

### Manager Architecture

```
CrestronNvxSdk (Main SDK)
├── DeviceInfoManager -> DeviceInfoState
├── DeviceCapabilitiesManager -> DeviceCapabilitiesState
├── AvRoutingManager -> AvRoutingState
├── AudioVideoInputOutputManager -> AudioVideoInputOutputState
├── IHttpService (HTTP/REST communication)
└── IWebSocketService (Real-time WebSocket updates)
    └── OnWebSocketMessageReceived
        └── RouteMessageToManagers()
            ├── Updates DeviceInfoState
            ├── Updates DeviceCapabilitiesState
            ├── Updates AvRoutingState
            └── Updates AudioVideoInputOutputState
```

### Message Flow

1. **Initial Connection**
   - SDK.ConnectAsync() authenticates and loads initial state
   - WebSocket is established with event handlers registered

2. **HTTP Operations**
   - Manager methods call IHttpService (GetAsync, PostAsync)
   - Responses are deserialized to DTOs
   - Manager state objects are updated
   - StateChanged events fire

3. **WebSocket Updates**
   - Device sends JSON update via WebSocket
   - OnWebSocketMessageReceived is triggered
   - RouteMessageToManagers parses JSON
   - Appropriate manager states are updated
   - StateChanged events fire for subscribers

---

## Next Steps

Phase 4 would include:
- Unit and integration tests
- Advanced error handling and resilience patterns
- Batch operations for efficiency
- Caching strategies
- Performance optimizations
- Full endpoint coverage documentation
