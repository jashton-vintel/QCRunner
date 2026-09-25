namespace QCRunner.Core.Instruments.Readers;

public sealed class ReaderErrorEventArgs : EventArgs
{
    public ReaderErrorEventArgs(string message, Exception? exception = null)
    {
        Message = message;
        Exception = exception;
    }

    public string Message { get; }

    public Exception? Exception { get; }
}
