namespace AlYurr_CrestronNvxSdk.Exceptions;

/// <summary>
/// Base exception for all Crestron NVX SDK operations.
/// </summary>
public class CrestronNvxSdkException : Exception
{
    public CrestronNvxSdkException(string message) : base(message) { }
    public CrestronNvxSdkException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Thrown when connection to the NVX device fails.
/// </summary>
public class ConnectionException : CrestronNvxSdkException
{
    public string IpAddress { get; }

    public ConnectionException(string ipAddress, string message) : base(message)
    {
        IpAddress = ipAddress;
    }

    public ConnectionException(string ipAddress, string message, Exception innerException)
        : base(message, innerException)
    {
        IpAddress = ipAddress;
    }
}

/// <summary>
/// Thrown when authentication with the NVX device fails.
/// </summary>
public class AuthenticationException : CrestronNvxSdkException
{
    public string IpAddress { get; }

    public AuthenticationException(string ipAddress, string message) : base(message)
    {
        IpAddress = ipAddress;
    }

    public AuthenticationException(string ipAddress, string message, Exception innerException)
        : base(message, innerException)
    {
        IpAddress = ipAddress;
    }
}

/// <summary>
/// Thrown when an API request validation fails.
/// </summary>
public class ValidationException : CrestronNvxSdkException
{
    public object? InvalidValue { get; }
    public string PropertyName { get; }

    public ValidationException(string propertyName, object? invalidValue, string message)
        : base(message)
    {
        PropertyName = propertyName;
        InvalidValue = invalidValue;
    }
}

/// <summary>
/// Thrown when a timeout occurs during an operation.
/// </summary>
public class TimeoutException : CrestronNvxSdkException
{
    public TimeoutException(string message) : base(message) { }
    public TimeoutException(string message, Exception innerException) : base(message, innerException) { }
}

/// <summary>
/// Thrown when the NVX device returns an error response.
/// </summary>
public class DeviceException : CrestronNvxSdkException
{
    public string ErrorCode { get; }

    public DeviceException(string errorCode, string message) : base(message)
    {
        ErrorCode = errorCode;
    }

    public DeviceException(string errorCode, string message, Exception innerException)
        : base(message, innerException)
    {
        ErrorCode = errorCode;
    }
}
