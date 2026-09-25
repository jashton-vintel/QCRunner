using Microsoft.Extensions.Logging;
using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;
using QCRunner.Core.Batches;
using QCRunner.Core.Methods;
using QCRunner.Core.Operations.Prompt;
using QCRunner.Core.Prompts;

namespace QCRunner.Core.Runners;

public class BatchRunner : IBatchRunner
{
    private readonly IOperationRunner _operationRunner;
    private readonly IPromptService _promptService;
    private readonly ILogger _logger;
    private readonly List<ICalculationResult> _calculationResults = new();
    private readonly List<IOperationResult> _operationResults = new();
    private CancellationTokenSource? _cancellation;

    public BatchRunner(Batch batch, IOperationRunner operationRunner, IPromptService promptService, ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(batch);
        ArgumentNullException.ThrowIfNull(operationRunner);
        ArgumentNullException.ThrowIfNull(promptService);
        ArgumentNullException.ThrowIfNull(logger);

        Batch = batch;
        _operationRunner = operationRunner;
        _promptService = promptService;
        _logger = logger;

        _operationRunner.OperationStarted += OnOperationStarted;
        _operationRunner.OperationCompleted += OnOperationCompleted;
    }

    public event EventHandler? BatchStarted;

    public event EventHandler<PhaseEventArgs>? PhaseStarted;

    public event EventHandler<OperationStartedEventArgs>? OperationStarted;

    public event EventHandler<OperationCompletedEventArgs>? OperationCompleted;

    public event EventHandler<PhaseEventArgs>? PhaseCompleted;

    public event EventHandler<BatchCompletedEventArgs>? BatchCompleted;

    public Batch Batch { get; }

    public BatchRunState State { get; private set; } = BatchRunState.NotStarted;

    public IReadOnlyList<ICalculationResult> CalculationResults => _calculationResults;

    public IReadOnlyList<IOperationResult> OperationResults => _operationResults;

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        if (State != BatchRunState.NotStarted)
        {
            throw new InvalidOperationException("A batch runner runs its batch once; create a new runner to run again.");
        }

        _cancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        CancellationToken token = _cancellation.Token;

        Start();

        try
        {
            List<Phase> phases = Batch.Method.Phases.OrderBy(phase => phase.Index).ToList();

            for (int index = 0; index < phases.Count; index++)
            {
                if (index > 0 && !await ConfirmPhaseStartAsync(phases[index - 1], phases[index], token).ConfigureAwait(false))
                {
                    Complete(BatchRunState.Cancelled, BatchResultType.Cancelled);
                    return;
                }

                OperationResultStatus phaseStatus = await RunPhaseAsync(phases[index], token).ConfigureAwait(false);

                if (phaseStatus == OperationResultStatus.Cancelled)
                {
                    Complete(BatchRunState.Cancelled, BatchResultType.Cancelled);
                    return;
                }

                if (phaseStatus == OperationResultStatus.Unsuccessful)
                {
                    Complete(BatchRunState.Failed, BatchResultType.Failed);
                    return;
                }
            }

            bool allPassed = _calculationResults.All(result => result.Result == CalculationAssayResult.Pass);
            Complete(BatchRunState.Completed, allPassed ? BatchResultType.Passed : BatchResultType.Failed);
        }
        catch (OperationCanceledException)
        {
            Complete(BatchRunState.Cancelled, BatchResultType.Cancelled);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Batch {Reference} stopped because of an unexpected error", Batch.Reference);
            Complete(BatchRunState.Failed, BatchResultType.Failed, exception);
        }
    }

    public void Pause()
    {
        if (State != BatchRunState.Running)
        {
            return;
        }

        State = BatchRunState.Paused;
        _operationRunner.Pause();
        _logger.LogInformation("Batch {Reference} paused", Batch.Reference);
    }

    public void Resume()
    {
        if (State != BatchRunState.Paused)
        {
            return;
        }

        State = BatchRunState.Running;
        _operationRunner.Resume();
        _logger.LogInformation("Batch {Reference} resumed", Batch.Reference);
    }

    public void Cancel()
    {
        _logger.LogWarning("Cancellation requested for batch {Reference}", Batch.Reference);
        _cancellation?.Cancel();
    }

    public virtual void Dispose()
    {
        _operationRunner.OperationStarted -= OnOperationStarted;
        _operationRunner.OperationCompleted -= OnOperationCompleted;
        _operationRunner.Dispose();
        _cancellation?.Dispose();
    }

    private void Start()
    {
        State = BatchRunState.Running;
        Batch.StartTime = DateTime.Now;
        Batch.Result = BatchResultType.InProgress;

        _logger.LogInformation("Batch {Reference} started with method {Method}", Batch.Reference, Batch.Method);
        BatchStarted?.Invoke(this, EventArgs.Empty);
    }

    private async Task<OperationResultStatus> RunPhaseAsync(Phase phase, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Phase {Index} '{Phase}' started", phase.Index, phase.Name);
        PhaseStarted?.Invoke(this, new PhaseEventArgs(phase));

        foreach (OperationGroup group in phase.OperationGroups.OrderBy(group => group.Index))
        {
            _logger.LogInformation("Queueing group {Index} '{Group}' ({Count} operations)", group.Index, group.Name, group.Operations.Count);
            _operationRunner.EnqueueRange(group.Operations.OrderBy(operation => operation.Index));
        }

        IReadOnlyList<IOperationResult> results = await _operationRunner.RunQueuedAsync(cancellationToken).ConfigureAwait(false);
        _operationResults.AddRange(results);

        OperationResultStatus status = OperationResultStatus.Successful;

        if (results.Any(result => result.Status == OperationResultStatus.Cancelled))
        {
            status = OperationResultStatus.Cancelled;
        }
        else if (results.Any(result => result.Status == OperationResultStatus.Unsuccessful))
        {
            status = OperationResultStatus.Unsuccessful;
        }

        _logger.LogInformation("Phase '{Phase}' finished with status {Status}", phase.Name, status);
        PhaseCompleted?.Invoke(this, new PhaseEventArgs(phase, status == OperationResultStatus.Successful));

        return status;
    }

    /// <summary>
    /// The pause between phases. The operator confirms that the plates and samples for the next
    /// phase are in place, or cancels the batch.
    /// </summary>
    private async Task<bool> ConfirmPhaseStartAsync(Phase previous, Phase next, CancellationToken cancellationToken)
    {
        var request = new PromptRequest("Phase complete", $"Phase '{previous.Name}' has finished. Start phase '{next.Name}'?", PromptButtons.YesCancel);
        PromptResponse response = await _promptService.ShowPromptAsync(request, cancellationToken).ConfigureAwait(false);

        return response is PromptResponse.Yes or PromptResponse.Ok;
    }

    private void Complete(BatchRunState state, BatchResultType result, Exception? exception = null)
    {
        State = state;
        Batch.EndTime = DateTime.Now;
        Batch.Result = result;

        _logger.LogInformation("Batch {Reference} {State} with result {Result}", Batch.Reference, state, result);
        BatchCompleted?.Invoke(this, new BatchCompletedEventArgs(Batch, state, CalculationResults, exception));
    }

    private void OnOperationStarted(object? sender, OperationStartedEventArgs e)
    {
        OperationStarted?.Invoke(this, e);
    }

    private void OnOperationCompleted(object? sender, OperationCompletedEventArgs e)
    {
        if (e.Result is CalculationOperationResult calculation)
        {
            _calculationResults.Add(calculation.CalculationResult);
        }

        OperationCompleted?.Invoke(this, e);
    }
}
