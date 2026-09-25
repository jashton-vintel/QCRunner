using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Results;

public interface ITickMark
{
    bool Enabled { get; }

    IRgbColor LineColor { get; }

    DashStyleType LineDashStyle { get; }

    int LineWidth { get; }

    /// <summary>Length of the tick as a percentage of the axis length.</summary>
    float Size { get; }

    TickMarkStyle TickMarkStyle { get; }
}
