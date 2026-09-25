using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Results;

/// <summary>
/// One line or point set on a result chart, described independently of any charting library.
/// </summary>
public interface ISeries
{
    string Name { get; }

    IReadOnlyList<ICoordinate> Data { get; }

    SeriesType Type { get; }

    MarkerType MarkerType { get; }

    int MarkerSize { get; }

    IRgbColor MarkerColor { get; }

    IRgbColor MarkerBorderColor { get; }

    int MarkerBorderWidth { get; }

    DashStyleType DashType { get; }

    int DashWidth { get; }

    IRgbColor Color { get; }
}
