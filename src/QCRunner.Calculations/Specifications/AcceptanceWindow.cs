using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Specifications;

/// <summary>
/// A lower and an upper specification limit that a measurement must sit between.
/// </summary>
public sealed class AcceptanceWindow
{
    public AcceptanceWindow(LowerSpecificationLimit lower, UpperSpecificationLimit upper)
    {
        ArgumentNullException.ThrowIfNull(lower);
        ArgumentNullException.ThrowIfNull(upper);

        if (!lower.Value.Unit.IsCompatibleWith(upper.Value.Unit))
        {
            throw new ArgumentException("The lower and upper limits must share a dimension.", nameof(upper));
        }

        Lower = lower;
        Upper = upper;
    }

    public LowerSpecificationLimit Lower { get; }

    public UpperSpecificationLimit Upper { get; }

    public string Format => Upper.Format;

    public bool Contains(IDimensionedValue measurement) => Lower.IsSatisfiedBy(measurement) && Upper.IsSatisfiedBy(measurement);

    public override string ToString() => $"{Lower} and {Upper}";
}
