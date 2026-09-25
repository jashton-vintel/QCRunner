namespace QCRunner.Calculations.Units;

public sealed class TurbidityUnit : Unit
{
    public static readonly TurbidityUnit NephelometricTurbidityUnits = new("nephelometric turbidity units", "NTU", 1);

    private TurbidityUnit(string name, string symbol, double conversionFactor)
        : base(name, symbol, conversionFactor)
    {
    }
}
