namespace QCRunner.Calculations.Units;

public sealed class MassConcentrationUnit : Unit
{
    public static readonly MassConcentrationUnit MicrogramsPerMillilitre = new("micrograms per millilitre", "ug/mL", 1);

    public static readonly MassConcentrationUnit MilligramsPerMillilitre = new("milligrams per millilitre", "mg/mL", 1000);

    private MassConcentrationUnit(string name, string symbol, double conversionFactor)
        : base(name, symbol, conversionFactor)
    {
    }
}
