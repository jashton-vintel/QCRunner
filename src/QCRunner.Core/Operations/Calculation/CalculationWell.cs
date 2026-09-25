using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Core.Operations.Calculation;

public class CalculationWell
{
    public int Id { get; set; }

    /// <summary>The name the calculation knows the well by, for example "Sample".</summary>
    public string Name { get; set; } = string.Empty;

    public WellCode Code { get; set; }

    public WellRole Role { get; set; }

    /// <summary>Which kind of reader data to look for at <see cref="Code"/>.</summary>
    public ReaderDataType DataType { get; set; }

    /// <summary>The known value of a standard; null for samples.</summary>
    public double? TrueValue { get; set; }

    public int CalculationOperationId { get; set; }

    public override string ToString() => $"{Name} at {Code} ({Role}, {DataType})";
}
