using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.ResultBuilding;

public sealed class Series : ISeries
{
    private readonly List<Coordinate> _data = new();

    public string Name { get; set; } = string.Empty;

    public IReadOnlyList<Coordinate> Data => _data;

    public SeriesType Type { get; set; } = SeriesType.Line;

    public MarkerType MarkerType { get; set; } = MarkerType.Undefined;

    public int MarkerSize { get; set; } = 5;

    public RgbColor MarkerColor { get; set; } = RgbColor.Empty;

    public RgbColor MarkerBorderColor { get; set; } = RgbColor.Empty;

    public int MarkerBorderWidth { get; set; } = 1;

    public DashStyleType DashType { get; set; } = DashStyleType.Solid;

    public int DashWidth { get; set; } = 1;

    public RgbColor Color { get; set; } = RgbColor.Empty;

    IReadOnlyList<ICoordinate> ISeries.Data => _data;

    IRgbColor ISeries.MarkerColor => MarkerColor;

    IRgbColor ISeries.MarkerBorderColor => MarkerBorderColor;

    IRgbColor ISeries.Color => Color;

    public void AddPoint(double x, double y) => _data.Add(new Coordinate(x, y));

    public void AddGap() => _data.Add(Coordinate.Empty);
}
