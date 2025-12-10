using AlYurr_CrestronNvxSdk.Models.AvRouting;
using AlYurr_CrestronNvxSdk.Models.AudioVideoInputOutput;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Interface for building and executing batch operations.
/// </summary>
public interface IBatchOperation
{
    /// <summary>
    /// Adds a routing operation to the batch.
    /// </summary>
    IBatchOperation SetAudioSource(string sourceUuid);

    /// <summary>
    /// Adds a video source routing operation to the batch.
    /// </summary>
    IBatchOperation SetVideoSource(string sourceUuid);

    /// <summary>
    /// Adds an audio output volume operation to the batch.
    /// </summary>
    IBatchOperation SetOutputVolume(string outputUuid, short volumeLevel);

    /// <summary>
    /// Adds an audio output mute operation to the batch.
    /// </summary>
    IBatchOperation SetOutputMute(string outputUuid, bool muted);

    /// <summary>
    /// Adds an HDCP control operation to the batch.
    /// </summary>
    IBatchOperation SetOutputHdcp(string outputUuid, string hdcpMode);

    /// <summary>
    /// Adds a resolution change operation to the batch.
    /// </summary>
    IBatchOperation SetOutputResolution(string outputUuid, string resolution);

    /// <summary>
    /// Adds a color space change operation to the batch.
    /// </summary>
    IBatchOperation SetOutputColorSpace(string outputUuid, string colorSpace);

    /// <summary>
    /// Gets the number of operations currently in the batch.
    /// </summary>
    int OperationCount { get; }

    /// <summary>
    /// Clears all operations from the batch.
    /// </summary>
    void Clear();

    /// <summary>
    /// Executes all operations in the batch sequentially.
    /// </summary>
    Task<BatchOperationResult> ExecuteSequentialAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes all operations in the batch in parallel.
    /// </summary>
    Task<BatchOperationResult> ExecuteParallelAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes all operations with fail-fast mode (stops on first error).
    /// </summary>
    Task<BatchOperationResult> ExecuteFailFastAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Interface for managing batch operations.
/// </summary>
public interface IBatchManager
{
    /// <summary>
    /// Creates a new batch operation builder.
    /// </summary>
    IBatchOperation CreateBatch();
}
