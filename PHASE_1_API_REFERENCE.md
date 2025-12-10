# Phase 1 API Reference

## Quick Start

### 1. Basic Connection

```csharp
// Option A: Using Builder (Recommended)
var sdk = new CrestronNvxSdkBuilder()
    .WithDevice("192.168.1.100", "admin", "password")
    .WithLogger(logger) // Optional
    .WithTimeout(TimeSpan.FromSeconds(30))
    .WithAutoReconnect(true)
    .WithMaxRetries(3)
    .Build();

await sdk.ConnectAsync();

// Option B: Direct Constructor (Legacy)
var sdk = new CrestronNvxSdk("192.168.1.100", "admin", "password");
await sdk.ConnectAsync();
```

### 2. Check Connection Status

```csharp
if (sdk.IsConnected)
{
    Console.WriteLine("Connected to device");
}
```

### 3. Access Raw Device State

```csharp
string rawJson = sdk.NvxState.RawDeviceJson;
```

### 4. Subscribe to State Changes

```csharp
sdk.NvxState.StateChanged += (sender, args) =>
{
    Console.WriteLine($"Category: {args.Category}");
    Console.WriteLine($"Property: {args.PropertyPath}");
    Console.WriteLine($"Old Value: {args.OldValue}");
    Console.WriteLine($"New Value: {args.NewValue}");
};
```

### 5. Graceful Disconnection

```csharp
await sdk.DisconnectAsync();
```

## Service Layer (For Advanced Use)

### IHttpService

```csharp
// Get typed response
var response = await httpService.GetAsync<DeviceInfo>("/Device/DeviceInfo");

// Get raw JSON string
var json = await httpService.GetRawAsync("/Device/DeviceInfo");

// POST with payload
var result = await httpService.PostAsync<object>(
    "/Device/AudioVideoInputOutput/Inputs/0/Ports/0/Audio",
    new { AudioTypeSelect = 1 }
);

// Check authentication
if (httpService.IsAuthenticated)
{
    // Can make requests
}
```

### IWebSocketService

```csharp
// Manual WebSocket control (usually automatic)
var wsService = new WebSocketService();
await wsService.ConnectAsync("192.168.1.100");

// Subscribe to messages
wsService.MessageReceived += (s, e) =>
{
    Console.WriteLine($"Received: {e.Message}");
};

// Send message
await wsService.SendMessageAsync(jsonMessage);

// Check connection state
var state = wsService.State; // WebSocketState enum
```

## Configuration Properties

### CrestronNvxSdk Properties

```csharp
// Get/set timeout for operations
sdk.Timeout = TimeSpan.FromSeconds(60);

// Enable/disable automatic reconnection
sdk.AutoReconnect = true;

// Set maximum reconnection retry attempts
sdk.MaxRetries = 5;

// Check if currently connected
bool isConnected = sdk.IsConnected;
```

## Event Handling

### State Change Events

```csharp
// Fires on any state change
sdk.NvxState.StateChanged += (sender, args) =>
{
    // args.Category: "DeviceInfo", "AudioVideoInputOutput", etc.
    // args.PropertyPath: "Inputs[0].Ports[0].Hdmi.HdcpState"
    // args.OldValue: Previous value
    // args.NewValue: New value
    // args.RawJson: The JSON that was merged
};
```

### WebSocket Events

```csharp
// Create service with events
var wsService = new WebSocketService();

// Connected
wsService.Connected += (s, e) =>
{
    Console.WriteLine("WebSocket connected");
};

// Message received
wsService.MessageReceived += (s, e) =>
{
    Console.WriteLine($"Message: {e.Message}");
};

// Disconnected
wsService.Disconnected += (s, e) =>
{
    Console.WriteLine("WebSocket disconnected");
};
```

## Exception Handling

