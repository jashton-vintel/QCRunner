namespace QCRunner.Infrastructure.Vendors.Hudson;

public sealed class SoloDeviceErrorEventArgs : EventArgs
{
    public SoloDeviceErrorEventArgs(string command, int errorCode, string message)
    {
        Command = command;
        ErrorCode = errorCode;
        Message = message;
    }

    public string Command { get; }

    public int ErrorCode { get; }

    public string Message { get; }
}
