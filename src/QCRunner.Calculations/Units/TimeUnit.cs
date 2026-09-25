namespace QCRunner.Calculations.Units;

public sealed class TimeUnit : Unit
{
    public static readonly TimeUnit Seconds = new("seconds", "s", 1);

    public static readonly TimeUnit Minutes = new("minutes", "min", 60);

    public static readonly TimeUnit Hours = new("hours", "h", 3600);

    private TimeUnit(string name, string symbol, double conversionFactor)
        : base(name, symbol, conversionFactor)
    {
    }
}
