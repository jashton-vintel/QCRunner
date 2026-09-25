using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using QCRunner.Calculations.Common.Results;
using QCRunner.Core.Calculations;
using QCRunner.Core.Instruments.Readers;
using QCRunner.Core.Instruments.Robots;
using QCRunner.Core.Operations;
using QCRunner.Core.Operations.Calculation;
using QCRunner.Core.Operations.Pause;
using QCRunner.Core.Operations.Prompt;
using QCRunner.Core.Operations.Reader;
using QCRunner.Core.Operations.Robot;
using QCRunner.Core.Prompts;
using QCRunner.Core.ReaderData;

namespace QCRunner.Core.Runners;

public sealed class OperationRunner : IOperationRunner
{
    private static readonly TimeSpan PauseCheckInterval = TimeSpan.FromMilliseconds(250);

    private readonly IReader _reader;
    private readonly IRobot _robot;
    private readonly ICalculationProcessor _calculationProcessor;
    private readonly IPromptService _promptService;
    private readonly ILogger<OperationRunner> _logger;
    private readonly ConcurrentQueue<IOperation> _queue = new();
    private volatile bool _isPaused;

    public OperationRunner(
        IReader reader,
        IRobot robot,
        ICalculationProcessor calculationProcessor,
        IPromptService promptService,
        IReaderDataStore readerData,
        ILogger<OperationRunner> logger)
    {
        ArgumentNullException.ThrowIfNull(reader);
        ArgumentNullException.ThrowIfNull(robot);
        ArgumentNullException.ThrowIfNull(calculationProcessor);
        ArgumentNullException.ThrowIfNull(promptService);
        ArgumentNullException.ThrowIfNull(readerData);
        ArgumentNullException.ThrowIfNull(logger);

        _reader = reader;
        _robot = robot;
        _calculationProcessor = calculationProcessor;
        _promptService = promptService;
        ReaderData = readerData;
        _logger = logger;

        _reader.ReaderDataReceived += OnReaderDataReceived;
        _reader.ReaderError += OnReaderError;
        _robot.MessageReceived += OnRobotMessageReceived;
        _robot.ErrorReceived += OnRobotErrorReceived;
    }

    public event EventHandler<OperationStartedEventArgs>? OperationStarted;

    public event EventHandler<OperationCompletedEventArgs>? OperationCompleted;

    public IOperation? CurrentOperation { get; private set; }

    public bool IsPaused => _isPaused;

    public int QueuedOperationCount => _queue.Count;

    public IReaderDataStore ReaderData { get; }

    public void Enqueue(IOperation operation)
    {
        ArgumentNullException.ThrowIfNull(operation);
        _queue.Enqueue(operation);
    }

    public void EnqueueRange(IEnumerable<IOperation> operations)
    {
        ArgumentNullException.ThrowIfNull(operations);

        foreach (IOperation operation in operations)
        {
            Enqueue(operation);
        }
    }

    public async Task<IReadOnlyList<IOperationResult>> RunQueuedAsync(CancellationToken cancellationToken = default)
    {
        var results = new List<IOperationResult>();

        while (_queue.TryDequeue(out IOperation? operation))
        {
            while (_isPaused)
            {
                await Task.Delay(PauseCheckInterval, cancellationToken).ConfigureAwait(false);
            }

            IOperationResult result = await RunAsync(operation, cancellationToken).ConfigureAwait(false);
            results.Add(result);

            if (result.Status != OperationResultStatus.Successful)
            {
                _logger.LogWarning("Discarding {Remaining} queued operations because '{Operation}' finished with status {Status}",
                    _queue.Count, operation.Name, result.Status);
                _queue.Clear();
                break;
            }
        }

        return results;
    }

    public async Task<IOperationResult> RunAsync(IOperation operation, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(operation);

        CurrentOperation = operation;
        _logger.LogInformation("Starting {Type} operation '{Operation}'", operation.Type, operation.Name);
        OperationStarted?.Invoke(this, new OperationStartedEventArgs(operation));

        Stopwatch stopwatch = Stopwatch.StartNew();
        IOperationResult result;

        try
        {
            result = await DispatchAsync(operation, cancellationToken).ConfigureAwait(false);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogWarning("Operation '{Operation}' was cancelled", operation.Name);
            result = OperationResult.Cancelled(operation);
        }
        catch (InsufficientWellDataException exception)
        {
            _logger.LogError("Operation '{Operation}' could not run: {Reason}", operation.Name, exception.Message);
            result = OperationResult.Failed(operation, exception.Message, exception);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Operation '{Operation}' failed", operation.Name);
            result = OperationResult.Failed(operation, exception.Message, exception);
        }

        _logger.LogInformation("Finished '{Operation}' with status {Status} after {Elapsed:N1} s",
            operation.Name, result.Status, stopwatch.Elapsed.TotalSeconds);

        OperationCompleted?.Invoke(this, new OperationCompletedEventArgs(operation, result));
        CurrentOperation = null;

        return result;
    }

