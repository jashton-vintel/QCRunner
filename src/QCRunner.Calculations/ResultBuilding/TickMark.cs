using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.ResultBuilding;

public sealed class TickMark : ITickMark
{
    public bool Enabled { get; set; } = true;

    public RgbColor LineColor { get; set; } = RgbColor.Black;

    public DashStyleType LineDashStyle { get; set; } = DashStyleType.Solid;

    public int LineWidth { get; set; } = 1;

    public float Size { get; set; } = 1.0f;

    public TickMarkStyle TickMarkStyle { get; set; } = TickMarkStyle.OutsideArea;

    IRgbColor ITickMark.LineColor => LineColor;
}
