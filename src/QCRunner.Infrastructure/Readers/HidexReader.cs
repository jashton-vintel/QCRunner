using Microsoft.Extensions.Logging;
using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Core.Instruments.Readers;
using QCRunner.Core.Labware;
using QCRunner.Core.Operations.Reader;
using QCRunner.Core.ReaderData;
using QCRunner.Infrastructure.Instruments;
using QCRunner.Infrastructure.Vendors.Hidex;

namespace QCRunner.Infrastructure.Readers;

/// <summary>
/// The production plate reader. Each operation is translated into the vendor API calls and the
/// vendor measurement events are translated back into <see cref="IReaderData"/>.
/// </summary>
public sealed class HidexReader : InstrumentBase, IReader
{
    private const string NotConnected = "(not connected)";

    private readonly IHidexSenseApi _api;
    private readonly HidexReaderOptions _options;
    private PlateOperationBase? _currentOperation;

    public HidexReader(IHidexSenseApi api, HidexReaderOptions options, ILogger<HidexReader> logger)
        : base("plate reader", logger)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(options);

        _api = api;
        _options = options;
        _api.MeasurementResults += OnMeasurementResults;
    }

    public event EventHandler<ReaderDataReceivedEventArgs>? ReaderDataReceived;

    public event EventHandler<ReaderErrorEventArgs>? ReaderError;

    public override string SerialNumber => IsConnected ? _api.SerialNumber : NotConnected;

    public Task PerformAbsorbanceAsync(AbsorbanceOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"absorbance ({operation.Wells.Count} wells, {operation.Cycles} cycles)", token =>
        {
            _currentOperation = operation;

            return _api.RunAbsorbanceAsync(
                operation.Flashes,
                ToPlateDimensions(operation.Plate),
                ToWellMask(operation),
                operation.Cycles,
                TimeSpan.FromMilliseconds(operation.Delay),
                token);
        }, cancellationToken);
    }

    public Task PerformLuminescenceAsync(LuminescenceOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"luminescence ({operation.Wells.Count} wells, {operation.Cycles} cycles)", token =>
        {
            _currentOperation = operation;

            return _api.RunLuminescenceAsync(
                operation.CountingTime,
                ToPlateDimensions(operation.Plate),
                ToWellMask(operation),
                operation.Cycles,
                token);
        }, cancellationToken);
    }

    public Task PerformHeatToTemperatureAsync(HeatToTemperatureOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"heat to {operation.Temperature:0.#} C ({operation.Mode})", token =>
            _api.StartHeatingWithTargetTemperatureAsync(operation.Temperature, operation.Mode == HeatMode.WaitUntilReached, token), cancellationToken);
    }

    public Task PerformShakeAsync(ShakeOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"shake ({operation.Intensity}, {operation.ShakeTime.TotalSeconds:0} s x {operation.ShakeCycles})", token =>
            _api.RunShakeAsync(operation.ShakeTime, ToPlateDimensions(operation.Plate), ToVendorIntensity(operation.Intensity), operation.ShakeCycles, token), cancellationToken);
    }

    public Task EjectAsync(int tension, CancellationToken cancellationToken = default)
    {
        return ExecuteCommandAsync($"eject tray (tension {tension})", token => _api.UnloadTrayAsync(tension, token), cancellationToken);
    }

    public Task LoadAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteCommandAsync("load tray", _api.LoadTrayAsync, cancellationToken);
    }

    public Task StandbyAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteCommandAsync("standby", _api.StandbyAsync, cancellationToken);
    }

    public override void Dispose()
    {
        _api.MeasurementResults -= OnMeasurementResults;
        base.Dispose();
    }

    protected override Task ConnectCoreAsync(CancellationToken cancellationToken)
    {
        return _api.ConnectAsync(_options.Port, cancellationToken);
    }

    protected override Task DisconnectCoreAsync(CancellationToken cancellationToken)
    {
        return _api.StandbyAsync(cancellationToken);
    }

    protected override void OnCommandFailed(string commandName, Exception exception)
    {
        ReaderError?.Invoke(this, new ReaderErrorEventArgs($"The {commandName} command failed: {exception.Message}", exception));
    }

    /// <summary>
    /// The tray holds the plate rotated relative to the robot deck, so the row and column
    /// offsets swap when the geometry is handed to the reader.
    /// </summary>
    private static HidexPlateDimensions ToPlateDimensions(ILabware plate)
    {
        return new HidexPlateDimensions(plate.RowCount, plate.ColumnCount, plate.ColumnOffset, plate.RowOffset, plate.WellSpacing, plate.WellDiameter);
    }

    private static IReadOnlyList<int> ToWellMask(MeasurementOperationBase operation)
    {
        int columns = operation.Plate.ColumnCount;

        return operation.Wells.Select(well => well.RowIndex() * columns + well.ColumnIndex()).ToList();
    }

    private static HidexShakeIntensity ToVendorIntensity(ShakeIntensity intensity)
    {
        return intensity switch
        {
            ShakeIntensity.Low => HidexShakeIntensity.Gentle,
            ShakeIntensity.Medium => HidexShakeIntensity.Normal,
            ShakeIntensity.High => HidexShakeIntensity.Fast,
            _ => throw new ArgumentOutOfRangeException(nameof(intensity), intensity, "Unknown shake intensity.")
        };
    }

    private void OnMeasurementResults(object? sender, HidexMeasurementResultEventArgs e)
    {
        PlateOperationBase? operation = _currentOperation;

        if (operation is null)
        {
            Logger.LogWarning("Discarding a reader result for well index {WellIndex} because no measurement is running", e.WellIndex);
            return;
        }

        int columns = operation.Plate.ColumnCount;
        WellCode well = WellCodeExtensions.FromRowAndColumn(e.WellIndex / columns, e.WellIndex % columns);
        DateTime measuredAt = DateTime.Now;

        IReaderData data = e.Technology switch
        {
            HidexMeasurementTechnology.Absorbance => new AbsorbanceData(well, e.Cycle, measuredAt, e.ElapsedTime, e.StartingWavelength, e.WavelengthStep, e.Values),
            HidexMeasurementTechnology.Luminescence => new LuminescenceData(well, e.Cycle, measuredAt, e.ElapsedTime, e.Values[0]),
            _ => throw new NotSupportedException($"Measurement technology {e.Technology} is not supported.")
        };

        ReaderDataReceived?.Invoke(this, new ReaderDataReceivedEventArgs(data));
    }
}
