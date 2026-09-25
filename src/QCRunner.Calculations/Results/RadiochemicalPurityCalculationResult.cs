using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.Results;

public sealed class RadiochemicalPurityCalculationResult : CalculationResultBase
{
    private IChart? _chromatogramChart;
    private IResultTable? _suitabilityTable;
    private IResultTable? _metricsTable;

    public override CalculationType Calculation => CalculationType.RadiochemicalPurity;

    public override string Name => "Radiochemical purity [%]";

    public IChart? ChromatogramChart { get => _chromatogramChart; init => _chromatogramChart = AddChart(value); }

    public IResultTable? SuitabilityTable { get => _suitabilityTable; init => _suitabilityTable = AddTable(value); }

    public IResultTable? MetricsTable { get => _metricsTable; init => _metricsTable = AddTable(value); }
}
