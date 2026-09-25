using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common;

public sealed class UnsupportedCalculationException : Exception
{
    public UnsupportedCalculationException(CalculationType calculation)
        : base($"No test is registered for the {calculation} calculation.")
    {
        Calculation = calculation;
    }

    public CalculationType Calculation { get; }
}
