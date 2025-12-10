using System.Net;
using System.Net.WebSockets;
using System.Text.Json;
using AlYurr_CrestronNvxSdk.Exceptions;
using AlYurr_CrestronNvxSdk.HelperFunctions;
using AlYurr_CrestronNvxSdk.Services;
using AlYurr_CrestronNvxSdk.State;
using Serilog;

namespace AlYurr_CrestronNvxSdk;

/// <summary>
/// Main SDK class for communicating with Crestron NVX devices.
/// </summary>
public partial class CrestronNvxSdk
{
    private readonly string _ipAddress;
    private readonly string _username;
    private readonly string _password;
    private readonly ILogger _logger;
    private readonly IHttpService _httpService;
    private readonly IWebSocketService _webSocketService;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly JsonSerializerOptions _jsonOptions;

    private IDeviceInfoManager? _deviceInfoManager;
    private IDeviceCapabilitiesManager? _deviceCapabilitiesManager;
    private IAvRoutingManager? _avRoutingManager;
    private IAudioVideoInputOutputManager? _audioVideoInputOutputManager;
    private ICacheService? _cacheService;
    private IBatchManager? _batchManager;

    public NvxState NvxState { get; internal set; } = new();

    /// <summary>
    /// Gets or sets the cache configuration.
    /// </summary>
    public CacheConfiguration? CacheConfiguration { get; set; }

    /// <summary>
    /// Gets the device information manager.
    /// </summary>
    public IDeviceInfoManager DeviceInfo => _deviceInfoManager!;

    /// <summary>
    /// Gets the device capabilities manager.
    /// </summary>
    public IDeviceCapabilitiesManager DeviceCapabilities => _deviceCapabilitiesManager!;

    /// <summary>
    /// Gets the routing manager.
    /// </summary>
    public IAvRoutingManager Routing => _avRoutingManager!;

    /// <summary>
    /// Gets the audio/video input/output manager.
    /// </summary>
    public IAudioVideoInputOutputManager AudioVideo => _audioVideoInputOutputManager!;

    /// <summary>
    /// Gets the cache service (if enabled).
    /// </summary>
    public ICacheService? Cache => _cacheService;

    /// <summary>
    /// Gets the batch operation manager for executing multiple operations.
    /// </summary>
    public IBatchManager Batch => _batchManager!;

