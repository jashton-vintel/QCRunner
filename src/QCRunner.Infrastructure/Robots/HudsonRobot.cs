using Microsoft.Extensions.Logging;
using QCRunner.Core.Instruments;
using QCRunner.Core.Instruments.Robots;
using QCRunner.Core.Operations.Robot;
using QCRunner.Infrastructure.Instruments;
using QCRunner.Infrastructure.Vendors.Hudson;

namespace QCRunner.Infrastructure.Robots;

/// <summary>
/// The production pipetting robot. Liquid handling operations are broken into the vendor
/// primitives (move to travel height, aspirate or dispense, mix) and every vendor result is
/// checked so a rejected command surfaces as an exception on the operation.
/// </summary>
public sealed class HudsonRobot : InstrumentBase, IRobot
{
    private const string NotConnected = "(not connected)";
    private const double MaximumTipVolume = 200.0;

    private readonly ISoloApi _api;
    private readonly HudsonRobotOptions _options;

    public HudsonRobot(ISoloApi api, HudsonRobotOptions options, ILogger<HudsonRobot> logger)
        : base("pipetting robot", logger)
    {
        ArgumentNullException.ThrowIfNull(api);
        ArgumentNullException.ThrowIfNull(options);

        _api = api;
        _options = options;
        _api.Message += OnMessage;
        _api.DeviceError += OnDeviceError;
    }

    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;

    public event EventHandler<ErrorReceivedEventArgs>? ErrorReceived;

    public override string SerialNumber => IsConnected ? _api.SerialNumber : NotConnected;

    public Task JogAsync(JogDirection direction, double distance, CancellationToken cancellationToken = default)
    {
        return ExecuteCommandAsync($"jog {direction} by {distance:0.##} mm", async token =>
            Check(await _api.JogAxisAsync(direction.ToString(), distance, token).ConfigureAwait(false)), cancellationToken);
    }

    public Task AspirateAsync(AspirateOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"aspirate {operation.Volume:0.#} uL from {operation.Position}:{operation.Point}", async token =>
        {
            EnsureVolumeFitsTip(operation.Volume);

            Check(await _api.MoveToTravelHeightAsync(token).ConfigureAwait(false));
            Check(await _api.AspirateAsync(operation.Position, operation.Point, operation.Volume, operation.AspirateHeight, ToOffsets(operation), token).ConfigureAwait(false));

            if (operation.MixCycles > 0)
            {
                Check(await _api.MixAsync(operation.Position, operation.MixVolume, operation.MixCycles, token).ConfigureAwait(false));
            }
        }, cancellationToken);
    }

    public Task DispenseAsync(DispenseOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"dispense {operation.Volume:0.#} uL to {operation.Position}:{operation.Point}", async token =>
        {
            Check(await _api.MoveToTravelHeightAsync(token).ConfigureAwait(false));
            Check(await _api.DispenseAsync(operation.Position, operation.Point, operation.Volume, operation.DispenseHeight, ToOffsets(operation), token).ConfigureAwait(false));

            if (operation.MixCycles > 0)
            {
                Check(await _api.MixAsync(operation.Position, operation.MixVolume, operation.MixCycles, token).ConfigureAwait(false));
            }
        }, cancellationToken);
    }

    public Task MixAsync(int position, double volume, int cycles, CancellationToken cancellationToken = default)
    {
        return ExecuteCommandAsync($"mix {cycles} x {volume:0.#} uL at position {position}", async token =>
        {
            EnsureVolumeFitsTip(volume);
            Check(await _api.MixAsync(position, volume, cycles, token).ConfigureAwait(false));
        }, cancellationToken);
    }

    public Task SetPlateTemperatureAsync(HeatedPlateOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return ExecuteCommandAsync($"set nest {operation.Position} to {operation.Temperature:0.#} C", async token =>
            Check(await _api.SetTemperatureAsync(operation.Position, operation.Temperature, token).ConfigureAwait(false)), cancellationToken);
    }

    public Task<string> SendTextAsync(string command, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command);

        return ExecuteCommandAsync($"send '{command}'", async token =>
            Check(await _api.SendTextAsync(command, token).ConfigureAwait(false)).Response, cancellationToken);
    }

    public Task HomeAsync(CancellationToken cancellationToken = default)
    {
        return ExecuteCommandAsync("home", async token => Check(await _api.HomeAsync(token).ConfigureAwait(false)), cancellationToken);
    }

    public override void Dispose()
    {
        _api.Message -= OnMessage;
        _api.DeviceError -= OnDeviceError;
        base.Dispose();
    }

    protected override async Task ConnectCoreAsync(CancellationToken cancellationToken)
    {
        Check(await _api.ConnectAsync(_options.Port, cancellationToken).ConfigureAwait(false));
    }

    protected override async Task DisconnectCoreAsync(CancellationToken cancellationToken)
    {
        Check(await _api.DisconnectAsync(cancellationToken).ConfigureAwait(false));
    }

    protected override void OnCommandFailed(string commandName, Exception exception)
    {
        int errorCode = exception is InstrumentCommandException commandException ? commandException.ErrorCode : 0;
        ErrorReceived?.Invoke(this, new ErrorReceivedEventArgs($"The {commandName} command failed: {exception.Message}", errorCode));
    }

    private static SoloOffsets ToOffsets(LiquidHandlingOperationBase operation)
    {
        return new SoloOffsets(operation.XOffset, operation.YOffset, operation.ZOffset);
    }

    private static void EnsureVolumeFitsTip(double volume)
    {
        if (volume <= 0 || volume > MaximumTipVolume)
        {
            throw new ArgumentOutOfRangeException(nameof(volume), volume, $"The volume must be between 0 and {MaximumTipVolume} uL.");
        }
    }

    private SoloCommandResult Check(SoloCommandResult result)
    {
        if (!result.Succeeded)
        {
            throw new InstrumentCommandException(InstrumentName, result.Message, result.ErrorCode);
        }

        return result;
    }

    private void OnMessage(object? sender, SoloMessageEventArgs e)
    {
        MessageReceived?.Invoke(this, new MessageReceivedEventArgs(e.Message));
    }

    private void OnDeviceError(object? sender, SoloDeviceErrorEventArgs e)
    {
        Logger.LogError("{Instrument}: command {Command} reported error {Code}: {Message}", InstrumentName, e.Command, e.ErrorCode, e.Message);
        ErrorReceived?.Invoke(this, new ErrorReceivedEventArgs(e.Message, e.ErrorCode));
    }
}
