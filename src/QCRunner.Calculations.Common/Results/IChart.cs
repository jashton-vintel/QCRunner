using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Results;

/// <summary>
/// A chart built by a calculation. The application decides how to render it; the engine only
/// describes what to draw.
/// </summary>
public interface IChart
{
    string Title { get; }

    IReadOnlyList<ISeries> Series { get; }

    IAxis AxisX { get; }

    IAxis AxisY { get; }

    DashStyleType BorderDashStyle { get; }
}
