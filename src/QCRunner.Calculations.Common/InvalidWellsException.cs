using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common;

/// <summary>
/// Raised when the wells handed to a calculation do not cover the roles the test requires.
/// </summary>
public sealed class InvalidWellsException : Exception
{
    public InvalidWellsException(CalculationType calculation, IReadOnlyList<string> missingWells)
        : base($"The {calculation} calculation cannot run because the following wells were not supplied: {string.Join(", ", missingWells)}.")
    {
        Calculation = calculation;
        MissingWells = missingWells;
    }

    public CalculationType Calculation { get; }

    public IReadOnlyList<string> MissingWells { get; }
}
