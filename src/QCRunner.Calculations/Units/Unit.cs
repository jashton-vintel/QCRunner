namespace QCRunner.Calculations.Units;

/// <summary>
/// A unit of measure. Units of the same dimension share a CLR type and convert through
/// <see cref="ConversionFactor"/>, which is the multiplier to the base unit of that dimension.
/// </summary>
public abstract class Unit
{
    protected Unit(string name, string symbol, double conversionFactor)
    {
        Name = name;
        Symbol = symbol;
        ConversionFactor = conversionFactor;
    }

    public string Name { get; }

    public string Symbol { get; }

    public double ConversionFactor { get; }

    public bool IsCompatibleWith(Unit other) => other.GetType() == GetType();

    public override string ToString() => Symbol;
}
