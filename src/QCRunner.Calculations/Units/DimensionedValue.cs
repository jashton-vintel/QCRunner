using System.Globalization;

namespace QCRunner.Calculations.Units;

public sealed class DimensionedValue : IDimensionedValue
{
    public DimensionedValue(double value, Unit unit)
    {
        ArgumentNullException.ThrowIfNull(unit);

        Value = value;
        Unit = unit;
    }

    public double Value { get; }

    public Unit Unit { get; }

    public double Evaluate() => Value;

    public double Evaluate(Unit targetUnit)
    {
        ArgumentNullException.ThrowIfNull(targetUnit);

        if (!Unit.IsCompatibleWith(targetUnit))
        {
            throw new InvalidOperationException($"A value in {Unit.Name} cannot be expressed in {targetUnit.Name}.");
        }

        return Value * Unit.ConversionFactor / targetUnit.ConversionFactor;
    }

    public DimensionedValue ConvertTo(Unit targetUnit) => new(Evaluate(targetUnit), targetUnit);

    public string ToString(string format) => ToString(format, includeUnitSymbol: true);

    public string ToString(string format, bool includeUnitSymbol)
    {
        string text = Value.ToString(format, CultureInfo.InvariantCulture);

        return includeUnitSymbol && Unit.Symbol.Length > 0 ? $"{text} {Unit.Symbol}" : text;
    }

    public override string ToString() => ToString("0.###");
}
