namespace QCRunner.Core.Instruments;

public sealed class InstrumentNotConnectedException : InvalidOperationException
{
    public InstrumentNotConnectedException(string instrumentName)
        : base($"The {instrumentName} is not connected.")
    {
        InstrumentName = instrumentName;
    }

    public string InstrumentName { get; }
}
