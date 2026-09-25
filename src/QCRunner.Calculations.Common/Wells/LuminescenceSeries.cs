namespace QCRunner.Calculations.Common.Wells;

/// <summary>
/// Luminescence counts keyed by elapsed time in seconds from the first reading.
/// </summary>
public sealed class LuminescenceSeries
{
    private readonly SortedDictionary<double, double> _points = new();

    public IReadOnlyDictionary<double, double> Points => _points;

    public int Count => _points.Count;

    public double FirstTime => _points.Count == 0 ? double.NaN : _points.Keys.First();

    public double LastTime => _points.Count == 0 ? double.NaN : _points.Keys.Last();

    public void Add(double elapsedSeconds, double counts)
    {
        _points[elapsedSeconds] = counts;
    }

    public void Clear()
    {
        _points.Clear();
    }

    public double CountsAt(double elapsedSeconds)
    {
        if (_points.Count == 0)
        {
            return double.NaN;
        }

        if (_points.TryGetValue(elapsedSeconds, out double exact))
        {
            return exact;
        }

        return _points.MinBy(point => Math.Abs(point.Key - elapsedSeconds)).Value;
    }
}