```csharp
try
{
    await sdk.ConnectAsync();
}
catch (AuthenticationException ex)
{
    Console.WriteLine($"Auth failed on {ex.IpAddress}");
}
catch (ConnectionException ex)
{
    Console.WriteLine($"Connection failed on {ex.IpAddress}");
}
catch (TimeoutException ex)
{
    Console.WriteLine("Operation timed out");
}
catch (ValidationException ex)
{
    Console.WriteLine($"Invalid {ex.PropertyName}: {ex.InvalidValue}");
}
catch (CrestronNvxSdkException ex)
{
    Console.WriteLine($"SDK error: {ex.Message}");
}
```

## Message Parsing (Direct Use)

```csharp
// Parse a complete device state
sdk.FullMessageParser(completeJsonString);

// Parse a partial update
sdk.PartialMessageParser(partialJsonString);
```

## Logging Integration

### Setting Up Serilog

```csharp
// Configure Serilog first
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();

// Now create SDK (will use configured logger)
var sdk = new CrestronNvxSdkBuilder()
    .WithDevice(ip, user, pass)
    .Build();
```

### Custom Logger

```csharp
var customLogger = LoggerBuilder.CreateCustomLogger();
var sdk = new CrestronNvxSdkBuilder()
    .WithDevice(ip, user, pass)
    .WithLogger(customLogger)
    .Build();
```

## Common Patterns

### Pattern 1: Fire and Forget with Logging

```csharp
try
{
    await sdk.ConnectAsync();
    // Use SDK...
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
finally
{
    await sdk.DisconnectAsync();
}
```

### Pattern 2: Monitor State for Specific Changes

```csharp
sdk.NvxState.StateChanged += (s, e) =>
{
    if (e.PropertyPath.Contains("AudioVideoInputOutput"))
    {
        Console.WriteLine("Audio/Video state changed");
    }
    
    if (e.PropertyPath.Contains("Hdmi"))
    {
        Console.WriteLine("HDMI state changed");
    }
};
```

### Pattern 3: Retry Logic

```csharp
int retries = 0;
while (retries < 3)
{
    try
    {
        await sdk.ConnectAsync();
        break;
    }
    catch (ConnectionException)
    {
        retries++;
        if (retries >= 3) throw;
        await Task.Delay(1000);
    }
}
```

### Pattern 4: Connection Monitoring

```csharp
var cts = new CancellationTokenSource();

// Monitor connection in background
Task.Run(async () =>
{
    while (!cts.Token.IsCancellationRequested)
    {
        if (!sdk.IsConnected)
        {
            Console.WriteLine("Connection lost!");
            // Handle disconnection
        }
        await Task.Delay(5000);
    }
}, cts.Token);

// Later...
cts.Cancel();
```

## Troubleshooting

### No Logs Appearing?
```csharp
// Make sure Serilog is configured before creating SDK
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console()
    .CreateLogger();
```

### Connection Fails?
```csharp
try
{
    await sdk.ConnectAsync();
}
catch (AuthenticationException ex)
{
    Console.WriteLine($"Check credentials for {ex.IpAddress}");
}
catch (ConnectionException ex)
{
    Console.WriteLine($"Check IP address: {ex.IpAddress}");
}
```

### WebSocket Not Receiving?
```csharp
// Check IsConnected property
if (!sdk.IsConnected)
{
    // Not connected to WebSocket
}

// Verify events are subscribed before connecting
sdk.NvxState.StateChanged += OnStateChanged;
await sdk.ConnectAsync();
```

## Performance Considerations

### Buffer Size
- WebSocket buffer: 4KB (suitable for incremental updates)
- Increase if receiving large messages: Modify `WebSocketService.cs`

### Timeout
- Default: 30 seconds
- Adjust based on network latency
- Affects HTTP requests, not WebSocket keep-alives

### Reconnection
- Exponential backoff: 1s, 2s, 4s, 8s...
- Disable with `AutoReconnect = false` for manual control
- Check `MaxRetries` to prevent infinite reconnection attempts

---

## What's Next (Phase 2)

Phase 2 will add:
- Strongly-typed models for all API responses
- Manager classes for domain operations
- Easy methods like `sdk.AudioVideoInputOutput.GetInputAsync(0)`
- Full API documentation
