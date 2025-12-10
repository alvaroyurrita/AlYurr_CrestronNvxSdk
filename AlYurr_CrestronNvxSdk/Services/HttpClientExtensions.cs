using System.Reflection;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Extension methods for HttpClient.
/// </summary>
internal static class HttpClientExtensions
{
    /// <summary>
    /// Gets the underlying HttpClientHandler from an HttpClient.
    /// </summary>
    public static HttpClientHandler? GetHttpClientHandler(this HttpClient httpClient)
    {
        try
        {
            var field = typeof(HttpClient)
                .GetField("_handler", BindingFlags.NonPublic | BindingFlags.Instance);
            return field?.GetValue(httpClient) as HttpClientHandler;
        }
        catch
        {
            return null;
        }
    }
}
