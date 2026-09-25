using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.Results;

public sealed class HalfLifeCalculationResult : CalculationResultBase
{
    private IChart? _decayChart;
    private IChart? _residualsChart;
    private IResultTable? _suitabilityTable;
    private IResultTable? _metricsTable;

    public override CalculationType Calculation => CalculationType.HalfLife;

    public override string Name => $"Half-life [{NumericValueUnit?.Symbol ?? "min"}]";

    public IChart? DecayChart { get => _decayChart; init => _decayChart = AddChart(value); }

    public IChart? ResidualsChart { get => _residualsChart; init => _residualsChart = AddChart(value); }

    public IResultTable? SuitabilityTable { get => _suitabilityTable; init => _suitabilityTable = AddTable(value); }

    public IResultTable? MetricsTable { get => _metricsTable; init => _metricsTable = AddTable(value); }
}
