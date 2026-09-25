using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Wells;

/// <summary>
/// A well read by luminescence: photon counts recorded over time.
/// </summary>
public sealed class LuminescenceWell : WellBase
{
    public LuminescenceWell(string name, WellRole role, WellCode code)
        : this(name, role, code, double.NaN)
    {
    }

    public LuminescenceWell(string name, WellRole role, WellCode code, double trueValue)
        : base(name, role, code, trueValue)
    {
        Counts = new LuminescenceSeries();
    }

    public LuminescenceSeries Counts { get; init; }

    public override ReaderDataType MeasurementType => ReaderDataType.Luminescence;
}
