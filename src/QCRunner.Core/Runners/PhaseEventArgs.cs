using QCRunner.Core.Methods;

namespace QCRunner.Core.Runners;

public sealed class PhaseEventArgs : EventArgs
{
    public PhaseEventArgs(Phase phase, bool succeeded = true)
    {
        Phase = phase;
        Succeeded = succeeded;
    }

    public Phase Phase { get; }

    public bool Succeeded { get; }
}
