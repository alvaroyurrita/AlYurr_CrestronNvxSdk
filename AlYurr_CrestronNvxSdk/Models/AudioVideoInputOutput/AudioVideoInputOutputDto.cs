using AlYurr_CrestronNvxSdk.State;

namespace AlYurr_CrestronNvxSdk.Models.AudioVideoInputOutput;

/// <summary>
/// Represents EDID configuration.
/// </summary>
public class EdidDto
{
    public string? CurrentEdid { get; set; }
    public List<EdidListItemDto>? EdidList { get; set; }
}

public class EdidListItemDto
{
    public string? Name { get; set; }
    public string? Type { get; set; }
}

/// <summary>
/// Represents digital audio configuration.
/// </summary>
public class DigitalAudioDto
{
    public ushort Channels { get; set; }
    public string? Format { get; set; }
}

/// <summary>
/// Represents audio configuration for inputs/outputs.
/// </summary>
public class AudioDto
{
    public ushort AudioTypeSelect { get; set; }
    public DigitalAudioDto? Digital { get; set; }
    public bool Mute { get; set; }
    public short Volume { get; set; }
    public short Bass { get; set; }
    public short Treble { get; set; }
}

/// <summary>
/// Represents power control configuration.
/// </summary>
public class PowerDto
{
    public string? DmInputType { get; set; } // "OFF", "Dm", "DmLite"
}

/// <summary>
/// Represents HDMI input configuration.
/// </summary>
public class HdmiInputDto
{
    public bool IsSourceHdcpActive { get; set; }
    public string? HdcpReceiverCapability { get; set; }
    public string? HdcpState { get; set; }
    public bool IsCecErrorDetected { get; set; }
    public ushort Status3D { get; set; }
    public string? ReceiveCecMessage { get; set; }
    public bool UnsupportedVideoDetected { get; set; }
}

/// <summary>
/// Represents an input port.
/// </summary>
public class InputPortDto
{
    public string? PortType { get; set; } // "Hdmi", "Analog", "Audio"
    public string? Uuid { get; set; }
    public bool IsSyncDetected { get; set; }
    public bool IsSourceDetected { get; set; }
    public bool IsInterlacedDetected { get; set; }
    public string? ColorSpace { get; set; }
    public string? ColorDepth { get; set; }
    public ushort HorizontalResolution { get; set; }
    public ushort VerticalResolution { get; set; }
    public ushort FramesPerSecond { get; set; }
    public string? AspectRatio { get; set; }
    public PowerDto? Power { get; set; }
    public EdidDto? Edid { get; set; }
    public HdmiInputDto? Hdmi { get; set; }
    public AudioDto? Audio { get; set; }
}

/// <summary>
/// Represents an input.
/// </summary>
public class InputDto
{
    public string? Name { get; set; }
    public string? Uuid { get; set; }
    public bool EndpointExists { get; set; }
    public string? EndpointId { get; set; }
    public string? VideoPortTypeSelect { get; set; } // "Hdmi", "Vga", "Bnc", "Auto"
    public List<InputPortDto>? Ports { get; set; }
}

/// <summary>
/// Represents downstream EDID configuration.
/// </summary>
public class DownstreamEdidDto
{
    public string? ManufacturerString { get; set; }
    public string? SerialNumberString { get; set; }
    public string? NameString { get; set; }
    public string? PrefTimingString { get; set; }
}

/// <summary>
/// Represents input control for CEC.
/// </summary>
public class InputControlDto
{
    public string? Format { get; set; } // "Hex", "Ascii"
    public string? Delimiter { get; set; } // "NONE", "CR", "LF", "CR_LF"
}

/// <summary>
/// Represents CEC control configuration.
/// </summary>
public class CecControlDto
{
    public string? Format { get; set; }
    public string? Delimiter { get; set; }
    public string? Type { get; set; }
    public InputControlDto? InputControl { get; set; }
}

/// <summary>
/// Represents HDMI output configuration.
/// </summary>
public class HdmiOutputDto
{
    public bool IsBlankingDisabled { get; set; }
    public bool DisabledByHdcp { get; set; }
    public bool IsOutputDisabled { get; set; }
    public string? HdcpState { get; set; }
    public bool IsCecInError { get; set; }
    public string? ReceiveCecMessage { get; set; }
    public string? HdcpTransmitterMode { get; set; } // "Auto", "FollowInput", "Always", "Never"
    public bool UnsupportedVideoDetected { get; set; }
}

