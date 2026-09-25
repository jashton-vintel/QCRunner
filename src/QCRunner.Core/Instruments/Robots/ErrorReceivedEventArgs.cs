namespace QCRunner.Core.Instruments.Robots;

public sealed class ErrorReceivedEventArgs : EventArgs
{
    public ErrorReceivedEventArgs(string message, int errorCode)
    {
        Message = message;
        ErrorCode = errorCode;
    }

    public string Message { get; }

    public int ErrorCode { get; }
}
