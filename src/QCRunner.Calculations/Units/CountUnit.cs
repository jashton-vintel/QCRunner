namespace QCRunner.Calculations.Units;

public sealed class CountUnit : Unit
{
    public static readonly CountUnit Counts = new("counts", "counts", 1);

    public static readonly CountUnit CountsPerSecond = new("counts per second", "cps", 1);

    private CountUnit(string name, string symbol, double conversionFactor)
        : base(name, symbol, conversionFactor)
    {
    }
}
