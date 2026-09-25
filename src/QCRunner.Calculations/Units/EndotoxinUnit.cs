namespace QCRunner.Calculations.Units;

public sealed class EndotoxinUnit : Unit
{
    public static readonly EndotoxinUnit EndotoxinUnitsPerMillilitre = new("endotoxin units per millilitre", "EU/mL", 1);

    private EndotoxinUnit(string name, string symbol, double conversionFactor)
        : base(name, symbol, conversionFactor)
    {
    }
}
