using QCRunner.Calculations.Common.Results;
using QCRunner.Core.Operations.Calculation;

namespace QCRunner.Core.Runners;

public sealed class CalculationOperationResult : OperationResult
{
    public CalculationOperationResult(CalculationOperation operation, ICalculationResult calculationResult)
        : base(operation, OperationResultStatus.Successful)
    {
        ArgumentNullException.ThrowIfNull(calculationResult);
        CalculationResult = calculationResult;
    }

    public ICalculationResult CalculationResult { get; }
}
