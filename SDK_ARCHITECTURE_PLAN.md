# C# SDK Architecture Plan for Crestron NVX API Wrapper

## Executive Summary
This plan outlines how to transform the existing HTTP REST API driver and WebSocket listener into a comprehensive, type-safe C# SDK that provides developers with clean, intuitive access to Crestron NVX device functionality.

---

## 1. ANALYSIS OF EXISTING CODEBASE

### Current State
- **Core Driver** (`CrestronNvxSdk`): Handles HTTP authentication, WebSocket connection, and basic message parsing
- **Connection Management**: Login, state fetching, WebSocket elevation, logout
- **Message Parsing**: Helper functions for JSON merging to handle partial updates
- **Logging**: Integrated Serilog support
- **State Management**: Basic `NvxState` object (currently empty)

### API Structure (from Markdown documentation)
The API is organized into 4 main categories:
1. **DeviceInfo** - Device metadata, firmware version, serial numbers, etc.
2. **DeviceCapabilities** - Port configuration, supported features, device limits
3. **AudioVideoInputOutput** - Input/Output ports, HDMI, audio, EDID, CEC control
4. **AvRouting** - Audio/video routing configuration

### Current Gaps
- No type-safe models for API responses
- No dedicated methods for individual API endpoints
- State updates are generic JSON merges without domain logic
- No event system for state changes
- No validation of input parameters

---

## 2. ARCHITECTURE OVERVIEW

### Layered Architecture

```
┌─────────────────────────────────────────┐
│   SDK Public API Layer                   │
│  (CrestronNvxSdk facade with domains)   │
├─────────────────────────────────────────┤
│   Domain Models & Managers               │
│  (DeviceInfo, AudioVideoIO, AvRouting)  │
├─────────────────────────────────────────┤
│   HTTP & WebSocket Transport Layer       │
│  (Request/Response handling)             │
├─────────────────────────────────────────┤
│   Serialization & JSON Handling          │
│  (Custom converters, JSON merging)       │
├─────────────────────────────────────────┤
│   Authentication & Connection Mgmt       │
│  (Login, cookies, certificate validation)|
└─────────────────────────────────────────┘
```

---

## 3. DATA MODEL GENERATION STRATEGY

### 3.1 Model Hierarchy
Create strongly-typed C# classes from the Markdown API tables and JSON responses:

**Level 1: Root Container**
```
├── Device (root object)
│   ├── DeviceInfo
│   ├── DeviceCapabilities
│   ├── AudioVideoInputOutput
│   └── AvRouting
```

**Level 2: Domain-Specific Models**
For each main category, create:
- **DTOs** (Data Transfer Objects) - Mirror the JSON structure exactly
- **Managers** - Provide type-safe methods for GET/POST operations
- **Event classes** - For WebSocket state change notifications

### 3.2 File Structure for Models

```
AlYurr_CrestronNvxSdk/
├── Models/
│   ├── DeviceInfo/
│   │   ├── DeviceInfoDto.cs (full response model)
│   │   ├── DeviceInfoState.cs (state object with auto-properties)
│   │   └── Enums/ (if any enum values exist)
│   ├── DeviceCapabilities/
│   │   ├── DeviceCapabilitiesDto.cs
│   │   ├── PortConfigDto.cs
│   │   └── DeviceCapabilitiesState.cs
│   ├── AudioVideoInputOutput/
│   │   ├── AudioVideoInputOutputDto.cs
│   │   ├── GlobalConfigDto.cs
│   │   ├── InputDto.cs / PortDto.cs / EdidDto.cs
│   │   ├── OutputDto.cs / HdmiDto.cs / CecControlDto.cs
│   │   ├── AudioDto.cs / DigitalAudioDto.cs
│   │   └── AudioVideoInputOutputState.cs
│   ├── AvRouting/
│   │   ├── AvRoutingDto.cs
│   │   ├── RouteControlDto.cs
│   │   ├── RouteDto.cs
│   │   └── AvRoutingState.cs
│   └── Common/
│       ├── BaseDto.cs (if common patterns exist)
│       └── Enums/ (shared enums)
```

### 3.3 Key Model Characteristics

Each DTO class will:
- **Auto-map from JSON**: Use `System.Text.Json` with custom converters
- **Validation**: Include data annotations for validation rules from API docs
- **Nullability**: Properly mark nullable vs non-nullable fields
- **Properties**: Clean C# properties (no field exposure)
- **Immutability options**: Consider read-only properties for some models

Each State class will:
- Inherit from or use a base interface (e.g., `IStateObject`)
- Support property change notifications (if needed)
- Handle partial updates from WebSocket messages

---

## 4. MANAGER PATTERN FOR DOMAIN OPERATIONS

### 4.1 Manager Classes
Create domain-specific managers that wrap HTTP calls:

**DeviceInfoManager**
```
- GetDeviceInfo() → DeviceInfoState
- RefreshDeviceInfo() → Task<DeviceInfoState>
- Events: DeviceInfoChanged
```

