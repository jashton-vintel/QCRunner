using QCRunner.Calculations.Common.Results;
using QCRunner.Core.Operations.Calculation;
using QCRunner.Core.ReaderData;

namespace QCRunner.Core.Calculations;

/// <summary>
/// Turns the reader data gathered for a calculation operation into the wells and parameters
/// the calculation engine expects, and returns what the engine produces.
/// </summary>
public interface ICalculationProcessor
{
    ICalculationResult Calculate(CalculationOperation operation, IReadOnlyList<WellReaderData> wellData);
}
