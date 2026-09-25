using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.ResultBuilding;

public sealed class LabelStyle : ILabelStyle
{
    public bool Enabled { get; set; } = true;

    public string Format { get; set; } = string.Empty;
}
