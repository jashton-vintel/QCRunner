using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Results;

/// <summary>
/// Shared shape of every calculation result. Concrete results expose their charts and tables
/// as named properties for the result viewer and also register them in the generic
/// <see cref="Charts"/> and <see cref="Tables"/> lists for reporting and export.
/// </summary>
public abstract class CalculationResultBase : ICalculationResult
{
    private readonly List<IChart> _charts = new();
    private readonly List<IResultTable> _tables = new();

    public abstract CalculationType Calculation { get; }

    public abstract string Name { get; }

    public CalculationAssayResult Result { get; init; } = CalculationAssayResult.Undefined;

    public double NumericValue { get; init; } = double.NaN;

    public Unit? NumericValueUnit { get; init; }

    public string ReportValue { get; init; } = string.Empty;

    public string AcceptanceCriteriaRange { get; init; } = string.Empty;

    public string Warnings { get; init; } = string.Empty;

    public IReadOnlyList<IChart> Charts => _charts;

    public IReadOnlyList<IResultTable> Tables => _tables;

    public override string ToString() => $"{Name}: {ReportValue} ({Result})";

    protected TChart? AddChart<TChart>(TChart? chart)
        where TChart : class, IChart
    {
        if (chart is not null)
        {
            _charts.Add(chart);
        }

        return chart;
    }

    protected TTable? AddTable<TTable>(TTable? table)
        where TTable : class, IResultTable
    {
        if (table is not null)
        {
            _tables.Add(table);
        }

        return table;
    }
}
