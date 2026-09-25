using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Results;

/// <summary>
/// Everything the application needs to display and report a completed calculation.
/// </summary>
public interface ICalculationResult
{
    CalculationType Calculation { get; }

    /// <summary>The name of the test including the unit it reports in, for example "Colour [mAU]".</summary>
    string Name { get; }

    CalculationAssayResult Result { get; }

    double NumericValue { get; }

    /// <summary>The value formatted for the batch report, for example "&lt; 3".</summary>
    string ReportValue { get; }

    string AcceptanceCriteriaRange { get; }

    string Warnings { get; }

    IReadOnlyList<IChart> Charts { get; }

    IReadOnlyList<IResultTable> Tables { get; }
}
