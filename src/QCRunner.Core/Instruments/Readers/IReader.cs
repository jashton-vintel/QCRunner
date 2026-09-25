using QCRunner.Core.Operations.Reader;

namespace QCRunner.Core.Instruments.Readers;

/// <summary>
/// A microplate reader. Measurement methods complete when the instrument has finished the
/// operation; the readings themselves arrive through <see cref="ReaderDataReceived"/> as each
/// well is measured.
/// </summary>
public interface IReader : IInstrument
{
    event EventHandler<ReaderDataReceivedEventArgs>? ReaderDataReceived;

    event EventHandler<ReaderErrorEventArgs>? ReaderError;

    Task PerformAbsorbanceAsync(AbsorbanceOperation operation, CancellationToken cancellationToken = default);

    Task PerformLuminescenceAsync(LuminescenceOperation operation, CancellationToken cancellationToken = default);

    Task PerformHeatToTemperatureAsync(HeatToTemperatureOperation operation, CancellationToken cancellationToken = default);

    Task PerformShakeAsync(ShakeOperation operation, CancellationToken cancellationToken = default);

    /// <summary>Opens the tray. A non-zero tension keeps the plate clamped while it is ejected.</summary>
    Task EjectAsync(int tension, CancellationToken cancellationToken = default);

    Task LoadAsync(CancellationToken cancellationToken = default);

    Task StandbyAsync(CancellationToken cancellationToken = default);
}
