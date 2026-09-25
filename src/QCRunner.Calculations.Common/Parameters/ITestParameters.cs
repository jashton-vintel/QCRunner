namespace QCRunner.Calculations.Common.Parameters;

/// <summary>
/// Named numeric inputs to a calculation: acceptance criteria, kit calibration values and
/// method-specific settings. Every parameter is a string name with a double value.
/// </summary>
public interface ITestParameters
{
    IReadOnlyDictionary<string, double> Values { get; }

    bool Contains(string name);

    double Get(string name);

    double GetOrDefault(string name, double fallback);

    /// <summary>Adds or overwrites parameters and returns the same instance for chaining.</summary>
    ITestParameters Add(IEnumerable<KeyValuePair<string, double>> parameters);
}
