using Microsoft.Extensions.Logging;
using QCRunner.Core.Instruments.Robots;
using QCRunner.Core.Operations.Robot;
using QCRunner.Infrastructure.Instruments;

namespace QCRunner.Infrastructure.Robots;

/// <summary>
/// A pipetting robot for exhibition shows and training. It needs no hardware, completes every
/// command successfully after a short delay and narrates what it would have done through
/// <see cref="MessageReceived"/> so the demonstration has something to show.
/// </summary>
public sealed class SimulationRobot : InstrumentBase, IRobot
{
    private static readonly TimeSpan CommandTime = TimeSpan.FromMilliseconds(300);

    public SimulationRobot(ILogger<SimulationRobot> logger)
        : base("simulated pipetting robot", logger)
    {
        IsConnected = true;
    }

    public event EventHandler<MessageReceivedEventArgs>? MessageReceived;

    public event EventHandler<ErrorReceivedEventArgs>? ErrorReceived;

    public override string SerialNumber => "SIM-ROBOT-001";

    public Task JogAsync(JogDirection direction, double distance, CancellationToken cancellationToken = default)
    {
        return RunAsync($"jog {direction} by {distance:0.##} mm", cancellationToken);
    }

    public Task AspirateAsync(AspirateOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return RunAsync($"aspirate {operation.Volume:0.#} uL from {operation.Position}:{operation.Point}", cancellationToken);
    }

    public Task DispenseAsync(DispenseOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return RunAsync($"dispense {operation.Volume:0.#} uL to {operation.Position}:{operation.Point}", cancellationToken);
    }

    public Task MixAsync(int position, double volume, int cycles, CancellationToken cancellationToken = default)
    {
        return RunAsync($"mix {cycles} x {volume:0.#} uL at position {position}", cancellationToken);
    }

    public Task SetPlateTemperatureAsync(HeatedPlateOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        return RunAsync($"set nest {operation.Position} to {operation.Temperature:0.#} C", cancellationToken);
    }

    public async Task<string> SendTextAsync(string command, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(command);

        await RunAsync($"send '{command}'", cancellationToken).ConfigureAwait(false);

        return $"OK {command}";
    }

    public Task HomeAsync(CancellationToken cancellationToken = default)
    {
        return RunAsync("home", cancellationToken);
    }

    protected override Task ConnectCoreAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    protected override Task DisconnectCoreAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    protected override void OnCommandFailed(string commandName, Exception exception)
    {
        ErrorReceived?.Invoke(this, new ErrorReceivedEventArgs($"The {commandName} command failed: {exception.Message}", 0));
    }

    private Task RunAsync(string description, CancellationToken cancellationToken)
    {
        return ExecuteCommandAsync(description, async token =>
        {
            await Task.Delay(CommandTime, token).ConfigureAwait(false);
            MessageReceived?.Invoke(this, new MessageReceivedEventArgs($"{description} complete"));
        }, cancellationToken);
    }
}
