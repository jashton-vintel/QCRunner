using QCRunner.Core.Operations;

namespace QCRunner.Core.Runners;

public sealed class OperationCompletedEventArgs : EventArgs
{
    public OperationCompletedEventArgs(IOperation operation, IOperationResult result)
    {
        Operation = operation;
        Result = result;
    }

    public IOperation Operation { get; }

    public IOperationResult Result { get; }
}
