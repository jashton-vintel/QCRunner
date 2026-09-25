namespace QCRunner.Infrastructure.Vendors.Hudson;

/// <summary>
/// The slice of the pipetting robot vendor SDK the application uses. Every command returns a
/// <see cref="SoloCommandResult"/> rather than throwing, which is how the real SDK reports
/// failures.
/// </summary>
public interface ISoloApi
{
    bool IsConnected { get; }

    string SerialNumber { get; }

    string FirmwareVersion { get; }

    event EventHandler<SoloMessageEventArgs>? Message;

    event EventHandler<SoloDeviceErrorEventArgs>? DeviceError;

    Task<SoloCommandResult> ConnectAsync(string port, CancellationToken cancellationToken);

    Task<SoloCommandResult> DisconnectAsync(CancellationToken cancellationToken);

    Task<SoloCommandResult> HomeAsync(CancellationToken cancellationToken);

    Task<SoloCommandResult> MoveToTravelHeightAsync(CancellationToken cancellationToken);

    Task<SoloCommandResult> AspirateAsync(int position, string point, double volume, double height, SoloOffsets offsets, CancellationToken cancellationToken);

    Task<SoloCommandResult> DispenseAsync(int position, string point, double volume, double height, SoloOffsets offsets, CancellationToken cancellationToken);

    Task<SoloCommandResult> MixAsync(int position, double volume, int cycles, CancellationToken cancellationToken);

    Task<SoloCommandResult> JogAxisAsync(string axis, double distance, CancellationToken cancellationToken);

    Task<SoloCommandResult> SendTextAsync(string command, CancellationToken cancellationToken);

    Task<SoloCommandResult> SetTemperatureAsync(int position, double temperature, CancellationToken cancellationToken);

    /// <summary>Stops the current movement immediately.</summary>
    void Halt();
}