**DeviceCapabilitiesManager**
```
- GetDeviceCapabilities() → DeviceCapabilitiesState
- GetPortConfiguration() → PortConfig
- RefreshCapabilities() → Task<DeviceCapabilitiesState>
- Events: CapabilitiesChanged
```

**AudioVideoInputOutputManager**
```
- GetAudioVideoConfig() → AudioVideoInputOutputState
- GetInputs() → List<InputState>
- GetOutputs() → List<OutputState>
- SetVideoSourceForRoute(routeId, inputId) → Task
- SetAudioSourceForRoute(routeId, inputId) → Task
- SetEdid(inputId, edidName) → Task
- TransmitCecMessage(outputId, message) → Task
- SetVolume(outputId, volume) → Task
- Events: InputConnected, OutputConnected, CecMessageReceived, etc.
```

**AvRoutingManager**
```
- GetRouteControl() → RouteControlState
- GetRoutes() → List<RouteState>
- CreateRoute(name) → Task<RouteState>
- UpdateRoute(routeId, audioSource, videoSource, usbSource) → Task
- DeleteRoute(routeId) → Task
- SetLayer3Enabled(enabled) → Task
- SetUsbFollowsVideo(enabled) → Task
- Events: RouteChanged, RouteCreated, RouteDeleted
```

### 4.2 Manager Registration
Managers are registered in the main SDK class:
```csharp
public class CrestronNvxSdk
{
    public IDeviceInfoManager DeviceInfo { get; }
    public IDeviceCapabilitiesManager DeviceCapabilities { get; }
    public IAudioVideoInputOutputManager AudioVideoInputOutput { get; }
    public IAvRoutingManager AvRouting { get; }
}
```

---

## 5. HTTP REQUEST/RESPONSE HANDLING

### 5.1 HttpService Abstraction
Create a low-level HTTP service that:
- Encapsulates all HTTP communication
- Handles authentication cookies automatically
- Supports both GET and POST operations
- Provides type-safe response deserialization
- Includes retry logic and error handling

**Methods:**
```csharp
Task<T> GetAsync<T>(string path) 
Task PostAsync<T>(string path, object payload)
Task<string> GetRawAsync(string path) // for WebSocket-compatible updates
```

### 5.2 Request Path Building
Paths follow the pattern `/Device/[Category]/[Property]/...`
- `/Device/DeviceInfo`
- `/Device/AudioVideoInputOutput/Inputs`
- `/Device/AudioVideoInputOutput/Inputs/0/Ports/0/Hdmi`
- `/Device/AvRouting/Routes/0`

### 5.3 Response Deserialization
Responses always come wrapped as:
```json
{
  "Device": {
    "CategoryName": { /* actual data */ }
  }
}
```

The SDK automatically unwraps these to expose just the domain object.

---

## 6. STATE MANAGEMENT & WEBSOCKET INTEGRATION

### 6.1 State Synchronization Strategy

**Initial Load:**
1. HTTP GET `/Device` returns full state
2. Parse response and populate all managers' state objects
3. Fire "Initialized" events

**Partial Updates:**
1. WebSocket receives partial JSON (e.g., `{"Device": {"AudioVideoInputOutput": {"Inputs": [...]}}}`)
2. Use existing `JsonMerge` utility to merge with current state
3. Managers detect changes and fire "Changed" events
4. Consumers subscribe to relevant events

### 6.2 WebSocket Message Handling
Enhance the existing `WebSocketListener` to:
- Parse received JSON messages
- Determine which domain was affected (DeviceInfo, AudioVideoInputOutput, etc.)
- Route updates to appropriate manager
- Handle errors and reconnection logic
- Fire domain-specific events

### 6.3 Event System Architecture
```csharp
public interface IStateChanged
{
    event EventHandler<StateChangedEventArgs>? StateChanged;
}

public class StateChangedEventArgs : EventArgs
{
    public string Category { get; } // "DeviceInfo", "AudioVideoInputOutput", etc.
    public string PropertyPath { get; } // "Inputs[0].Ports[0].Hdmi.HdcpState"
    public object? OldValue { get; }
    public object? NewValue { get; }
}
```

---

## 7. API OPERATIONS MAPPING

### 7.1 From Markdown to Method Implementation

For each row in the API documentation:
- **Property Name**: Becomes a C# property on the model
- **Parent Path**: Determines object nesting in the class hierarchy
- **Methods**: Determines if property is GET-only, or supports POST
- **Validation Rules**: Applied as attribute validators or in manager methods
- **Version**: Tracked for compatibility checks

### 7.2 Example: AudioVideoInputOutput

**From Markdown:**
| Property | Path | Version | Type | Methods |
| AudioTypeSelect | /Device/AudioVideoInputOutput/Inputs/x/Ports/x/Audio/ | 2.0.0 | Numeric | GET, POST |

