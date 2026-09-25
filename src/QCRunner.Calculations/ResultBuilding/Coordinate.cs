using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.ResultBuilding;

public sealed class Coordinate : ICoordinate
{
    public Coordinate(double x, double y)
    {
        X = x;
        Y = y;
    }

    private Coordinate()
    {
        IsEmpty = true;
    }

    /// <summary>A gap in a series; viewers break the line here rather than drawing through it.</summary>
    public static Coordinate Empty => new();

    public double X { get; }

    public double Y { get; }

    public bool IsEmpty { get; }
}
