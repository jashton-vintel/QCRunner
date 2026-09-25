namespace QCRunner.Core.Operations.Calculation;

public class CalculationParameter
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public double Value { get; set; }

    public int CalculationOperationId { get; set; }

    public override string ToString() => $"{Name} = {Value}";
}
