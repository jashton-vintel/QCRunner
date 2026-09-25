using System.Collections;

namespace QCRunner.Calculations.Common.Parameters;

public sealed class TestParameters : ITestParameters, IEnumerable<KeyValuePair<string, double>>
{
    private readonly Dictionary<string, double> _values;

    public TestParameters()
    {
        _values = new Dictionary<string, double>(StringComparer.OrdinalIgnoreCase);
    }

    public TestParameters(IEnumerable<KeyValuePair<string, double>> parameters)
        : this()
    {
        Add(parameters);
    }

    public IReadOnlyDictionary<string, double> Values => _values;

    public bool Contains(string name) => _values.ContainsKey(name);

    public double Get(string name)
    {
        if (!_values.TryGetValue(name, out double value))
        {
            throw new KeyNotFoundException($"The calculation parameter '{name}' has not been supplied.");
        }

        return value;
    }

    public double GetOrDefault(string name, double fallback)
    {
        return _values.TryGetValue(name, out double value) ? value : fallback;
    }

    public void Add(string name, double value)
    {
        _values[name] = value;
    }

    public ITestParameters Add(IEnumerable<KeyValuePair<string, double>> parameters)
    {
        foreach (KeyValuePair<string, double> parameter in parameters)
        {
            _values[parameter.Key] = parameter.Value;
        }

        return this;
    }

    public IEnumerator<KeyValuePair<string, double>> GetEnumerator() => _values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
