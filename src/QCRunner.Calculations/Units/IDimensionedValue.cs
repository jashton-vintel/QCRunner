namespace QCRunner.Calculations.Units;

public interface IDimensionedValue
{
    double Value { get; }

    Unit Unit { get; }

    double Evaluate();

    /// <summary>Returns the value converted into another unit of the same dimension.</summary>
    double Evaluate(Unit targetUnit);

    string ToString(string format);

    string ToString(string format, bool includeUnitSymbol);
}
