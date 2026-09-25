using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.ResultBuilding;

public sealed class Axis : IAxis
{
    public string Title { get; set; } = string.Empty;

    public AxisEnabled Enabled { get; set; } = AxisEnabled.Auto;

    public double Interval { get; set; }

    public double Minimum { get; set; } = double.NaN;

    public double Maximum { get; set; } = double.NaN;

    public LabelStyle LabelStyle { get; set; } = new();

    public RgbColor LineColor { get; set; } = RgbColor.Black;

    public Grid MajorGrid { get; set; } = new();

    public Grid MinorGrid { get; set; } = new() { Enabled = false };

    public TickMark MajorTickMark { get; set; } = new();

    public TickMark MinorTickMark { get; set; } = new() { Enabled = false };

    public TextOrientation TextOrientation { get; set; } = TextOrientation.Auto;

    ILabelStyle IAxis.LabelStyle => LabelStyle;

    IRgbColor IAxis.LineColor => LineColor;

    IGrid IAxis.MajorGrid => MajorGrid;

    IGrid IAxis.MinorGrid => MinorGrid;

    ITickMark IAxis.MajorTickMark => MajorTickMark;

    ITickMark IAxis.MinorTickMark => MinorTickMark;
}
