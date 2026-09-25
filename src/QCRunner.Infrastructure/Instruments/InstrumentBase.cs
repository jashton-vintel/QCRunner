using System.Diagnostics;
using Microsoft.Extensions.Logging;
using QCRunner.Core.Instruments;

namespace QCRunner.Infrastructure.Instruments;

/// <summary>
/// Common behaviour for every instrument: one command at a time, a connection check before
/// each command, and consistent logging around it. Derived classes express each hardware
/// action as a call to <see cref="ExecuteCommandAsync(string, Func{CancellationToken, Task}, CancellationToken)"/>.
/// </summary>
public abstract class InstrumentBase : IInstrument, IDisposable
{
    private readonly SemaphoreSlim _commandLock = new(1, 1);

    protected InstrumentBase(string instrumentName, ILogger logger)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(instrumentName);
        ArgumentNullException.ThrowIfNull(logger);

        InstrumentName = instrumentName;
        Logger = logger;
    }

    public string InstrumentName { get; }

    public abstract string SerialNumber { get; }

    public bool IsConnected { get; protected set; }

    protected ILogger Logger { get; }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await _commandLock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (IsConnected)
            {
                return;
            }

            Logger.LogInformation("Connecting to the {Instrument}", InstrumentName);
            await ConnectCoreAsync(cancellationToken).ConfigureAwait(false);
            IsConnected = true;
            Logger.LogInformation("Connected to the {Instrument}, serial number {SerialNumber}", InstrumentName, SerialNumber);
        }
        finally
        {
            _commandLock.Release();
        }
    }

    public async Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        await _commandLock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (!IsConnected)
            {
                return;
            }

            Logger.LogInformation("Disconnecting from the {Instrument}", InstrumentName);
            await DisconnectCoreAsync(cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            IsConnected = false;
            _commandLock.Release();
        }
    }

    public virtual void Dispose()
    {
        _commandLock.Dispose();
    }

    protected abstract Task ConnectCoreAsync(CancellationToken cancellationToken);

    protected abstract Task DisconnectCoreAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Runs one instrument command exclusively: waits for any command in flight, checks the
    /// connection, logs the start and the finish, and reports failures through
    /// <see cref="OnCommandFailed"/> before rethrowing them to the caller.
    /// </summary>
    protected async Task ExecuteCommandAsync(string commandName, Func<CancellationToken, Task> command, CancellationToken cancellationToken)
    {
        await ExecuteCommandAsync(commandName, async token =>
        {
            await command(token).ConfigureAwait(false);
            return true;
        }, cancellationToken).ConfigureAwait(false);
    }

    protected async Task<TResult> ExecuteCommandAsync<TResult>(string commandName, Func<CancellationToken, Task<TResult>> command, CancellationToken cancellationToken)
    {
        await _commandLock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            EnsureConnected();

            Logger.LogInformation("{Instrument}: {Command} started", InstrumentName, commandName);
            Stopwatch stopwatch = Stopwatch.StartNew();

            try
            {
                TResult result = await command(cancellationToken).ConfigureAwait(false);
                Logger.LogInformation("{Instrument}: {Command} finished in {Elapsed:N1} s", InstrumentName, commandName, stopwatch.Elapsed.TotalSeconds);

                return result;
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                Logger.LogWarning("{Instrument}: {Command} cancelled after {Elapsed:N1} s", InstrumentName, commandName, stopwatch.Elapsed.TotalSeconds);
                throw;
            }
            catch (Exception exception)
            {
                Logger.LogError(exception, "{Instrument}: {Command} failed", InstrumentName, commandName);
                OnCommandFailed(commandName, exception);
                throw;
            }
        }
        finally
        {
            _commandLock.Release();
        }
    }

    protected virtual void OnCommandFailed(string commandName, Exception exception)
    {
    }

    protected void EnsureConnected()
    {
        if (!IsConnected)
        {
            throw new InstrumentNotConnectedException(InstrumentName);
        }
    }
}
