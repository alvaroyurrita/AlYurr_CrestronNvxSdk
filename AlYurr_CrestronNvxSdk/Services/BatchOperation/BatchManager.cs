using Serilog;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Implementation of batch operation manager.
/// </summary>
public class BatchManager : IBatchManager
{
    private readonly IAudioVideoInputOutputManager? _audioVideoManager;
    private readonly IAvRoutingManager? _routingManager;
    private readonly ILogger _logger;

    public BatchManager(
        IAudioVideoInputOutputManager? audioVideoManager = null,
        IAvRoutingManager? routingManager = null,
        ILogger? logger = null)
    {
        _audioVideoManager = audioVideoManager;
        _routingManager = routingManager;
        _logger = logger ?? Log.Logger;
    }

    /// <summary>
    /// Creates a new batch operation builder.
    /// </summary>
    public IBatchOperation CreateBatch()
    {
        _logger.Debug("Creating new batch operation");
        return new BatchOperation(_audioVideoManager, _routingManager, _logger);
    }
}
