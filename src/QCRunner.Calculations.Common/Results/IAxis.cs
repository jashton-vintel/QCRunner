using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Results;

public interface IAxis
{
    string Title { get; }

    AxisEnabled Enabled { get; }

    /// <summary>Distance between major ticks; zero lets the viewer choose.</summary>
    double Interval { get; }

    double Minimum { get; }

    double Maximum { get; }

    ILabelStyle LabelStyle { get; }

    IRgbColor LineColor { get; }

    IGrid MajorGrid { get; }

    IGrid MinorGrid { get; }

    ITickMark MajorTickMark { get; }

    ITickMark MinorTickMark { get; }

    TextOrientation TextOrientation { get; }
}
