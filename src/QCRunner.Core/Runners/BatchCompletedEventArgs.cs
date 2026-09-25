using QCRunner.Calculations.Common.Results;
using QCRunner.Core.Batches;

namespace QCRunner.Core.Runners;

public sealed class BatchCompletedEventArgs : EventArgs
{
    public BatchCompletedEventArgs(Batch batch, BatchRunState state, IReadOnlyList<ICalculationResult> calculationResults, Exception? exception = null)
    {
        Batch = batch;
        State = state;
        CalculationResults = calculationResults;
        Exception = exception;
    }

    public Batch Batch { get; }

    public BatchRunState State { get; }

    public IReadOnlyList<ICalculationResult> CalculationResults { get; }

    public Exception? Exception { get; }
}
