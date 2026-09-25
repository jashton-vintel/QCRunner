using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.ResultBuilding;

public sealed class Grid : IGrid
{
    public bool Enabled { get; set; } = true;

    public RgbColor LineColor { get; set; } = RgbColor.LightGray;

    public DashStyleType LineDashStyle { get; set; } = DashStyleType.Solid;

    public int LineWidth { get; set; } = 1;

    IRgbColor IGrid.LineColor => LineColor;
}
