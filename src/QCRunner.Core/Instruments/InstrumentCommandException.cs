namespace QCRunner.Core.Instruments;

/// <summary>
/// Raised when an instrument rejects or fails a command.
/// </summary>
public sealed class InstrumentCommandException : Exception
{
    public InstrumentCommandException(string instrumentName, string message, int errorCode = 0)
        : base($"The {instrumentName} reported an error: {message}")
    {
        InstrumentName = instrumentName;
        ErrorCode = errorCode;
    }

    public string InstrumentName { get; }

    public int ErrorCode { get; }
}
