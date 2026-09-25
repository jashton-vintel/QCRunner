namespace QCRunner.Calculations.Units;

public sealed class PercentageUnit : Unit
{
    public static readonly PercentageUnit Percent = new("percent", "%", 1);

    private PercentageUnit(string name, string symbol, double conversionFactor)
        : base(name, symbol, conversionFactor)
    {
    }
}