**Generated Code:**
```csharp
// In InputPortAudioDto.cs
public class InputPortAudioDto
{
    [Range(0, 3)]
    public int AudioTypeSelect { get; set; } // 0=Auto, 1=HDMI, 2=Analog, 3=SPDIF
}

// In AudioVideoInputOutputManager.cs
public async Task SetAudioTypeSelectAsync(int inputIndex, int portIndex, int audioTypeSelect)
{
    var path = $"/Device/AudioVideoInputOutput/Inputs/{inputIndex}/Ports/{portIndex}/Audio/AudioTypeSelect";
    await _httpService.PostAsync<object>(path, new { AudioTypeSelect = audioTypeSelect });
}
```

---

## 8. ERROR HANDLING & VALIDATION

### 8.1 Custom Exceptions
```
CrestronNvxSdkException (base)
├── ConnectionException
├── AuthenticationException
├── ValidationException
├── TimeoutException
└── DeviceException (device returns error)
```

### 8.2 Validation
- Input validation on managers before sending HTTP requests
- API constraint validation based on Markdown docs
- Device-specific validation based on capabilities

### 8.3 Async/Await
All I/O operations are async with proper cancellation token support

---

## 9. CONFIGURATION & INITIALIZATION

### 9.1 Builder Pattern
```csharp
var sdk = new CrestronNvxSdkBuilder()
    .WithDevice(ipAddress, username, password)
    .WithLogger(logger)
    .WithTimeout(TimeSpan.FromSeconds(30))
    .WithAutoReconnect(enabled: true)
    .Build();

await sdk.ConnectAsync();
```

### 9.2 Configuration Options
- Connection timeout
- HTTP retry policy
- WebSocket reconnection strategy
- Event subscription mode (auto/manual)

---

## 10. IMPLEMENTATION WORKFLOW

### Phase 1: Foundation (Current Files)
- Enhance existing `CrestronNvxSdk` to use managers
- Create `IHttpService` abstraction
- Create base model classes

### Phase 2: Model Generation (API Documentation → Code)
- Generate all DTOs from API markdown tables
- Create State classes for each domain
- Implement JSON serialization with custom converters

### Phase 3: Manager Implementation
- Implement manager interfaces
- Create HTTP request builders
- Add parameter validation

### Phase 4: WebSocket Integration
- Enhance WebSocket listener
- Implement state merging logic
- Fire domain-specific events

### Phase 5: Testing & Refinement
- Unit tests for managers
- Integration tests with mock NVX devices
- Documentation and examples

---

## 11. USAGE EXAMPLES (Expected)

### Example 1: Get Device Info
```csharp
var sdk = new CrestronNvxSdk(ipAddress, username, password);
await sdk.ConnectAsync();

var deviceInfo = sdk.DeviceInfo.State;
Console.WriteLine($"Device: {deviceInfo.Name}");
Console.WriteLine($"Model: {deviceInfo.Model}");
Console.WriteLine($"Firmware: {deviceInfo.DeviceVersion}");
```

### Example 2: Monitor Audio/Video Routing
```csharp
sdk.AudioVideoInputOutput.StateChanged += (s, e) =>
{
    if (e.PropertyPath.Contains("Inputs"))
        Console.WriteLine($"Input state changed: {e.PropertyPath}");
};

await sdk.AudioVideoInputOutput.SetVideoSourceAsync(routeId: 0, inputId: 0);
```

### Example 3: Control CEC
```csharp
await sdk.AudioVideoInputOutput.TransmitCecMessageAsync(
    outputId: 0,
    message: "01 03 47 00",
    format: CecMessageFormat.Hex
);
```

### Example 4: Configure Routing
```csharp
var routes = sdk.AvRouting.GetRoutes();
foreach (var route in routes)
{
    Console.WriteLine($"Route: {route.Name}");
    Console.WriteLine($"  Audio: {route.AudioSource}");
    Console.WriteLine($"  Video: {route.VideoSource}");
}

await sdk.AvRouting.SetLayer3EnabledAsync(enabled: true);
```

---

## 12. KEY DESIGN PRINCIPLES

1. **Type Safety**: No raw JSON or untyped objects exposed to consumers
2. **Discoverability**: IntelliSense-friendly property and method names
3. **Asynchronicity**: All I/O is async-first
4. **Immutability**: DTOs from JSON are mostly immutable
5. **Testability**: Interfaces for all managers, mockable HTTP layer
6. **Maintainability**: Models auto-generated from docs when possible
7. **Performance**: Efficient JSON parsing, connection pooling
8. **Logging**: Deep visibility into operations through Serilog

---

## Summary

This SDK will transform raw HTTP calls into a developer-friendly C# library where:
- ✅ Every API endpoint is a typed method
- ✅ Every response is a strongly-typed object
- ✅ State changes are discoverable through events
- ✅ Input validation catches errors early
- ✅ WebSocket updates are seamless
- ✅ Complex paths are abstracted away

The implementation follows the existing driver code as a foundation while adding comprehensive abstractions for production use.
