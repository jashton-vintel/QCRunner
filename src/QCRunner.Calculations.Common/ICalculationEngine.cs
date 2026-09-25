using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;

namespace QCRunner.Calculations.Common;

/// <summary>
/// The public face of the calculation engine. The application supplies reader data as wells
/// and receives a result object; it never sees the individual test classes.
/// </summary>
public interface ICalculationEngine
{
    ICalculationResult Calculate(CalculationType calculation, IEnumerable<ITestData> data, ITestParameters parameters);

    /// <summary>The wells a calculation expects, used by the method designer to pre-populate calculation operations.</summary>
    IReadOnlyList<IWell> GetWellsNeeded(CalculationType calculation);

    ITestParameters GetDefaultParameters(CalculationType calculation);
}
