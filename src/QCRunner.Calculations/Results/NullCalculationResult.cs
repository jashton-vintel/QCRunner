using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Results;

/// <summary>
/// The result handed back when a calculation could not be attempted.
/// </summary>
public sealed class NullCalculationResult : CalculationResultBase
{
    public NullCalculationResult(CalculationType calculation)
    {
        Calculation = calculation;
    }

    public override CalculationType Calculation { get; }

    public override string Name => "No result";
}