/// <summary>
/// Represents an output port.
/// </summary>
public class OutputPortDto
{
    public string? PortType { get; set; }
    public string? Uuid { get; set; }
    public bool IsSinkConnected { get; set; }
    public bool IsSourceDetected { get; set; }
    public string? ColorSpace { get; set; }
    public string? ColorDepth { get; set; }
    public string? ColorSpaceMode { get; set; }
    public ushort HorizontalBezelCompensation { get; set; }
    public ushort VerticalBezelCompensation { get; set; }
    public ushort VideoTimeout { get; set; }
    public bool IsVideoTimeoutEnabled { get; set; }
    public float Underscan { get; set; }
    public string? MaxColorDepth { get; set; }
    public string? AspectRatioMode { get; set; }
    public ushort HorizontalResolution { get; set; }
    public ushort VerticalResolution { get; set; }
    public ushort FramesPerSecond { get; set; }
    public string? AspectRatio { get; set; }
    public string? Resolution { get; set; }
    public string? ResolutionOptionsName { get; set; }
    public string? ResolutionOptionsVersion { get; set; }
    public string? Orientation { get; set; } // "Landscape", "Portrait"
    public DownstreamEdidDto? DownstreamEdid { get; set; }
    public HdmiOutputDto? Hdmi { get; set; }
    public CecControlDto? CecControl { get; set; }
    public AudioDto? Audio { get; set; }
}

/// <summary>
/// Represents an output.
/// </summary>
public class OutputDto
{
    public string? Name { get; set; }
    public string? Uuid { get; set; }
    public bool EndpointExists { get; set; }
    public string? VideoPortTypeSelect { get; set; }
    public List<OutputPortDto>? Ports { get; set; }
}

/// <summary>
/// Represents global audio/video configuration.
/// </summary>
public class GlobalConfigDto
{
    public string? GlobalEdid { get; set; }
    public string? GlobalEdidType { get; set; } // "Copy", "System", "Custom"
}

/// <summary>
/// Data Transfer Object for AudioVideoInputOutput response.
/// </summary>
public class AudioVideoInputOutputDto
{
    public GlobalConfigDto? GlobalConfig { get; set; }
    public List<InputDto>? Inputs { get; set; }
    public List<OutputDto>? Outputs { get; set; }
    public string? Version { get; set; }
}

/// <summary>
/// State object for AudioVideoInputOutput.
/// </summary>
public class AudioVideoInputOutputState : StateBase
{
    private AudioVideoInputOutputDto _data = new();
    private List<InputState> _inputStates = new();
    private List<OutputState> _outputStates = new();

    public AudioVideoInputOutputDto Data
    {
        get => _data;
        set
        {
            if (_data != value)
            {
                var oldValue = _data;
                _data = value;
                UpdateInputAndOutputStates();
                RaiseStateChanged(CreateStateChangedEventArgs("AudioVideoInputOutput", "Data", oldValue, value));
            }
        }
    }

    public List<InputState> Inputs => _inputStates;
    public List<OutputState> _outputStates_internal => _outputStates;
    public string? Version => _data.Version;

    private void UpdateInputAndOutputStates()
    {
        _inputStates.Clear();
        if (_data.Inputs != null)
        {
            foreach (var input in _data.Inputs)
            {
                _inputStates.Add(new InputState { Data = input });
            }
        }

        _outputStates.Clear();
        if (_data.Outputs != null)
        {
            foreach (var output in _data.Outputs)
            {
                _outputStates.Add(new OutputState { Data = output });
            }
        }
    }
}

/// <summary>
/// State object for an input.
/// </summary>
public class InputState : StateBase
{
    private InputDto _data = new();

    public InputDto Data
    {
        get => _data;
        set
        {
            if (_data != value)
            {
                _data = value;
                RaiseStateChanged(CreateStateChangedEventArgs("Input", "Data", null, value));
            }
        }
    }

    public string? Name => _data.Name;
    public string? Uuid => _data.Uuid;
    public bool EndpointExists => _data.EndpointExists;
    public string? EndpointId => _data.EndpointId;
    public string? VideoPortTypeSelect => _data.VideoPortTypeSelect;
    public List<InputPortDto>? Ports => _data.Ports;
}

/// <summary>
/// State object for an output.
/// </summary>
public class OutputState : StateBase
{
    private OutputDto _data = new();

    public OutputDto Data
    {
        get => _data;
        set
        {
            if (_data != value)
            {
                _data = value;
                RaiseStateChanged(CreateStateChangedEventArgs("Output", "Data", null, value));
            }
        }
    }

    public string? Name => _data.Name;
    public string? Uuid => _data.Uuid;
    public bool EndpointExists => _data.EndpointExists;
    public string? VideoPortTypeSelect => _data.VideoPortTypeSelect;
    public List<OutputPortDto>? Ports => _data.Ports;
}
