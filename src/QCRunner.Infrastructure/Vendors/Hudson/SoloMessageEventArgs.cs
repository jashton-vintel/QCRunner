namespace QCRunner.Infrastructure.Vendors.Hudson;

public sealed class SoloMessageEventArgs : EventArgs
{
    public SoloMessageEventArgs(string message)
    {
        Message = message;
    }

    public string Message { get; }
}