    public void Pause()
    {
        _logger.LogInformation("Operation running paused; the current operation will finish first");
        _isPaused = true;
    }

    public void Resume()
    {
        _logger.LogInformation("Operation running resumed");
        _isPaused = false;
    }

    public void Dispose()
    {
        _reader.ReaderDataReceived -= OnReaderDataReceived;
        _reader.ReaderError -= OnReaderError;
        _robot.MessageReceived -= OnRobotMessageReceived;
        _robot.ErrorReceived -= OnRobotErrorReceived;
    }

    private Task<IOperationResult> DispatchAsync(IOperation operation, CancellationToken cancellationToken)
    {
        return operation switch
        {
            AbsorbanceOperation absorbance => RunMeasurementAsync(absorbance, () => _reader.PerformAbsorbanceAsync(absorbance, cancellationToken)),
            LuminescenceOperation luminescence => RunMeasurementAsync(luminescence, () => _reader.PerformLuminescenceAsync(luminescence, cancellationToken)),
            ShakeOperation shake => RunInstrumentCommandAsync(shake, () => _reader.PerformShakeAsync(shake, cancellationToken)),
            HeatToTemperatureOperation heat => RunInstrumentCommandAsync(heat, () => _reader.PerformHeatToTemperatureAsync(heat, cancellationToken)),
            AspirateOperation aspirate => RunInstrumentCommandAsync(aspirate, () => _robot.AspirateAsync(aspirate, cancellationToken)),
            DispenseOperation dispense => RunInstrumentCommandAsync(dispense, () => _robot.DispenseAsync(dispense, cancellationToken)),
            HeatedPlateOperation heatedPlate => RunInstrumentCommandAsync(heatedPlate, () => _robot.SetPlateTemperatureAsync(heatedPlate, cancellationToken)),
            CalculationOperation calculation => RunCalculationAsync(calculation, cancellationToken),
            PromptOperation prompt => RunPromptAsync(prompt, cancellationToken),
            PauseOperation pause => RunPauseAsync(pause, cancellationToken),
            _ => throw new NotSupportedException($"Operations of type {operation.GetType().Name} cannot be run.")
        };
    }

    private static async Task<IOperationResult> RunInstrumentCommandAsync(IOperation operation, Func<Task> command)
    {
        await command().ConfigureAwait(false);
        return OperationResult.Successful(operation);
    }

    private async Task<IOperationResult> RunMeasurementAsync(MeasurementOperationBase operation, Func<Task> measure)
    {
        int readingsBefore = ReaderData.Count;

        await measure().ConfigureAwait(false);

        IReadOnlyList<IReaderData> readings = ReaderData.Snapshot().Skip(readingsBefore).ToList();
        _logger.LogInformation("'{Operation}' produced {Count} readings", operation.Name, readings.Count);

        return new MeasurementOperationResult(operation, readings);
    }

    private async Task<IOperationResult> RunCalculationAsync(CalculationOperation operation, CancellationToken cancellationToken)
    {
        IReadOnlyList<WellReaderData> wellData = ReaderData.SelectWellData(operation);

        ICalculationResult calculationResult = await Task
            .Run(() => _calculationProcessor.Calculate(operation, wellData), cancellationToken)
            .ConfigureAwait(false);

        return new CalculationOperationResult(operation, calculationResult);
    }

    private async Task<IOperationResult> RunPromptAsync(PromptOperation operation, CancellationToken cancellationToken)
    {
        var request = new PromptRequest(operation.MessageTitle, operation.MessageText, operation.Buttons);
        PromptResponse response = await _promptService.ShowPromptAsync(request, cancellationToken).ConfigureAwait(false);

        return new PromptOperationResult(operation, response);
    }

    private static async Task<IOperationResult> RunPauseAsync(PauseOperation operation, CancellationToken cancellationToken)
    {
        await Task.Delay(operation.Duration, cancellationToken).ConfigureAwait(false);
        return OperationResult.Successful(operation);
    }

    private void OnReaderDataReceived(object? sender, ReaderDataReceivedEventArgs e)
    {
        ReaderData.Add(e.Data);
        _logger.LogDebug("Reader data received: {Data}", e.Data);
    }

    private void OnReaderError(object? sender, ReaderErrorEventArgs e)
    {
        _logger.LogError(e.Exception, "Reader error: {Message}", e.Message);
    }

    private void OnRobotMessageReceived(object? sender, MessageReceivedEventArgs e)
    {
        _logger.LogDebug("Robot: {Message}", e.Message);
    }

    private void OnRobotErrorReceived(object? sender, ErrorReceivedEventArgs e)
    {
        _logger.LogError("Robot error {Code}: {Message}", e.ErrorCode, e.Message);
    }
}
