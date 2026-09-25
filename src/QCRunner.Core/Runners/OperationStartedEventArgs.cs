using QCRunner.Core.Operations;

namespace QCRunner.Core.Runners;

public sealed class OperationStartedEventArgs : EventArgs
{
    public OperationStartedEventArgs(IOperation operation)
    {
        Operation = operation;
    }

    public IOperation Operation { get; }
}