    /// <summary>
    /// Gets or sets the connection timeout.
    /// </summary>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    /// <summary>
    /// Gets or sets whether to automatically reconnect on disconnection.
    /// </summary>
    public bool AutoReconnect { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum number of connection retries.
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Gets a value indicating whether the SDK is currently connected.
    /// </summary>
    public bool IsConnected => _webSocketService.State == WebSocketState.Open;

    public CrestronNvxSdk(string ipAddress, string username, string password, ILogger? logger = null)
    {
        _ipAddress = ipAddress;
        _username = username;
        _password = password;
        _logger = logger == null
            ? Log.Logger.ForContext<CrestronNvxSdk>()
            : logger.ForContext<CrestronNvxSdk>();
        
        _httpService = new HttpService(_logger);
        _webSocketService = new WebSocketService(_logger);
        _cancellationTokenSource = new CancellationTokenSource();
        _jsonOptions = new JsonSerializerOptions();
        _jsonOptions.Converters.Add(new JsonInt32Converter());
        _jsonOptions.PropertyNameCaseInsensitive = true;

        // Initialize managers
        InitializeManagers();
    }

    private void InitializeManagers()
    {
        // Initialize cache service based on configuration
        _cacheService = CacheConfiguration != null 
            ? new MemoryCacheService(CacheConfiguration)
            : new NoCacheService();

        _deviceInfoManager = new DeviceInfoManager(_httpService, null);
        _deviceCapabilitiesManager = new DeviceCapabilitiesManager(_httpService, null);
        _avRoutingManager = new AvRoutingManager(_httpService, null);
        _audioVideoInputOutputManager = new AudioVideoInputOutputManager(_httpService, null);
        _batchManager = new BatchManager(_audioVideoInputOutputManager, _avRoutingManager, _logger);
    }
    /// <summary>
    /// Connects to the NVX device and initializes the state.
    /// </summary>
    public async Task ConnectAsync()
    {
        try
        {
            _logger.Information("Connecting to NVX at {IpAddress}", _ipAddress);

            // Authenticate HTTP
            var authenticated = await _httpService.AuthenticateAsync(
                _ipAddress, _username, _password, _cancellationTokenSource.Token);
            
            if (!authenticated)
            {
                throw new AuthenticationException(_ipAddress, 
                    $"Failed to authenticate with NVX device at {_ipAddress}");
            }

            // Load initial state
            _logger.Debug("Loading initial state from NVX at {IpAddress}", _ipAddress);
            var deviceState = await _httpService.GetRawAsync("/Device", _cancellationTokenSource.Token);
            NvxState.RawDeviceJson = deviceState;
            _logger.Debug("Initial state loaded successfully");

            // Get cookies for WebSocket
            var httpClient = _httpService.GetHttpClient();
            var handler = httpClient.GetHttpClientHandler();
            if (handler?.CookieContainer is CookieContainer cookies)
            {
                ((WebSocketService)_webSocketService).SetCookies(cookies);
            }

            // Connect WebSocket
            await _webSocketService.ConnectAsync(_ipAddress, _cancellationTokenSource.Token);

            // Setup event handlers
            _webSocketService.MessageReceived += OnWebSocketMessageReceived;
            _webSocketService.Disconnected += OnWebSocketDisconnected;

            // Start listening for messages
            Task.Run(async () => await _webSocketService.StartListeningAsync(_cancellationTokenSource.Token), 
                _cancellationTokenSource.Token);

            _logger.Information("Successfully connected to NVX at {IpAddress}", _ipAddress);
        }
        catch (AuthenticationException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new ConnectionException(_ipAddress, 
                $"Failed to connect to NVX device at {_ipAddress}", ex);
        }
    }

    /// <summary>
    /// Disconnects from the NVX device.
    /// </summary>
    public async Task DisconnectAsync()
    {
        try
        {
            _logger.Information("Disconnecting from NVX at {IpAddress}", _ipAddress);

            // Unsubscribe from events
            _webSocketService.MessageReceived -= OnWebSocketMessageReceived;
            _webSocketService.Disconnected -= OnWebSocketDisconnected;

            // Disconnect WebSocket
            await _webSocketService.DisconnectAsync(_cancellationTokenSource.Token);

            // Logout HTTP
            await _httpService.LogoutAsync(_cancellationTokenSource.Token);

            _logger.Information("Successfully disconnected from NVX at {IpAddress}", _ipAddress);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error during disconnection from NVX at {IpAddress}", _ipAddress);
            throw;
        }
    }

    private void OnWebSocketMessageReceived(object? sender, WebSocketMessageReceivedEventArgs e)
    {
        try
        {
            _logger.Debug("Processing WebSocket message update");
            NvxState.MergeJson(e.Message);
            
            // Route messages to appropriate managers
            RouteMessageToManagers(e.Message);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error processing WebSocket message");
        }
    }

    private void RouteMessageToManagers(string jsonMessage)
    {
        try
        {
            using var doc = JsonDocument.Parse(jsonMessage);
            var root = doc.RootElement;

            // Route to DeviceInfo manager
            if (root.TryGetProperty("Device", out var deviceElement))
            {
                if (_deviceInfoManager is DeviceInfoManager dimgr)
                {
                    var deviceJson = deviceElement.GetRawText();
                    var deviceInfo = JsonSerializer.Deserialize<Models.DeviceInfo.DeviceInfoDto>(deviceJson, _jsonOptions);
                    if (deviceInfo != null)
                    {
                        dimgr.State.Data = deviceInfo;
                    }
                }
            }

            // Route to DeviceCapabilities manager
            if (root.TryGetProperty("Capabilities", out var capElement))
            {
                if (_deviceCapabilitiesManager is DeviceCapabilitiesManager dcmgr)
                {
                    var capJson = capElement.GetRawText();
                    var capabilities = JsonSerializer.Deserialize<Models.DeviceCapabilities.DeviceCapabilitiesDto>(capJson, _jsonOptions);
                    if (capabilities != null)
                    {
                        dcmgr.State.Data = capabilities;
                    }
                }
            }

            // Route to AvRouting manager
            if (root.TryGetProperty("Routing", out var routingElement))
            {
                if (_avRoutingManager is AvRoutingManager amgr)
                {
                    var routingJson = routingElement.GetRawText();
                    var routing = JsonSerializer.Deserialize<Models.AvRouting.AvRoutingDto>(routingJson, _jsonOptions);
                    if (routing != null)
                    {
                        amgr.State.Data = routing;
                    }
                }
            }

            // Route to AudioVideoInputOutput manager
            if (root.TryGetProperty("AudioVideoInputOutput", out var avElement))
            {
                if (_audioVideoInputOutputManager is AudioVideoInputOutputManager avmgr)
                {
                    var avJson = avElement.GetRawText();
                    var audioVideo = JsonSerializer.Deserialize<Models.AudioVideoInputOutput.AudioVideoInputOutputDto>(avJson, _jsonOptions);
                    if (audioVideo != null)
                    {
                        avmgr.State.Data = audioVideo;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            _logger.Warning(ex, "Error routing message to managers");
        }
    }

    private void OnWebSocketDisconnected(object? sender, EventArgs e)
    {
        _logger.Warning("WebSocket disconnected from NVX at {IpAddress}", _ipAddress);
        
        if (AutoReconnect)
        {
            _logger.Information("Attempting to reconnect to NVX at {IpAddress}", _ipAddress);
            Task.Run(ReconnectAsync);
        }
    }

    private async Task ReconnectAsync()
    {
        int retryCount = 0;
        while (retryCount < MaxRetries)
        {
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, retryCount)), _cancellationTokenSource.Token);
                await ConnectAsync();
                _logger.Information("Successfully reconnected to NVX at {IpAddress}", _ipAddress);
                return;
            }
            catch (Exception ex)
            {
                retryCount++;
                _logger.Warning(ex, "Reconnection attempt {Attempt} of {MaxRetries} failed for NVX at {IpAddress}", 
                    retryCount, MaxRetries, _ipAddress);
            }
        }

        _logger.Error("Failed to reconnect to NVX at {IpAddress} after {MaxRetries} attempts", 
            _ipAddress, MaxRetries);
    }
}