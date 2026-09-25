namespace QCRunner.Calculations.Units;

public sealed class AbsorbanceUnit : Unit
{
    public static readonly AbsorbanceUnit AbsorbanceUnits = new("absorbance units", "AU", 1);

    public static readonly AbsorbanceUnit MilliAbsorbanceUnits = new("milli absorbance units", "mAU", 0.001);

    private AbsorbanceUnit(string name, string symbol, double conversionFactor)
        : base(name, symbol, conversionFactor)
    {
    }
}
