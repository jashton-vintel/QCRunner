using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Specifications;

/// <summary>
/// One side of an acceptance criterion. Measurements are rounded to the same number of
/// decimals the report shows before they are compared, so a value that prints as passing
/// also passes.
/// </summary>
public abstract class SpecificationLimit
{
    protected SpecificationLimit(IDimensionedValue value, bool isClosed, int? decimals)
    {
        ArgumentNullException.ThrowIfNull(value);

        Value = value;
        IsClosed = isClosed;
        Decimals = decimals;
    }

    public IDimensionedValue Value { get; }

    /// <summary>True when a measurement equal to the limit is acceptable.</summary>
    public bool IsClosed { get; }

    public int? Decimals { get; }

    public abstract string Symbol { get; }

    /// <summary>A numeric format string with the same number of decimals as the comparison uses.</summary>
    public string Format => Decimals is int decimals && decimals > 0 ? "0." + new string('0', decimals) : "0";

    public bool IsSatisfiedBy(IDimensionedValue measurement)
    {
        ArgumentNullException.ThrowIfNull(measurement);

        double measured = Round(measurement.Evaluate(Value.Unit));
        double limit = Round(Value.Evaluate());

        return !double.IsNaN(measured) && Compare(measured, limit);
    }

    public override string ToString() => $"{Symbol} {Value.ToString(Format)}";

    protected abstract bool Compare(double measured, double limit);

    private double Round(double value)
    {
        return Decimals is int decimals ? Math.Round(value, decimals, MidpointRounding.AwayFromZero) : value;
    }
}
