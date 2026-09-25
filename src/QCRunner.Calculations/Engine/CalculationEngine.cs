using QCRunner.Calculations.Common;
using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;

namespace QCRunner.Calculations.Engine;

public sealed class CalculationEngine : ICalculationEngine
{
    public ICalculationResult Calculate(CalculationType calculation, IEnumerable<ITestData> data, ITestParameters parameters)
    {
        return TestProvider.Create(calculation).Calculate(data, parameters);
    }

    public IReadOnlyList<IWell> GetWellsNeeded(CalculationType calculation)
    {
        return TestProvider.Create(calculation).WellsNeeded;
    }

    public ITestParameters GetDefaultParameters(CalculationType calculation)
    {
        return TestProvider.Create(calculation).DefaultParameters;
    }
}
