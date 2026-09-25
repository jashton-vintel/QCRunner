using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;

namespace QCRunner.Calculations.Engine;

/// <summary>
/// A single QC test. Instances are created per calculation by <see cref="TestProvider"/>
/// and are never shared, so a test can keep working state in fields.
/// </summary>
internal interface ITest
{
    string Name { get; }

    CalculationType CalculationType { get; }

    ITestParameters DefaultParameters { get; }

    IReadOnlyList<IWell> WellsNeeded { get; }

    ICalculationResult Calculate(IEnumerable<ITestData> data, ITestParameters parameters);
}
