using QCRunner.Core.Operations;

namespace QCRunner.Core.Runners;

public class OperationResult : IOperationResult
{
    public OperationResult(IOperation operation, OperationResultStatus status, string message = "", Exception? exception = null)
    {
        ArgumentNullException.ThrowIfNull(operation);

        Operation = operation;
        Status = status;
        Message = message;
        Exception = exception;
    }

    public IOperation Operation { get; }

    public OperationResultStatus Status { get; }

    public string Message { get; }

    public Exception? Exception { get; }

    public static OperationResult Successful(IOperation operation) => new(operation, OperationResultStatus.Successful);

    public static OperationResult Cancelled(IOperation operation) => new(operation, OperationResultStatus.Cancelled, "The operation was cancelled.");

    public static OperationResult Failed(IOperation operation, string message, Exception? exception = null) =>
        new(operation, OperationResultStatus.Unsuccessful, message, exception);

    public override string ToString() => string.IsNullOrEmpty(Message) ? $"{Operation.Name}: {Status}" : $"{Operation.Name}: {Status} - {Message}";
}
