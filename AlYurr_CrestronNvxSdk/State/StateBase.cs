namespace AlYurr_CrestronNvxSdk.State;

/// <summary>
/// Event arguments for state change notifications.
/// </summary>
public class StateChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the category of the state that changed (e.g., "DeviceInfo", "AudioVideoInputOutput").
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets the property path that changed (e.g., "Inputs[0].Ports[0].Hdmi.HdcpState").
    /// </summary>
    public string PropertyPath { get; set; } = string.Empty;

    /// <summary>
    /// Gets the old value before the change.
    /// </summary>
    public object? OldValue { get; set; }

    /// <summary>
    /// Gets the new value after the change.
    /// </summary>
    public object? NewValue { get; set; }

    /// <summary>
    /// Gets the raw JSON that was received from the device.
    /// </summary>
    public string RawJson { get; set; } = string.Empty;
}

/// <summary>
/// Interface for objects that track state changes.
/// </summary>
public interface IStateChanged
{
    /// <summary>
    /// Event fired when any property in the state changes.
    /// </summary>
    event EventHandler<StateChangedEventArgs>? StateChanged;
}

/// <summary>
/// Base class for state objects.
/// </summary>
public abstract class StateBase : IStateChanged
{
    public event EventHandler<StateChangedEventArgs>? StateChanged;

    /// <summary>
    /// Raises the StateChanged event.
    /// </summary>
    protected virtual void RaiseStateChanged(StateChangedEventArgs args)
    {
        StateChanged?.Invoke(this, args);
    }

    /// <summary>
    /// Creates state changed event arguments.
    /// </summary>
    protected static StateChangedEventArgs CreateStateChangedEventArgs(
        string category,
        string propertyPath,
        object? oldValue,
        object? newValue,
        string rawJson = "")
    {
        return new StateChangedEventArgs
        {
            Category = category,
            PropertyPath = propertyPath,
            OldValue = oldValue,
            NewValue = newValue,
            RawJson = rawJson
        };
    }
}

/// <summary>
/// Tracks the complete state of the NVX device.
/// </summary>
public class NvxState : StateBase
{
    private string _rawDeviceJson = string.Empty;

    /// <summary>
    /// Gets or sets the raw device JSON from the last state update.
    /// </summary>
    public string RawDeviceJson
    {
        get => _rawDeviceJson;
        set
        {
            if (_rawDeviceJson != value)
            {
                var oldValue = _rawDeviceJson;
                _rawDeviceJson = value;
                RaiseStateChanged(CreateStateChangedEventArgs("NvxState", "RawDeviceJson", oldValue, value, value));
            }
        }
    }

    /// <summary>
    /// Updates the state with a raw JSON merge.
    /// </summary>
    public void MergeJson(string jsonUpdate)
    {
        RawDeviceJson = AlYurr_CrestronNvxSdk.HelperFunctions.JsonMerge.Merge(_rawDeviceJson, jsonUpdate);
    }
}
