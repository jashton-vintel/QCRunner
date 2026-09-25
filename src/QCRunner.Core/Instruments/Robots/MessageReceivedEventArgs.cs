namespace QCRunner.Core.Instruments.Robots;

public sealed class MessageReceivedEventArgs : EventArgs
{
    public MessageReceivedEventArgs(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
