using AlYurr_CrestronNvxSdk.Models.AudioVideoInputOutput;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Interface for managing audio/video input and output operations.
/// </summary>
public interface IAudioVideoInputOutputManager
{
    /// <summary>
    /// Gets the current audio/video state.
    /// </summary>
    AudioVideoInputOutputState State { get; }

    /// <summary>
    /// Retrieves all audio/video inputs and outputs asynchronously.
    /// </summary>
    /// <returns>The audio/video input/output state.</returns>
    /// <exception cref="CrestronNvxSdkException">Thrown when an error occurs during the operation.</exception>
    Task<AudioVideoInputOutputState> GetAsync();

    /// <summary>
    /// Refreshes audio/video inputs and outputs by polling the device.
    /// </summary>
    /// <returns>The updated audio/video state.</returns>
    Task<AudioVideoInputOutputState> RefreshAsync();

    /// <summary>
    /// Gets a specific input by UUID.
    /// </summary>
    /// <param name="inputUuid">The UUID of the input.</param>
    /// <returns>The input state, or null if not found.</returns>
    Task<InputState?> GetInputAsync(string inputUuid);

    /// <summary>
    /// Gets a specific output by UUID.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <returns>The output state, or null if not found.</returns>
    Task<OutputState?> GetOutputAsync(string outputUuid);

    /// <summary>
    /// Sets the audio volume for an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="volume">The volume level (-80 to 0 dB).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetOutputVolumeAsync(string outputUuid, short volume);

    /// <summary>
    /// Mutes or unmutes an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="mute">True to mute, false to unmute.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetOutputMuteAsync(string outputUuid, bool mute);

    /// <summary>
    /// Sets the HDCP state for an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="hdcpMode">The HDCP mode (Auto, FollowInput, Always, Never).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetOutputHdcpAsync(string outputUuid, string hdcpMode);

    /// <summary>
    /// Sets the resolution for an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="resolution">The resolution name.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetOutputResolutionAsync(string outputUuid, string resolution);

    /// <summary>
    /// Sets the color space for an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="colorSpace">The color space (RGB, YCbCr).</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SetOutputColorSpaceAsync(string outputUuid, string colorSpace);

    /// <summary>
    /// Sends a CEC command to an output.
    /// </summary>
    /// <param name="outputUuid">The UUID of the output.</param>
    /// <param name="cecCommand">The CEC command to send.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendCecCommandAsync(string outputUuid, string cecCommand);
}
