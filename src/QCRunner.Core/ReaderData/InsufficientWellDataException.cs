using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Common;
using QCRunner.Core.Operations.Calculation;

namespace QCRunner.Core.ReaderData;

/// <summary>
/// Raised when a calculation asks for wells that no reader operation in the batch has read yet.
/// </summary>
public sealed class InsufficientWellDataException : Exception
{
    public InsufficientWellDataException(CalculationType calculation, IReadOnlyList<CalculationWell> missingWells)
        : base(BuildMessage(calculation, missingWells))
    {
        Calculation = calculation;
        MissingWells = missingWells;
    }

    public CalculationType Calculation { get; }

    public IReadOnlyList<CalculationWell> MissingWells { get; }

    private static string BuildMessage(CalculationType calculation, IReadOnlyList<CalculationWell> missingWells)
    {
        IEnumerable<string> descriptions = missingWells.Select(well => $"{well.Code} ({well.Name}, {well.DataType})");

        return $"There is insufficient data to perform the {calculation.GetDescription()} calculation. No reader data has been collected for: {string.Join(", ", descriptions)}.";
    }
}
