using System.Net;
using System.Text.Json;
using Serilog;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Implementation of IHttpService for communicating with NVX devices.
/// </summary>
public class HttpService : IHttpService
{
    private HttpClient? _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ILogger _logger;
    private string _ipAddress = string.Empty;

    public bool IsAuthenticated => _httpClient != null;

    public HttpService(ILogger? logger = null)
    {
        _logger = logger ?? Log.Logger.ForContext<HttpService>();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            WriteIndented = false
        };
    }

    public async Task<bool> AuthenticateAsync(string ipAddress, string username, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            _ipAddress = ipAddress;
            _logger.Debug("Authenticating to NVX at {IpAddress}", ipAddress);

            var credentials = $"login={username}&&passwd={password}";
            var handler = new HttpClientHandler
            {
                ClientCertificateOptions = ClientCertificateOption.Manual,
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
                CookieContainer = new CookieContainer()
            };

            _httpClient = new HttpClient(handler);

            using var body = new StringContent(credentials);
            var nvxUri = $"https://{ipAddress}/userlogin.html";
            
            var result = await _httpClient.PostAsync(nvxUri, body, cancellationToken);
            
            if (!result.IsSuccessStatusCode)
            {
                _logger.Error("Authentication failed to NVX at {IpAddress} with status code {StatusCode}", 
                    ipAddress, result.StatusCode);
                return false;
            }

            _logger.Information("Successfully authenticated to NVX at {IpAddress}", ipAddress);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error during authentication to NVX at {IpAddress}", ipAddress);
            return false;
        }
    }

    public async Task LogoutAsync(CancellationToken cancellationToken = default)
    {
        if (_httpClient == null)
        {
            _logger.Warning("Logout attempted but HTTP client is not initialized");
            return;
        }

        try
        {
            _logger.Debug("Logging out from NVX at {IpAddress}", _ipAddress);
            await _httpClient.GetStringAsync($"https://{_ipAddress}/logout", cancellationToken);
            _httpClient?.Dispose();
            _httpClient = null;
            _logger.Information("Successfully logged out from NVX at {IpAddress}", _ipAddress);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error during logout from NVX at {IpAddress}", _ipAddress);
        }
    }

    public async Task<T?> GetAsync<T>(string path, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = await GetRawAsync(path, cancellationToken);
            if (string.IsNullOrEmpty(json))
                return default;

            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }
        catch (JsonException ex)
        {
            _logger.Error(ex, "JSON deserialization error for GET {Path}", path);
            return default;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error during GET request to {Path}", path);
            return default;
        }
    }

    public async Task<string> GetRawAsync(string path, CancellationToken cancellationToken = default)
    {
        if (_httpClient == null)
            throw new InvalidOperationException("HTTP client not authenticated. Call AuthenticateAsync first.");

        try
        {
            _logger.Debug("GET request to {Path}", path);
            var url = $"https://{_ipAddress}{path}";
            var response = await _httpClient.GetStringAsync(url, cancellationToken);
            _logger.Debug("GET request to {Path} succeeded", path);
            return response;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error during GET request to {Path}", path);
            throw;
        }
    }

    public async Task<T?> PostAsync<T>(string path, object payload, CancellationToken cancellationToken = default)
    {
        try
        {
            var json = await PostRawAsync(path, payload, cancellationToken);
            if (string.IsNullOrEmpty(json))
                return default;

            return JsonSerializer.Deserialize<T>(json, _jsonOptions);
        }
        catch (JsonException ex)
        {
            _logger.Error(ex, "JSON deserialization error for POST {Path}", path);
            return default;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error during POST request to {Path}", path);
            return default;
        }
    }

    public async Task<string> PostRawAsync(string path, object payload, CancellationToken cancellationToken = default)
    {
        if (_httpClient == null)
            throw new InvalidOperationException("HTTP client not authenticated. Call AuthenticateAsync first.");

        try
        {
            _logger.Debug("POST request to {Path}", path);
            var url = $"https://{_ipAddress}{path}";
            var jsonContent = JsonSerializer.Serialize(payload, _jsonOptions);
            var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(url, content, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            
            if (!response.IsSuccessStatusCode)
            {
                _logger.Warning("POST request to {Path} returned status {StatusCode}", path, response.StatusCode);
            }
            else
            {
                _logger.Debug("POST request to {Path} succeeded", path);
            }

            return responseContent;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error during POST request to {Path}", path);
            throw;
        }
    }

    public HttpClient GetHttpClient()
    {
        if (_httpClient == null)
            throw new InvalidOperationException("HTTP client not authenticated. Call AuthenticateAsync first.");
        return _httpClient;
    }
}
