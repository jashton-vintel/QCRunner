using QCRunner.Core.Operations;
using QCRunner.Core.ReaderData;

namespace QCRunner.Core.Runners;

/// <summary>
/// Runs operations one at a time against the hardware and services behind them. Callers can
/// run a single operation directly or queue a sequence and run the queue; pausing takes effect
/// between operations because an instrument command cannot be suspended half way through.
/// </summary>
public interface IOperationRunner : IDisposable
{
    event EventHandler<OperationStartedEventArgs>? OperationStarted;

    event EventHandler<OperationCompletedEventArgs>? OperationCompleted;

    IOperation? CurrentOperation { get; }

    bool IsPaused { get; }

    int QueuedOperationCount { get; }

    /// <summary>Everything the reader has reported so far, available for raw data viewing.</summary>
    IReaderDataStore ReaderData { get; }

    void Enqueue(IOperation operation);

    void EnqueueRange(IEnumerable<IOperation> operations);

    /// <summary>
    /// Runs queued operations in order until the queue is empty or an operation does not
    /// succeed, in which case the remaining operations are discarded.
    /// </summary>
    Task<IReadOnlyList<IOperationResult>> RunQueuedAsync(CancellationToken cancellationToken = default);

    Task<IOperationResult> RunAsync(IOperation operation, CancellationToken cancellationToken = default);

    void Pause();

    void Resume();
}
