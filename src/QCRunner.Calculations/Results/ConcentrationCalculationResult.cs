using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.Results;

public sealed class ConcentrationCalculationResult : CalculationResultBase
{
    private IChart? _countsChart;
    private IResultTable? _suitabilityTable;
    private IResultTable? _metricsTable;

    public override CalculationType Calculation => CalculationType.Concentration;

    public override string Name => $"Concentration [{NumericValueUnit?.Symbol ?? "MBq/mL"}]";

    public IChart? CountsChart { get => _countsChart; init => _countsChart = AddChart(value); }

    public IResultTable? SuitabilityTable { get => _suitabilityTable; init => _suitabilityTable = AddTable(value); }

    public IResultTable? MetricsTable { get => _metricsTable; init => _metricsTable = AddTable(value); }
}
