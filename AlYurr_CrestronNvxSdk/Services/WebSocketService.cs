using System.Net;
using System.Net.WebSockets;
using System.Text;
using Serilog;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Implementation of IWebSocketService for WebSocket communication with NVX devices.
/// </summary>
public class WebSocketService : IWebSocketService
{
    private ClientWebSocket? _ws;
    private readonly ILogger _logger;
    private string _ipAddress = string.Empty;
    private CancellationTokenSource? _listeningCancellation;
    private CookieContainer? _cookies;

    public WebSocketState State => _ws?.State ?? WebSocketState.Closed;

    public event EventHandler<WebSocketMessageReceivedEventArgs>? MessageReceived;
    public event EventHandler? Connected;
    public event EventHandler? Disconnected;

    public WebSocketService(ILogger? logger = null)
    {
        _logger = logger ?? Log.Logger.ForContext<WebSocketService>();
    }

    public void SetCookies(CookieContainer cookies)
    {
        _cookies = cookies;
    }

    public async Task ConnectAsync(string ipAddress, CancellationToken cancellationToken = default)
    {
        try
        {
            _ipAddress = ipAddress;
            _logger.Debug("Connecting WebSocket to NVX at {IpAddress}", ipAddress);

            _ws = new ClientWebSocket();
            _ws.Options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            if (_cookies != null)
            {
                _ws.Options.Cookies = _cookies;
            }

            var nvxWebsocketUri = $"wss://{ipAddress}/websockify";
            await _ws.ConnectAsync(new Uri(nvxWebsocketUri), cancellationToken);

            if (_ws.State != WebSocketState.Open)
            {
                _logger.Error("Failed to connect WebSocket to NVX at {IpAddress}", ipAddress);
                throw new InvalidOperationException($"Failed to establish WebSocket connection to {ipAddress}");
            }

            _logger.Information("Successfully connected WebSocket to NVX at {IpAddress}", ipAddress);
            Connected?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error connecting WebSocket to NVX at {IpAddress}", ipAddress);
            throw;
        }
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        if (_ws == null)
        {
            _logger.Warning("Disconnect attempted but WebSocket is not initialized");
            return;
        }

        try
        {
            _logger.Debug("Disconnecting WebSocket from NVX at {IpAddress}", _ipAddress);

            _listeningCancellation?.Cancel();

            if (_ws.State == WebSocketState.Open)
            {
                await _ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client initiated closure", cancellationToken);
            }

            _ws.Dispose();
            _ws = null;

            _logger.Information("Successfully disconnected WebSocket from NVX at {IpAddress}", _ipAddress);
            Disconnected?.Invoke(this, EventArgs.Empty);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error disconnecting WebSocket from NVX at {IpAddress}", _ipAddress);
        }
    }

    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        if (_ws == null || _ws.State != WebSocketState.Open)
            throw new InvalidOperationException("WebSocket is not connected");

        try
        {
            _logger.Debug("Sending WebSocket message: {Message}", message);
            var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
            await _ws.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);
            _logger.Debug("WebSocket message sent successfully");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error sending WebSocket message");
            throw;
        }
    }

    public async Task StartListeningAsync(CancellationToken cancellationToken = default)
    {
        if (_ws == null || _ws.State != WebSocketState.Open)
            throw new InvalidOperationException("WebSocket is not connected");

        _listeningCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        try
        {
            _logger.Debug("Starting WebSocket listener for NVX at {IpAddress}", _ipAddress);

            while (_ws.State == WebSocketState.Open && !_listeningCancellation.Token.IsCancellationRequested)
            {
                var buffer = new ArraySegment<byte>(new byte[4096]);
                
                try
                {
                    var receive = await _ws.ReceiveAsync(buffer, _listeningCancellation.Token);

                    if (buffer.Array == null || receive.Count == 0)
                        continue;

                    var message = Encoding.UTF8.GetString(buffer.Array, 0, receive.Count);
                    _logger.Debug("Received WebSocket message: {Message}", message);
                    
                    MessageReceived?.Invoke(this, new WebSocketMessageReceivedEventArgs { Message = message });
                }
                catch (OperationCanceledException)
                {
                    _logger.Debug("WebSocket listener cancelled for NVX at {IpAddress}", _ipAddress);
                    break;
                }
                catch (WebSocketException ex)
                {
                    _logger.Error(ex, "WebSocket error for NVX at {IpAddress}", _ipAddress);
                    break;
                }
            }
        }
        finally
        {
            _logger.Debug("Stopped WebSocket listener for NVX at {IpAddress}", _ipAddress);
            _listeningCancellation?.Dispose();
            _listeningCancellation = null;
        }
    }
}
