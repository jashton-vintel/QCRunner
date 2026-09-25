using Microsoft.Extensions.Logging;
using QCRunner.Infrastructure.Simulation;

namespace QCRunner.Infrastructure.Vendors.Hidex;

/// <summary>
/// Stands in for the plate reader vendor SDK, which is not distributed with this solution.
/// From the outside it behaves like the real API: it tracks connection state, reports a serial
/// number and raises one measurement result per well per cycle.
/// </summary>
public sealed class HidexSenseApiStub : IHidexSenseApi
{
    private const int StartingWavelength = 300;
    private const int WavelengthStep = 1;
    private const int SpectrumPointCount = 501;
    private const int Seed = 2024;

    private static readonly TimeSpan WellReadTime = TimeSpan.FromMilliseconds(50);
    private static readonly TimeSpan TrayTime = TimeSpan.FromMilliseconds(500);

    private readonly SimulatedSignalGenerator _signals = new(Seed);
    private readonly ILogger<HidexSenseApiStub> _logger;

    public HidexSenseApiStub(ILogger<HidexSenseApiStub> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public event EventHandler<HidexMeasurementResultEventArgs>? MeasurementResults;

    public bool IsConnected { get; private set; }

    public string SerialNumber => "HSX-000123";

    public string FirmwareVersion => "3.2.0-stub";

    public double CurrentTemperature { get; private set; } = 22.0;

    public async Task ConnectAsync(string port, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Vendor SDK: opening {Port}", port);
        await Task.Delay(TrayTime, cancellationToken).ConfigureAwait(false);
        IsConnected = true;
    }

    public Task StandbyAsync(CancellationToken cancellationToken)
    {
        IsConnected = false;
        return Task.CompletedTask;
    }

    public Task LoadTrayAsync(CancellationToken cancellationToken)
    {
        return Task.Delay(TrayTime, cancellationToken);
    }

    public Task UnloadTrayAsync(int tension, CancellationToken cancellationToken)
    {
        return Task.Delay(TrayTime, cancellationToken);
    }

    public async Task StartHeatingWithTargetTemperatureAsync(double temperature, bool waitUntilReached, CancellationToken cancellationToken)
    {
        if (waitUntilReached)
        {
            await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken).ConfigureAwait(false);
        }

        CurrentTemperature = temperature;
    }

    public Task StopHeatingAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    public async Task RunAbsorbanceAsync(int flashes, HidexPlateDimensions plate, IReadOnlyList<int> wellMask, int cycles, TimeSpan cycleInterval, CancellationToken cancellationToken)
    {
        DateTime started = DateTime.Now;

        for (int cycle = 1; cycle <= cycles; cycle++)
        {
            foreach (int wellIndex in wellMask)
            {
                await Task.Delay(WellReadTime, cancellationToken).ConfigureAwait(false);

                IReadOnlyList<double> spectrum = _signals.AbsorbanceSpectrum(wellIndex, SpectrumPointCount);
                MeasurementResults?.Invoke(this, new HidexMeasurementResultEventArgs(HidexMeasurementTechnology.Absorbance, wellIndex, cycle, DateTime.Now - started, StartingWavelength, WavelengthStep, spectrum));
            }

            if (cycle < cycles)
            {
                await Task.Delay(cycleInterval, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    public async Task RunLuminescenceAsync(TimeSpan countingTime, HidexPlateDimensions plate, IReadOnlyList<int> wellMask, int cycles, CancellationToken cancellationToken)
    {
        DateTime started = DateTime.Now;

        for (int cycle = 1; cycle <= cycles; cycle++)
        {
            foreach (int wellIndex in wellMask)
            {
                await Task.Delay(WellReadTime, cancellationToken).ConfigureAwait(false);

                double counts = _signals.LuminescenceCounts(wellIndex, cycle);
                MeasurementResults?.Invoke(this, new HidexMeasurementResultEventArgs(HidexMeasurementTechnology.Luminescence, wellIndex, cycle, DateTime.Now - started, 0, 1, new[] { counts }));
            }
        }
    }

    public Task RunShakeAsync(TimeSpan duration, HidexPlateDimensions plate, HidexShakeIntensity intensity, int cycles, CancellationToken cancellationToken)
    {
        return Task.Delay(TrayTime, cancellationToken);
    }
}
