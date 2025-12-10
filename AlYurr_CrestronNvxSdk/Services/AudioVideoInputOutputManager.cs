using AlYurr_CrestronNvxSdk.Models.AudioVideoInputOutput;
using AlYurr_CrestronNvxSdk.Exceptions;
using Microsoft.Extensions.Logging;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Manager for audio/video input and output operations.
/// </summary>
public class AudioVideoInputOutputManager : IAudioVideoInputOutputManager
{
    private readonly IHttpService _httpService;
    private readonly ILogger? _logger;
    private readonly AudioVideoInputOutputState _state = new();

    /// <summary>
    /// Gets the current audio/video state.
    /// </summary>
    public AudioVideoInputOutputState State => _state;

    /// <summary>
    /// Initializes a new instance of the <see cref="AudioVideoInputOutputManager"/> class.
    /// </summary>
    /// <param name="httpService">The HTTP service for API communication.</param>
    /// <param name="logger">Optional logger for debugging.</param>
    public AudioVideoInputOutputManager(IHttpService httpService, ILogger? logger = null)
    {
        _httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));
        _logger = logger;
    }

    /// <summary>
    /// Retrieves all audio/video inputs and outputs asynchronously.
    /// </summary>
    /// <returns>The audio/video input/output state.</returns>
    public async Task<AudioVideoInputOutputState> GetAsync()
    {
        try
        {
            _logger?.LogDebug("Fetching audio/video inputs and outputs");
            var result = await _httpService.GetAsync<AudioVideoInputOutputDto>("/api/v1/audioVideoInputOutput");
            
            if (result != null)
            {
                _state.Data = result;
                _logger?.LogInformation("Audio/video inputs and outputs retrieved successfully");
            }
            
            return _state;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to fetch audio/video inputs and outputs");
            throw new DeviceException("AV_IO_ERROR", "Failed to retrieve audio/video inputs and outputs", ex);
        }
    }

    /// <summary>
    /// Refreshes audio/video inputs and outputs by polling the device.
    /// </summary>
    /// <returns>The updated audio/video state.</returns>
    public async Task<AudioVideoInputOutputState> RefreshAsync()
    {
        return await GetAsync();
    }

    /// <summary>
    /// Gets a specific input by UUID.
    /// </summary>
    /// <param name="inputUuid">The UUID of the input.</param>
    /// <returns>The input state, or null if not found.</returns>
    public async Task<InputState?> GetInputAsync(string inputUuid)
    {
        try
        {
            if (string.IsNullOrEmpty(inputUuid))
                throw new ArgumentNullException(nameof(inputUuid));
            
            _logger?.LogDebug("Fetching input {InputUuid}", inputUuid);
            var result = await _httpService.GetAsync<InputDto>($"/api/v1/audioVideoInputOutput/inputs/{inputUuid}");
            
            if (result != null)
            {
                var inputState = new InputState { Data = result };
                _logger?.LogInformation("Input {InputUuid} retrieved successfully", inputUuid);
                return inputState;
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to fetch input {InputUuid}", inputUuid);
            throw new DeviceException("AV_IO_ERROR", "Failed to retrieve input", ex);
        }
    }

    /// <summary>
    /// Gets a specific output by UUID.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <returns>The output state, or null if not found.</returns>
    public async Task<OutputState?> GetOutputAsync(string outputUuid)
    {
        try
        {
            if (string.IsNullOrEmpty(outputUuid))
                throw new ArgumentNullException(nameof(outputUuid));
            
            _logger?.LogDebug("Fetching output {OutputUuid}", outputUuid);
            var result = await _httpService.GetAsync<OutputDto>($"/api/v1/audioVideoInputOutput/outputs/{outputUuid}");
            
            if (result != null)
            {
                var outputState = new OutputState { Data = result };
                _logger?.LogInformation("Output {OutputUuid} retrieved successfully", outputUuid);
                return outputState;
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to fetch output {OutputUuid}", outputUuid);
            throw new DeviceException("AV_IO_ERROR", "Failed to retrieve output", ex);
        }
    }

    /// <summary>
    /// Sets the audio volume for an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="volume">The volume level (-80 to 0 dB).</param>
    public async Task SetOutputVolumeAsync(string outputUuid, short volume)
    {
        try
        {
            if (string.IsNullOrEmpty(outputUuid))
                throw new ArgumentNullException(nameof(outputUuid));
            
            _logger?.LogDebug("Setting output {OutputUuid} volume to {Volume}", outputUuid, volume);
            var payload = new { volume };
            await _httpService.PostAsync<object>($"/api/v1/audioVideoInputOutput/outputs/{outputUuid}/audio/volume", payload);
            
            _logger?.LogInformation("Output volume set successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to set output volume");
            throw new DeviceException("AV_IO_ERROR", "Failed to set output volume", ex);
        }
    }

    /// <summary>
    /// Mutes or unmutes an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="mute">True to mute, false to unmute.</param>
    public async Task SetOutputMuteAsync(string outputUuid, bool mute)
    {
        try
        {
            if (string.IsNullOrEmpty(outputUuid))
                throw new ArgumentNullException(nameof(outputUuid));
            
            _logger?.LogDebug("Setting output {OutputUuid} mute to {Mute}", outputUuid, mute);
            var payload = new { mute };
            await _httpService.PostAsync<object>($"/api/v1/audioVideoInputOutput/outputs/{outputUuid}/audio/mute", payload);
            
            _logger?.LogInformation("Output mute state set successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to set output mute state");
            throw new DeviceException("AV_IO_ERROR", "Failed to set output mute state", ex);
        }
    }

    /// <summary>
    /// Sets the HDCP state for an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="hdcpMode">The HDCP mode (Auto, FollowInput, Always, Never).</param>
    public async Task SetOutputHdcpAsync(string outputUuid, string hdcpMode)
    {
        try
        {
            if (string.IsNullOrEmpty(outputUuid))
                throw new ArgumentNullException(nameof(outputUuid));
            if (string.IsNullOrEmpty(hdcpMode))
                throw new ArgumentNullException(nameof(hdcpMode));
            
            _logger?.LogDebug("Setting output {OutputUuid} HDCP mode to {HdcpMode}", outputUuid, hdcpMode);
            var payload = new { hdcpTransmitterMode = hdcpMode };
            await _httpService.PostAsync<object>($"/api/v1/audioVideoInputOutput/outputs/{outputUuid}/hdmi/hdcp", payload);
            
            _logger?.LogInformation("Output HDCP mode set successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to set output HDCP mode");
            throw new DeviceException("AV_IO_ERROR", "Failed to set output HDCP mode", ex);
        }
    }

    /// <summary>
    /// Sets the resolution for an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="resolution">The resolution name.</param>
    public async Task SetOutputResolutionAsync(string outputUuid, string resolution)
    {
        try
        {
            if (string.IsNullOrEmpty(outputUuid))
                throw new ArgumentNullException(nameof(outputUuid));
            if (string.IsNullOrEmpty(resolution))
                throw new ArgumentNullException(nameof(resolution));
            
            _logger?.LogDebug("Setting output {OutputUuid} resolution to {Resolution}", outputUuid, resolution);
            var payload = new { resolutionOptionsName = resolution };
            await _httpService.PostAsync<object>($"/api/v1/audioVideoInputOutput/outputs/{outputUuid}/resolution", payload);
            
            _logger?.LogInformation("Output resolution set successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to set output resolution");
            throw new DeviceException("AV_IO_ERROR", "Failed to set output resolution", ex);
        }
    }

    /// <summary>
    /// Sets the color space for an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="colorSpace">The color space (RGB, YCbCr).</param>
    public async Task SetOutputColorSpaceAsync(string outputUuid, string colorSpace)
    {
        try
        {
            if (string.IsNullOrEmpty(outputUuid))
                throw new ArgumentNullException(nameof(outputUuid));
            if (string.IsNullOrEmpty(colorSpace))
                throw new ArgumentNullException(nameof(colorSpace));
            
            _logger?.LogDebug("Setting output {OutputUuid} color space to {ColorSpace}", outputUuid, colorSpace);
            var payload = new { colorSpace };
            await _httpService.PostAsync<object>($"/api/v1/audioVideoInputOutput/outputs/{outputUuid}/colorSpace", payload);
            
            _logger?.LogInformation("Output color space set successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to set output color space");
            throw new DeviceException("AV_IO_ERROR", "Failed to set output color space", ex);
        }
    }

    /// <summary>
    /// Sends a CEC command to an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="cecCommand">The CEC command to send.</param>
    public async Task SendCecCommandAsync(string outputUuid, string cecCommand)
    {
        try
        {
            if (string.IsNullOrEmpty(outputUuid))
                throw new ArgumentNullException(nameof(outputUuid));
            if (string.IsNullOrEmpty(cecCommand))
                throw new ArgumentNullException(nameof(cecCommand));
            
            _logger?.LogDebug("Sending CEC command to output {OutputUuid}", outputUuid);
            var payload = new { cecCommand };
            await _httpService.PostAsync<object>($"/api/v1/audioVideoInputOutput/outputs/{outputUuid}/cec/command", payload);
            
            _logger?.LogInformation("CEC command sent successfully");
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Failed to send CEC command");
            throw new DeviceException("AV_IO_ERROR", "Failed to send CEC command", ex);
        }
    }
}
