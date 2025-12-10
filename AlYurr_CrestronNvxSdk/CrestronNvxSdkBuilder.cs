using Serilog;
using AlYurr_CrestronNvxSdk.Services;

namespace AlYurr_CrestronNvxSdk;

/// <summary>
/// Builder for configuring and creating CrestribNvxSdk instances.
/// </summary>
public class CrestronNvxSdkBuilder
{
    private string _ipAddress = string.Empty;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private ILogger? _logger;
    private TimeSpan _timeout = TimeSpan.FromSeconds(30);
    private bool _autoReconnect = true;
    private int _maxRetries = 3;
    private CacheConfiguration? _cacheConfiguration;

    /// <summary>
    /// Configures the device connection parameters.
    /// </summary>
    public CrestronNvxSdkBuilder WithDevice(string ipAddress, string username, string password)
    {
        _ipAddress = ipAddress ?? throw new ArgumentNullException(nameof(ipAddress));
        _username = username ?? throw new ArgumentNullException(nameof(username));
        _password = password ?? throw new ArgumentNullException(nameof(password));
        return this;
    }

    /// <summary>
    /// Configures the logger to use.
    /// </summary>
    public CrestronNvxSdkBuilder WithLogger(ILogger? logger)
    {
        _logger = logger;
        return this;
    }

    /// <summary>
    /// Configures the connection timeout.
    /// </summary>
    public CrestronNvxSdkBuilder WithTimeout(TimeSpan timeout)
    {
        _timeout = timeout;
        return this;
    }

    /// <summary>
    /// Configures whether to automatically reconnect on disconnection.
    /// </summary>
    public CrestronNvxSdkBuilder WithAutoReconnect(bool enabled)
    {
        _autoReconnect = enabled;
        return this;
    }

    /// <summary>
    /// Configures the maximum number of connection retries.
    /// </summary>
    public CrestronNvxSdkBuilder WithMaxRetries(int maxRetries)
    {
        _maxRetries = maxRetries;
        return this;
    }

    /// <summary>
    /// Configures caching with default settings.
    /// </summary>
    public CrestronNvxSdkBuilder WithCaching()
    {
        _cacheConfiguration = new CacheConfiguration();
        return this;
    }

    /// <summary>
    /// Configures caching with custom settings.
    /// </summary>
    public CrestronNvxSdkBuilder WithCaching(CacheConfiguration configuration)
    {
        _cacheConfiguration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        return this;
    }

    /// <summary>
    /// Disables caching.
    /// </summary>
    public CrestronNvxSdkBuilder WithoutCaching()
    {
        _cacheConfiguration = null;
        return this;
    }

    /// <summary>
    /// Builds the CrestribNvxSdk instance with the configured settings.
    /// </summary>
    public CrestribNvxSdk Build()
    {
        if (string.IsNullOrEmpty(_ipAddress))
            throw new InvalidOperationException("Device IP address must be configured. Call WithDevice() first.");

        var sdk = new CrestribNvxSdk(_ipAddress, _username, _password, _logger)
        {
            Timeout = _timeout,
            AutoReconnect = _autoReconnect,
            MaxRetries = _maxRetries,
            CacheConfiguration = _cacheConfiguration
        };

        return sdk;
    }
}
