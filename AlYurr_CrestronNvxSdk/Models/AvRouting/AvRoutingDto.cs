using AlYurr_CrestronNvxSdk.State;

namespace AlYurr_CrestronNvxSdk.Models.AvRouting;

/// <summary>
/// Represents route control options.
/// </summary>
public class RouteControlDto
{
    /// <summary>
    /// Gets or sets whether network layer 3 is enabled.
    /// </summary>
    public bool IsLayer3Enabled { get; set; }

    /// <summary>
    /// Gets or sets whether USB follows video is enabled.
    /// </summary>
    public bool IsUsbFollowsVideoEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether changing USB remote device is enabled.
    /// </summary>
    public bool IsChangeUsbRemoteDeviceEnabled { get; set; }

    /// <summary>
    /// Gets or sets whether secondary audio follows video is enabled.
    /// </summary>
    public bool IsSecondaryAudioFollowsVideoEnabled { get; set; }
}

/// <summary>
/// Represents an audio/video route.
/// </summary>
public class RouteDto
{
    /// <summary>
    /// Gets or sets the name of the stream object.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the desired audio source
    /// </summary>
    public string? AudioSource { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the desired video source
    /// </summary>
    public string? VideoSource { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the desired USB source
    /// </summary>
    public string? UsbSource { get; set; }

    /// <summary>
    /// Gets or sets whether automatic stream routing is enabled.
    /// </summary>
    public bool AutomaticStreamRoutingEnabled { get; set; }

    /// <summary>
    /// Gets or sets the unique ID of the stream object.
    /// </summary>
    public string? UniqueId { get; set; }
}

/// <summary>
/// Data Transfer Object for AV Routing response.
/// </summary>
public class AvRoutingDto
{
    /// <summary>
    /// Gets or sets the route control settings.
    /// </summary>
    public RouteControlDto? RouteControl { get; set; }

    /// <summary>
    /// Gets or sets the collection of routes.
    /// </summary>
    public List<RouteDto>? Routes { get; set; }

    /// <summary>
    /// Gets or sets the API version.
    /// </summary>
    public string? Version { get; set; }
}
//TODO: Check consistency between RouteControlState and RouteState property setters
//Seems like RouteControlState has extra logic in the setters that RouteState lacks. It is also generating events whenever a property is set
//Should this happen everywhere or only in RouteControlState or remove it from there?
/// <summary>
/// State object for route control.
/// </summary>
public class RouteControlState : StateBase
{
    private RouteControlDto _data = new();

    public RouteControlDto Data
    {
        get => _data;
        set
        {
            if (_data != value)
            {
                var oldValue = _data;
                _data = value;
                RaiseStateChanged(CreateStateChangedEventArgs("RouteControl", "Data", oldValue, value));
            }
        }
    }

    public bool IsLayer3Enabled
    {
        get => _data.IsLayer3Enabled;
        set
        {
            if (_data.IsLayer3Enabled != value)
            {
                var oldValue = _data.IsLayer3Enabled;
                _data.IsLayer3Enabled = value;
                RaiseStateChanged(CreateStateChangedEventArgs("RouteControl", nameof(IsLayer3Enabled), oldValue, value));
            }
        }
    }

    public bool IsUsbFollowsVideoEnabled
    {
        get => _data.IsUsbFollowsVideoEnabled;
        set
        {
            if (_data.IsUsbFollowsVideoEnabled != value)
            {
                var oldValue = _data.IsUsbFollowsVideoEnabled;
                _data.IsUsbFollowsVideoEnabled = value;
                RaiseStateChanged(CreateStateChangedEventArgs("RouteControl", nameof(IsUsbFollowsVideoEnabled), oldValue, value));
            }
        }
    }

    public bool IsChangeUsbRemoteDeviceEnabled
    {
        get => _data.IsChangeUsbRemoteDeviceEnabled;
        set
        {
            if (_data.IsChangeUsbRemoteDeviceEnabled != value)
            {
                var oldValue = _data.IsChangeUsbRemoteDeviceEnabled;
                _data.IsChangeUsbRemoteDeviceEnabled = value;
                RaiseStateChanged(CreateStateChangedEventArgs("RouteControl", nameof(IsChangeUsbRemoteDeviceEnabled), oldValue, value));
            }
        }
    }

    public bool IsSecondaryAudioFollowsVideoEnabled
    {
        get => _data.IsSecondaryAudioFollowsVideoEnabled;
        set
        {
            if (_data.IsSecondaryAudioFollowsVideoEnabled != value)
            {
                var oldValue = _data.IsSecondaryAudioFollowsVideoEnabled;
                _data.IsSecondaryAudioFollowsVideoEnabled = value;
                RaiseStateChanged(CreateStateChangedEventArgs("RouteControl", nameof(IsSecondaryAudioFollowsVideoEnabled), oldValue, value));
            }
        }
    }
}
//TODO: Some of these properties should have setters since they accept post.
/// <summary>
/// State object for an individual route.
/// </summary>
public class RouteState : StateBase
{
    private RouteDto _data = new();

    public RouteDto Data
    {
        get => _data;
        set
        {
            if (_data != value)
            {
                var oldValue = _data;
                _data = value;
                RaiseStateChanged(CreateStateChangedEventArgs("Route", "Data", oldValue, value));
            }
        }
    }

    public string? Name => _data.Name;
    public string? AudioSource => _data.AudioSource;
    public string? VideoSource => _data.VideoSource;
    public string? UsbSource => _data.UsbSource;
    public bool AutomaticStreamRoutingEnabled => _data.AutomaticStreamRoutingEnabled;
    public string? UniqueId => _data.UniqueId;
}

/// <summary>
/// State object for AV Routing.
/// </summary>
public class AvRoutingState : StateBase
{
    private AvRoutingDto _data = new();
    private List<RouteState> _routeStates = new();

    public AvRoutingDto Data
    {
        get => _data;
        set
        {
            if (_data != value)
            {
                var oldValue = _data;
                _data = value;
                UpdateRouteStates();
                RaiseStateChanged(CreateStateChangedEventArgs("AvRouting", "Data", oldValue, value));
            }
        }
    }

    public RouteControlState RouteControl { get; } = new();

    public List<RouteState> Routes => _routeStates;

    public string? Version => _data.Version;

    private void UpdateRouteStates()
    {
        _routeStates.Clear();
        if (_data.Routes != null)
        {
            foreach (var route in _data.Routes)
            {
                _routeStates.Add(new RouteState { Data = route });
            }
        }
    }
}
