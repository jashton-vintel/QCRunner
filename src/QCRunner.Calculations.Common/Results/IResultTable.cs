namespace QCRunner.Calculations.Common.Results;

/// <summary>
/// A table of formatted text produced by a calculation, such as a system suitability table or
/// the test metrics table shown on the batch report.
/// </summary>
public interface IResultTable
{
    string Name { get; }

    IReadOnlyList<string> Columns { get; }

    IReadOnlyList<IReadOnlyList<string>> Rows { get; }
}
