using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Core.Operations.Reader;

/// <summary>
/// A reader operation that produces data. <see cref="Wells"/> is the mask of wells to read;
/// <see cref="Cycles"/> is how many times the whole mask is read.
/// </summary>
public abstract class MeasurementOperationBase : PlateOperationBase
{
    public int Cycles { get; set; } = 1;

    public List<WellCode> Wells { get; set; } = new();
}
