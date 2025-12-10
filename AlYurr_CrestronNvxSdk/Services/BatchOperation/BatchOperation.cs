using System.Diagnostics;
using Serilog;

namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Represents a single operation in a batch.
/// </summary>
internal class BatchOperationItem
{
    public required string OperationType { get; set; }
    public required Func<Task> Operation { get; set; }
    public required string Description { get; set; }
}

/// <summary>
/// Implementation of batch operation builder and executor.
/// </summary>
public class BatchOperation : IBatchOperation
{
    private readonly List<BatchOperationItem> _operations = new();
    private readonly IAudioVideoInputOutputManager? _audioVideoManager;
    private readonly IAvRoutingManager? _routingManager;
    private readonly ILogger _logger;

    /// <summary>
    /// Gets the number of operations in the batch.
    /// </summary>
    public int OperationCount => _operations.Count;

    public BatchOperation(
        IAudioVideoInputOutputManager? audioVideoManager,
        IAvRoutingManager? routingManager,
        ILogger? logger = null)
    {
        _audioVideoManager = audioVideoManager;
        _routingManager = routingManager;
        _logger = logger ?? Log.Logger;
    }

    public IBatchOperation SetAudioSource(string sourceUuid)
    {
        _operations.Add(new BatchOperationItem
        {
            OperationType = "SetAudioSource",
            Description = $"Set audio source to {sourceUuid}",
            Operation = async () =>
            {
                if (_routingManager != null)
                {
                    await _routingManager.SetAudioSourceAsync(sourceUuid);
                }
            }
        });
        return this;
    }

    public IBatchOperation SetVideoSource(string sourceUuid)
    {
        _operations.Add(new BatchOperationItem
        {
            OperationType = "SetVideoSource",
            Description = $"Set video source to {sourceUuid}",
            Operation = async () =>
            {
                if (_routingManager != null)
                {
                    await _routingManager.SetVideoSourceAsync(sourceUuid);
                }
            }
        });
        return this;
    }

    public IBatchOperation SetOutputVolume(string outputUuid, short volumeLevel)
    {
        _operations.Add(new BatchOperationItem
        {
            OperationType = "SetOutputVolume",
            Description = $"Set output {outputUuid} volume to {volumeLevel}",
            Operation = async () =>
            {
                if (_audioVideoManager != null)
                {
                    await _audioVideoManager.SetOutputVolumeAsync(outputUuid, volumeLevel);
                }
            }
        });
        return this;
    }

    public IBatchOperation SetOutputMute(string outputUuid, bool muted)
    {
        _operations.Add(new BatchOperationItem
        {
            OperationType = "SetOutputMute",
            Description = $"Set output {outputUuid} mute to {muted}",
            Operation = async () =>
            {
                if (_audioVideoManager != null)
                {
                    await _audioVideoManager.SetOutputMuteAsync(outputUuid, muted);
                }
            }
        });
        return this;
    }

    public IBatchOperation SetOutputHdcp(string outputUuid, string hdcpMode)
    {
        _operations.Add(new BatchOperationItem
        {
            OperationType = "SetOutputHdcp",
            Description = $"Set output {outputUuid} HDCP to {hdcpMode}",
            Operation = async () =>
            {
                if (_audioVideoManager != null)
                {
                    await _audioVideoManager.SetOutputHdcpAsync(outputUuid, hdcpMode);
                }
            }
        });
        return this;
    }

    public IBatchOperation SetOutputResolution(string outputUuid, string resolution)
    {
        _operations.Add(new BatchOperationItem
        {
            OperationType = "SetOutputResolution",
            Description = $"Set output {outputUuid} resolution to {resolution}",
            Operation = async () =>
            {
                if (_audioVideoManager != null)
                {
                    await _audioVideoManager.SetOutputResolutionAsync(outputUuid, resolution);
                }
            }
        });
        return this;
    }

    public IBatchOperation SetOutputColorSpace(string outputUuid, string colorSpace)
    {
        _operations.Add(new BatchOperationItem
        {
            OperationType = "SetOutputColorSpace",
            Description = $"Set output {outputUuid} color space to {colorSpace}",
            Operation = async () =>
            {
                if (_audioVideoManager != null)
                {
                    await _audioVideoManager.SetOutputColorSpaceAsync(outputUuid, colorSpace);
                }
            }
        });
        return this;
    }

