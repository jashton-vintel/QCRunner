using QCRunner.Core.Operations.Robot;

namespace QCRunner.Core.Instruments.Robots;

/// <summary>
/// A pipetting robot. Every command is queued behind the previous one so callers on different
/// threads never interleave instrument traffic.
/// </summary>
public interface IRobot : IInstrument
{
    event EventHandler<MessageReceivedEventArgs>? MessageReceived;

    event EventHandler<ErrorReceivedEventArgs>? ErrorReceived;

    /// <summary>Moves one axis by a distance in millimetres. Negative distances move the axis the other way.</summary>
    Task JogAsync(JogDirection direction, double distance, CancellationToken cancellationToken = default);

    Task AspirateAsync(AspirateOperation operation, CancellationToken cancellationToken = default);

    Task DispenseAsync(DispenseOperation operation, CancellationToken cancellationToken = default);

    Task MixAsync(int position, double volume, int cycles, CancellationToken cancellationToken = default);

    Task SetPlateTemperatureAsync(HeatedPlateOperation operation, CancellationToken cancellationToken = default);

    /// <summary>Sends a raw command in the vendor protocol and returns the reply.</summary>
    Task<string> SendTextAsync(string command, CancellationToken cancellationToken = default);

    Task HomeAsync(CancellationToken cancellationToken = default);
}
