using QCRunner.Core.Operations;

namespace QCRunner.Core.Runners;

public interface IOperationResult
{
    IOperation Operation { get; }

    OperationResultStatus Status { get; }

    string Message { get; }

    Exception? Exception { get; }
}
