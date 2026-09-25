using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Core.Operations.Calculation;

/// <summary>
/// Runs one QC test over reader data collected earlier in the batch.
/// </summary>
public class CalculationOperation : OperationBase
{
    public override OperationType Type => OperationType.Calculation;

    public CalculationType CalculationType { get; set; }

    public virtual ICollection<CalculationParameter> Parameters { get; set; } = new List<CalculationParameter>();

    /// <summary>The wells whose reader data the calculation needs, and the role each one plays.</summary>
    public virtual ICollection<CalculationWell> Wells { get; set; } = new List<CalculationWell>();
}