    public void Clear()
    {
        _operations.Clear();
    }

    public async Task<BatchOperationResult> ExecuteSequentialAsync(CancellationToken cancellationToken = default)
    {
        var result = new BatchOperationResult();
        var stopwatch = Stopwatch.StartNew();

        _logger.Information("Starting sequential batch execution with {Count} operations", _operations.Count);

        for (int i = 0; i < _operations.Count; i++)
        {
            var operation = _operations[i];
            try
            {
                _logger.Debug("Executing operation {Index}/{Total}: {Description}", i + 1, _operations.Count, operation.Description);
                await operation.Operation();
                result.SuccessfulOperations++;
            }
            catch (Exception ex)
            {
                result.FailedOperations++;
                result.Errors.Add(new OperationError
                {
                    OperationIndex = i,
                    OperationType = operation.OperationType,
                    ErrorMessage = ex.Message,
                    Exception = ex
                });
                _logger.Error(ex, "Operation {Index} failed: {Description}", i, operation.Description);
            }
        }

        stopwatch.Stop();
        result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;

        _logger.Information(
            "Sequential batch execution completed: {Successful} successful, {Failed} failed in {Ms}ms",
            result.SuccessfulOperations,
            result.FailedOperations,
            result.ExecutionTimeMs);

        return result;
    }

    public async Task<BatchOperationResult> ExecuteParallelAsync(CancellationToken cancellationToken = default)
    {
        var result = new BatchOperationResult();
        var stopwatch = Stopwatch.StartNew();

        _logger.Information("Starting parallel batch execution with {Count} operations", _operations.Count);

        var tasks = _operations.Select(async (operation, index) =>
        {
            try
            {
                _logger.Debug("Executing operation {Index}: {Description}", index, operation.Description);
                await operation.Operation();
                return (Success: true, Index: index, operation, Error: (OperationError?)null);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Operation {Index} failed: {Description}", index, operation.Description);
                return (Success: false, Index: index, operation, Error: new OperationError
                {
                    OperationIndex = index,
                    OperationType = operation.OperationType,
                    ErrorMessage = ex.Message,
                    Exception = ex
                });
            }
        }).ToList();

        var results = await Task.WhenAll(tasks);

        foreach (var r in results)
        {
            if (r.Success)
                result.SuccessfulOperations++;
            else
            {
                result.FailedOperations++;
                if (r.Error != null)
                    result.Errors.Add(r.Error);
            }
        }

        stopwatch.Stop();
        result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;

        _logger.Information(
            "Parallel batch execution completed: {Successful} successful, {Failed} failed in {Ms}ms",
            result.SuccessfulOperations,
            result.FailedOperations,
            result.ExecutionTimeMs);

        return result;
    }

    public async Task<BatchOperationResult> ExecuteFailFastAsync(CancellationToken cancellationToken = default)
    {
        var result = new BatchOperationResult();
        var stopwatch = Stopwatch.StartNew();

        _logger.Information("Starting fail-fast batch execution with {Count} operations", _operations.Count);

        for (int i = 0; i < _operations.Count; i++)
        {
            var operation = _operations[i];
            try
            {
                _logger.Debug("Executing operation {Index}/{Total}: {Description}", i + 1, _operations.Count, operation.Description);
                await operation.Operation();
                result.SuccessfulOperations++;
            }
            catch (Exception ex)
            {
                result.FailedOperations++;
                result.Errors.Add(new OperationError
                {
                    OperationIndex = i,
                    OperationType = operation.OperationType,
                    ErrorMessage = ex.Message,
                    Exception = ex
                });
                _logger.Error(ex, "Batch execution stopped at operation {Index}: {Description}", i, operation.Description);

                stopwatch.Stop();
                result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;
                return result; // Stop on first error
            }
        }

        stopwatch.Stop();
        result.ExecutionTimeMs = stopwatch.ElapsedMilliseconds;

        _logger.Information(
            "Fail-fast batch execution completed: {Successful} successful in {Ms}ms",
            result.SuccessfulOperations,
            result.ExecutionTimeMs);

        return result;
    }
}
