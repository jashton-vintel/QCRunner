namespace QCRunner.Calculations.Units;

public sealed class DimensionlessUnit : Unit
{
    public static readonly DimensionlessUnit None = new("dimensionless", string.Empty, 1);

    public static readonly DimensionlessUnit RetentionFactor = new("retention factor", "Rf", 1);

    private DimensionlessUnit(string name, string symbol, double conversionFactor)
        : base(name, symbol, conversionFactor)
    {
    }
}
