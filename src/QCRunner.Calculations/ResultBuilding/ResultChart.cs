using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.ResultBuilding;

public sealed class ResultChart : IChart
{
    private readonly List<Series> _series = new();

    public string Title { get; set; } = string.Empty;

    public IReadOnlyList<Series> Series => _series;

    public Axis AxisX { get; set; } = new();

    public Axis AxisY { get; set; } = new();

    public DashStyleType BorderDashStyle { get; set; } = DashStyleType.Solid;

    /// <summary>The smallest X value across every series, or NaN when the chart is empty.</summary>
    public double XRangeMinimum => Range(point => point.X, Math.Min, double.PositiveInfinity);

    public double XRangeMaximum => Range(point => point.X, Math.Max, double.NegativeInfinity);

    public double YRangeMinimum => Range(point => point.Y, Math.Min, double.PositiveInfinity);

    public double YRangeMaximum => Range(point => point.Y, Math.Max, double.NegativeInfinity);

    IReadOnlyList<ISeries> IChart.Series => _series;

    IAxis IChart.AxisX => AxisX;

    IAxis IChart.AxisY => AxisY;

    public void AddSeries(Series series)
    {
        ArgumentNullException.ThrowIfNull(series);
        _series.Add(series);
    }

    private double Range(Func<Coordinate, double> selector, Func<double, double, double> reducer, double seed)
    {
        double result = seed;

        foreach (Coordinate point in _series.SelectMany(series => series.Data).Where(point => !point.IsEmpty))
        {
            result = reducer(result, selector(point));
        }

        return double.IsInfinity(result) ? double.NaN : result;
    }
}
