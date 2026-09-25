namespace QCRunner.Core.Instruments;

public interface IInstrument
{
    string SerialNumber { get; }

    bool IsConnected { get; }

    Task ConnectAsync(CancellationToken cancellationToken = default);

    Task DisconnectAsync(CancellationToken cancellationToken = default);
}
