using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Core.ReaderData;

public abstract class ReaderDataBase : IReaderData
{
    protected ReaderDataBase(WellCode well, int cycle, DateTime measuredAt, TimeSpan elapsedTime)
    {
        Well = well;
        Cycle = cycle;
        MeasuredAt = measuredAt;
        ElapsedTime = elapsedTime;
    }

    public WellCode Well { get; }

    public abstract ReaderDataType DataType { get; }

    public int Cycle { get; }

    public DateTime MeasuredAt { get; }

    public TimeSpan ElapsedTime { get; }

    public override string ToString() => $"{DataType} {Well} cycle {Cycle}";
}
