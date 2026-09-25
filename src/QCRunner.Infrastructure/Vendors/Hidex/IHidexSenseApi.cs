namespace QCRunner.Infrastructure.Vendors.Hidex;

/// <summary>
/// The slice of the plate reader vendor SDK the application uses, expressed as an interface so
/// the reader can be tested without the SDK and so a stub can stand in for it here.
/// </summary>
public interface IHidexSenseApi
{
    bool IsConnected { get; }

    string SerialNumber { get; }

    string FirmwareVersion { get; }

    double CurrentTemperature { get; }

    /// <summary>Raised once per well per cycle while a measurement runs.</summary>
    event EventHandler<HidexMeasurementResultEventArgs>? MeasurementResults;

    Task ConnectAsync(string port, CancellationToken cancellationToken);

    Task StandbyAsync(CancellationToken cancellationToken);

    Task LoadTrayAsync(CancellationToken cancellationToken);

    Task UnloadTrayAsync(int tension, CancellationToken cancellationToken);

    Task StartHeatingWithTargetTemperatureAsync(double temperature, bool waitUntilReached, CancellationToken cancellationToken);

    Task StopHeatingAsync(CancellationToken cancellationToken);

    Task RunAbsorbanceAsync(int flashes, HidexPlateDimensions plate, IReadOnlyList<int> wellMask, int cycles, TimeSpan cycleInterval, CancellationToken cancellationToken);

    Task RunLuminescenceAsync(TimeSpan countingTime, HidexPlateDimensions plate, IReadOnlyList<int> wellMask, int cycles, CancellationToken cancellationToken);

    Task RunShakeAsync(TimeSpan duration, HidexPlateDimensions plate, HidexShakeIntensity intensity, int cycles, CancellationToken cancellationToken);
}
