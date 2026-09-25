using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Core.ReaderData;

public sealed class LuminescenceData : ReaderDataBase
{
    public LuminescenceData(WellCode well, int cycle, DateTime measuredAt, TimeSpan elapsedTime, double counts)
        : base(well, cycle, measuredAt, elapsedTime)
    {
        Counts = counts;
    }

    public override ReaderDataType DataType => ReaderDataType.Luminescence;

    public double Counts { get; }
}
