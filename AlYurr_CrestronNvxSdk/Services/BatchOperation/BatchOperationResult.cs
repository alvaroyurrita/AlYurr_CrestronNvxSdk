namespace AlYurr_CrestronNvxSdk.Services;

/// <summary>
/// Represents the result of a batch operation execution.
/// </summary>
public class BatchOperationResult
{
    /// <summary>
    /// Gets or sets the number of successfully executed operations.
    /// </summary>
    public int SuccessfulOperations { get; set; }

    /// <summary>
    /// Gets or sets the number of failed operations.
    /// </summary>
    public int FailedOperations { get; set; }

    /// <summary>
    /// Gets or sets the list of errors encountered during execution.
    /// </summary>
    public List<OperationError> Errors { get; set; } = new();

    /// <summary>
    /// Gets or sets the execution time in milliseconds.
    /// </summary>
    public long ExecutionTimeMs { get; set; }

    /// <summary>
    /// Gets a value indicating whether all operations succeeded.
    /// </summary>
    public bool IsSuccessful => FailedOperations == 0;

    /// <summary>
    /// Gets the total number of operations executed.
    /// </summary>
    public int TotalOperations => SuccessfulOperations + FailedOperations;
}

/// <summary>
/// Represents an error that occurred during a batch operation.
/// </summary>
public class OperationError
{
    /// <summary>
    /// Gets or sets the index of the operation that failed.
    /// </summary>
    public int OperationIndex { get; set; }

    /// <summary>
    /// Gets or sets the type of operation that failed.
    /// </summary>
    public string? OperationType { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Gets or sets the exception that was thrown.
    /// </summary>
    public Exception? Exception { get; set; }
}

/// <summary>
/// Execution mode for batch operations.
/// </summary>
public enum BatchExecutionMode
{
    /// <summary>Execute operations sequentially, one after another.</summary>
    Sequential,

    /// <summary>Execute operations in parallel when possible.</summary>
    Parallel,

    /// <summary>Stop on first error (fail-fast mode).</summary>
    FailFast
}
