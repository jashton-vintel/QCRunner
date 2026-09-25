using QCRunner.Calculations.Common.Results;
using QCRunner.Core.Batches;

namespace QCRunner.Core.Runners;

/// <summary>
/// Runs a whole batch: every phase of the method in order, each phase as a queue of operations,
/// with the operator confirming the start of each phase after the first. Events are raised on
/// the thread the batch is running on; a UI marshals them to its own thread.
/// </summary>
public interface IBatchRunner : IDisposable
{
    Batch Batch { get; }

    BatchRunState State { get; }

    IReadOnlyList<ICalculationResult> CalculationResults { get; }

    IReadOnlyList<IOperationResult> OperationResults { get; }

    event EventHandler? BatchStarted;

    event EventHandler<PhaseEventArgs>? PhaseStarted;

    event EventHandler<OperationStartedEventArgs>? OperationStarted;

    event EventHandler<OperationCompletedEventArgs>? OperationCompleted;

    event EventHandler<PhaseEventArgs>? PhaseCompleted;

    event EventHandler<BatchCompletedEventArgs>? BatchCompleted;

    /// <summary>Runs the batch to completion. The returned task completes when the batch has finished, failed or been cancelled.</summary>
    Task RunAsync(CancellationToken cancellationToken = default);

    void Pause();

    void Resume();

    void Cancel();
}
