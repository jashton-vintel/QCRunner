using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Results;

public interface IGrid
{
    bool Enabled { get; }

    IRgbColor LineColor { get; }

    DashStyleType LineDashStyle { get; }

    int LineWidth { get; }
}
