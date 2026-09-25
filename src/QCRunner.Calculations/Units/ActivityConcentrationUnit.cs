namespace QCRunner.Calculations.Units;

public sealed class ActivityConcentrationUnit : Unit
{
    public static readonly ActivityConcentrationUnit MegabecquerelsPerMillilitre = new("megabecquerels per millilitre", "MBq/mL", 1);

    public static readonly ActivityConcentrationUnit GigabecquerelsPerMillilitre = new("gigabecquerels per millilitre", "GBq/mL", 1000);

    private ActivityConcentrationUnit(string name, string symbol, double conversionFactor)
        : base(name, symbol, conversionFactor)
    {
    }
}
