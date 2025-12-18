using System.Net;
using System.Net.WebSockets;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Abstracts HTTP communication with the NVX device.
/// </summary>
public interface IHttpService
{
    /// <summary>
    /// Gets the authentication state of the HTTP service.
    /// </summary>
    bool IsAuthenticated { get; }

    /// <summary>
    /// Sends an HTTP GET request and deserializes the response.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response into.</typeparam>
    /// <param name="path">The API path (e.g., "/Device/DeviceInfo").</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The deserialized response object.</returns>
    Task<T?> GetAsync<T>(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an HTTP GET request and returns the raw JSON string.
    /// </summary>
    /// <param name="path">The API path.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The raw JSON response string.</returns>
    Task<string> GetRawAsync(string path, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an HTTP POST request with a JSON payload.
    /// </summary>
    /// <typeparam name="T">The type to deserialize the response into.</typeparam>
    /// <param name="path">The API path.</param>
    /// <param name="payload">The object to serialize as JSON and send.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The deserialized response object.</returns>
    Task<T?> PostAsync<T>(string path, object payload, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an HTTP POST request and returns the raw JSON string.
    /// </summary>
    /// <param name="path">The API path.</param>
    /// <param name="payload">The object to serialize as JSON and send.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>The raw JSON response string.</returns>
    Task<string> PostRawAsync(string path, object payload, CancellationToken cancellationToken = default);

    /// <summary>
    /// Authenticates with the NVX device using the provided credentials.
    /// </summary>
    /// <param name="ipAddress">The IP address of the NVX device.</param>
    /// <param name="username">The username for authentication.</param>
    /// <param name="password">The password for authentication.</param>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>True if authentication succeeded; otherwise, false.</returns>
    Task<bool> AuthenticateAsync(string ipAddress, string username, string password, CancellationToken cancellationToken = default);

    /// <summary>
    /// Logs out from the NVX device.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the request.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task LogoutAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the underlying HttpClient for advanced scenarios.
    /// </summary>
    HttpClient GetHttpClient();

    /// <summary>
    /// Sets the HttpClientHandler for the HTTP client.
    /// </summary>
    /// <param name="handler"></param>
    void SetHttpClientHandler(HttpClientHandler handler);
}

/// <summary>
/// Abstracts WebSocket communication with the NVX device.
/// </summary>
public interface IWebSocketService
{
    /// <summary>
    /// Gets the current connection state.
    /// </summary>
    WebSocketState State { get; }

    /// <summary>
    /// Connects to the WebSocket endpoint.
    /// </summary>
    /// <param name="ipAddress">The IP address of the NVX device.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ConnectAsync(string ipAddress, CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects from the WebSocket endpoint.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DisconnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a message through the WebSocket.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendMessageAsync(string message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Starts listening for WebSocket messages.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>A task that completes when the listener stops.</returns>
    Task StartListeningAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Event fired when a message is received from the WebSocket.
    /// </summary>
    event EventHandler<WebSocketMessageReceivedEventArgs>? MessageReceived;

    /// <summary>
    /// Event fired when the WebSocket is connected.
    /// </summary>
    event EventHandler? Connected;

    /// <summary>
    /// Event fired when the WebSocket is disconnected.
    /// </summary>
    event EventHandler? Disconnected;

    /// <summary>
    /// Sets the cookies for WebSocket authentication.
    /// </summary>  
    public void SetCookies(CookieContainer cookies);

}

/// <summary>
/// Event arguments for WebSocket messages.
/// </summary>
public class WebSocketMessageReceivedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the raw message content.
    /// </summary>
    public string Message { get; set; } = string.Empty;
}
