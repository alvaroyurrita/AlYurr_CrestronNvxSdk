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
public partial class CrestribNvxSdk
{
    private readonly string _ipAddress;
    private readonly string _username;
    private readonly string _password;
    private readonly ILogger _logger;
    private readonly IHttpService _httpService;
    private readonly IWebSocketService _webSocketService;
    private readonly CancellationTokenSource _cancellationTokenSource;
    private readonly JsonSerializerOptions _jsonOptions;

    public NvxState NvxState { get; internal set; } = new();

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

    public CrestribNvxSdk(string ipAddress, string username, string password, ILogger? logger = null)
    {
        _ipAddress = ipAddress;
        _username = username;
        _password = password;
        _logger = logger == null
            ? Log.Logger.ForContext<CrestribNvxSdk>()
            : logger.ForContext<CrestribNvxSdk>();
        
        _httpService = new HttpService(_logger);
        _webSocketService = new WebSocketService(_logger);
        _cancellationTokenSource = new CancellationTokenSource();
        _jsonOptions = new JsonSerializerOptions();
        _jsonOptions.Converters.Add(new JsonInt32Converter());
        _jsonOptions.PropertyNameCaseInsensitive = true;
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
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error processing WebSocket message");
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