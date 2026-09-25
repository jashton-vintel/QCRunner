namespace QCRunner.Core.Operations.Pause;

public class PauseOperation : OperationBase
{
    public override OperationType Type => OperationType.Pause;

    public int DurationSeconds { get; set; }

    public TimeSpan Duration => TimeSpan.FromSeconds(DurationSeconds);
}
