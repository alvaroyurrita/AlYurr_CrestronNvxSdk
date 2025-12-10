using System.Text.Json;

namespace AlYurr_CrestronNvxSdk;

/// <summary>
/// Partial class for message parsing functionality.
/// </summary>
public partial class CrestronNvxSdk
{
    /// <summary>
    /// Parses a complete device state message and updates internal state.
    /// </summary>
    public void FullMessageParser(string deviceJson)
    {
        try
        {
            NvxState.RawDeviceJson = deviceJson;
            _logger.Debug("Full message parsed and state updated");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error parsing full message");
            throw;
        }
    }

    /// <summary>
    /// Parses a partial state update message.
    /// </summary>
    public void PartialMessageParser(string partialJson)
    {
        try
        {
            NvxState.MergeJson(partialJson);
            _logger.Debug("Partial message merged into state");
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error parsing partial message");
            throw;
        }
    }
}