namespace QCRunner.Core.Runners;

public enum BatchRunState
{
    NotStarted = 0,
    Running = 1,
    Paused = 2,
    Completed = 3,
    Cancelled = 4,
    Failed = 5
}
