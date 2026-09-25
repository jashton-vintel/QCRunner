using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;

namespace QCRunner.Calculations.Engine;

/// <summary>
/// Template for every test: merge the supplied parameters over the defaults, load the data,
/// validate it, then calculate.
/// </summary>
internal abstract class TestBase : ITest
{
    private ITestParameters? _parameters;

    public abstract string Name { get; }

    public abstract CalculationType CalculationType { get; }

    public virtual ITestParameters DefaultParameters => new TestParameters();

    public abstract IReadOnlyList<IWell> WellsNeeded { get; }

    protected ITestParameters Parameters =>
        _parameters ?? throw new InvalidOperationException("Parameters are only available while a calculation is running.");

    protected List<string> Warnings { get; } = new();

    protected string WarningText => string.Join("; ", Warnings);

    public ICalculationResult Calculate(IEnumerable<ITestData> data, ITestParameters parameters)
    {
        ArgumentNullException.ThrowIfNull(data);
        ArgumentNullException.ThrowIfNull(parameters);

        _parameters = new TestParameters(DefaultParameters.Values).Add(parameters.Values);
        Warnings.Clear();

        LoadData(data);
        Validate();

        return CalculateCore();
    }

    protected abstract void LoadData(IEnumerable<ITestData> data);

    protected abstract void Validate();

    protected abstract ICalculationResult CalculateCore();
}
