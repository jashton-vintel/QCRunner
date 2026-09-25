using Microsoft.Extensions.Logging;

namespace QCRunner.Infrastructure.Vendors.Hudson;

/// <summary>
/// Stands in for the pipetting robot vendor SDK, which is not distributed with this solution.
/// Commands succeed after a short delay and echo a message the way the real device does.
/// </summary>
public sealed class SoloApiStub : ISoloApi
{
    private static readonly HashSet<string> Axes = new(StringComparer.OrdinalIgnoreCase) { "X", "Y", "Z" };
    private static readonly TimeSpan CommandTime = TimeSpan.FromMilliseconds(300);

    private readonly ILogger<SoloApiStub> _logger;
    private CancellationTokenSource _halt = new();

    public SoloApiStub(ILogger<SoloApiStub> logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        _logger = logger;
    }

    public event EventHandler<SoloMessageEventArgs>? Message;

    public event EventHandler<SoloDeviceErrorEventArgs>? DeviceError;

    public bool IsConnected { get; private set; }

    public string SerialNumber => "SOLO-000456";

    public string FirmwareVersion => "2.0.3-stub";

    public async Task<SoloCommandResult> ConnectAsync(string port, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Vendor SDK: opening {Port}", port);
        await Task.Delay(CommandTime, cancellationToken).ConfigureAwait(false);
        IsConnected = true;

        return SoloCommandResult.Ok();
    }

    public Task<SoloCommandResult> DisconnectAsync(CancellationToken cancellationToken)
    {
        IsConnected = false;
        return Task.FromResult(SoloCommandResult.Ok());
    }

    public Task<SoloCommandResult> HomeAsync(CancellationToken cancellationToken)
    {
        return RunCommandAsync("HOME", cancellationToken);
    }

    public Task<SoloCommandResult> MoveToTravelHeightAsync(CancellationToken cancellationToken)
    {
        return RunCommandAsync("MOVE Z TRAVEL", cancellationToken);
    }

    public Task<SoloCommandResult> AspirateAsync(int position, string point, double volume, double height, SoloOffsets offsets, CancellationToken cancellationToken)
    {
        return RunCommandAsync($"ASPIRATE {volume:0.#} FROM {position}:{point} AT {height:0.#}", cancellationToken);
    }

    public Task<SoloCommandResult> DispenseAsync(int position, string point, double volume, double height, SoloOffsets offsets, CancellationToken cancellationToken)
    {
        return RunCommandAsync($"DISPENSE {volume:0.#} TO {position}:{point} AT {height:0.#}", cancellationToken);
    }

    public Task<SoloCommandResult> MixAsync(int position, double volume, int cycles, CancellationToken cancellationToken)
    {
        return RunCommandAsync($"MIX {cycles} x {volume:0.#} AT {position}", cancellationToken);
    }

    public Task<SoloCommandResult> JogAxisAsync(string axis, double distance, CancellationToken cancellationToken)
    {
        if (!Axes.Contains(axis))
        {
            return Task.FromResult(Fail("JOG", 12, $"Unknown axis '{axis}'"));
        }

        return RunCommandAsync($"JOG {axis.ToUpperInvariant()} {distance:0.##}", cancellationToken);
    }

    public async Task<SoloCommandResult> SendTextAsync(string command, CancellationToken cancellationToken)
    {
        SoloCommandResult result = await RunCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return result with { Response = $"OK {command}" };
    }

    public Task<SoloCommandResult> SetTemperatureAsync(int position, double temperature, CancellationToken cancellationToken)
    {
        return RunCommandAsync($"SETTCTEMPERATURE {position}, {temperature:0.#}", cancellationToken);
    }

    public void Halt()
    {
        CancellationTokenSource halted = Interlocked.Exchange(ref _halt, new CancellationTokenSource());
        halted.Cancel();
        halted.Dispose();
    }

    private async Task<SoloCommandResult> RunCommandAsync(string command, CancellationToken cancellationToken)
    {
        if (!IsConnected)
        {
            return Fail(command, -99, "The port is closed");
        }

        using CancellationTokenSource linked = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, _halt.Token);

        try
        {
            await Task.Delay(CommandTime, linked.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return Fail(command, -1, "Halted");
        }

        Message?.Invoke(this, new SoloMessageEventArgs($"{command} complete"));

        return SoloCommandResult.Ok();
    }

    private SoloCommandResult Fail(string command, int errorCode, string message)
    {
        DeviceError?.Invoke(this, new SoloDeviceErrorEventArgs(command, errorCode, message));
        return SoloCommandResult.Failed(errorCode, message);
    }
}
