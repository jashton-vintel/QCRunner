using Microsoft.Extensions.Logging;
using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Core.Instruments.Readers;
using QCRunner.Core.Operations.Reader;
using QCRunner.Core.ReaderData;
using QCRunner.Infrastructure.Instruments;
using QCRunner.Infrastructure.Simulation;

namespace QCRunner.Infrastructure.Readers;

/// <summary>
/// A plate reader for exhibition shows and training. It needs no hardware, completes every
/// operation after a short delay and reports well-formed readings that produce valid, passing
/// calculations with good values for demonstration purposes.
/// </summary>
public sealed class SimulationReader : InstrumentBase, IReader
{
    private const int StartingWavelength = 300;
    private const int WavelengthStep = 1;
    private const int SpectrumPointCount = 501;
    private const int Seed = 2024;

    private static readonly TimeSpan WellReadTime = TimeSpan.FromMilliseconds(50);
    private static readonly TimeSpan TrayTime = TimeSpan.FromMilliseconds(500);
    private static readonly TimeSpan HeatingTime = TimeSpan.FromSeconds(1);

    private readonly SimulatedSignalGenerator _signals = new(Seed);

    public SimulationReader(ILogger<SimulationReader> logger)
        : base("simulated plate reader", logger)
    {
        IsConnected = true;
    }

    public event EventHandler<ReaderDataReceivedEventArgs>? ReaderDataReceived;

    public event EventHandler<ReaderErrorEventArgs>? ReaderError;

    public override string SerialNumber => "SIM-READER-001";

    public Task PerformAbsorbanceAsync(AbsorbanceOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"absorbance ({operation.Wells.Count} wells, {operation.Cycles} cycles)", async token =>
        {
            DateTime started = DateTime.Now;

            for (int cycle = 1; cycle <= operation.Cycles; cycle++)
            {
                foreach (WellCode well in operation.Wells)
                {
                    await Task.Delay(WellReadTime, token).ConfigureAwait(false);

                    IReadOnlyList<double> spectrum = _signals.AbsorbanceSpectrum((int)well, SpectrumPointCount);
                    Publish(new AbsorbanceData(well, cycle, DateTime.Now, DateTime.Now - started, StartingWavelength, WavelengthStep, spectrum));
                }
            }
        }, cancellationToken);
    }

    public Task PerformLuminescenceAsync(LuminescenceOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"luminescence ({operation.Wells.Count} wells, {operation.Cycles} cycles)", async token =>
        {
            DateTime started = DateTime.Now;

            for (int cycle = 1; cycle <= operation.Cycles; cycle++)
            {
                foreach (WellCode well in operation.Wells)
                {
                    await Task.Delay(WellReadTime, token).ConfigureAwait(false);

                    double counts = _signals.LuminescenceCounts((int)well, cycle);
                    Publish(new LuminescenceData(well, cycle, DateTime.Now, DateTime.Now - started, counts));
                }
            }
        }, cancellationToken);
    }

    public Task PerformHeatToTemperatureAsync(HeatToTemperatureOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"heat to {operation.Temperature:0.#} C ({operation.Mode})", token =>
            operation.Mode == HeatMode.WaitUntilReached ? Task.Delay(HeatingTime, token) : Task.CompletedTask, cancellationToken);
    }

    public Task PerformShakeAsync(ShakeOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"shake ({operation.Intensity}, {operation.ShakeTime.TotalSeconds:0} s x {operation.ShakeCycles})", token =>
            Task.Delay(TrayTime, token), cancellationToken);
    }

    public Task EjectAsync(int tension, CancellationToken cancellationToken = default)
    {
        return ExecuteCommandAsync($"eject tray (tension {tension})", token => Task.Delay(TrayTime, token), cancellationToken);
    }

    public Task LoadAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteCommandAsync("load tray", token => Task.Delay(TrayTime, token), cancellationToken);
    }

    public Task StandbyAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteCommandAsync("standby", _ => Task.CompletedTask, cancellationToken);
    }

    protected override Task ConnectCoreAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    protected override Task DisconnectCoreAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    protected override void OnCommandFailed(string commandName, Exception exception)
    {
        ReaderError?.Invoke(this, new ReaderErrorEventArgs($"The {commandName} command failed: {exception.Message}", exception));
    }

    private void Publish(IReaderData data)
    {
        ReaderDataReceived?.Invoke(this, new ReaderDataReceivedEventArgs(data));
    }
}
