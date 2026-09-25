namespace QCRunner.Calculations.Common.Wells;

/// <summary>
/// Absorbance values keyed by wavelength in nanometres.
/// </summary>
public sealed class AbsorbanceSpectrum
{
    private readonly SortedDictionary<double, double> _points = new();

    public IReadOnlyDictionary<double, double> Points => _points;

    public int Count => _points.Count;

    public double MinimumWavelength => _points.Count == 0 ? double.NaN : _points.Keys.First();

    public double MaximumWavelength => _points.Count == 0 ? double.NaN : _points.Keys.Last();

    public void Add(double wavelength, double absorbance)
    {
        _points[wavelength] = absorbance;
    }

    public void Clear()
    {
        _points.Clear();
    }

    /// <summary>
    /// Returns the absorbance recorded closest to the requested wavelength.
    /// </summary>
    public double AbsorbanceAt(double wavelength)
    {
        if (_points.Count == 0)
        {
            return double.NaN;
        }

        if (_points.TryGetValue(wavelength, out double exact))
        {
            return exact;
        }

        return _points.MinBy(point => Math.Abs(point.Key - wavelength)).Value;
    }

    public void CopyFrom(AbsorbanceSpectrum source)
    {
        _points.Clear();

        foreach (KeyValuePair<double, double> point in source._points)
        {
            _points.Add(point.Key, point.Value);
        }
    }
}
