using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.Results;

public sealed class RadiochemicalIdentityCalculationResult : CalculationResultBase
{
    private IChart? _chromatogramChart;
    private IChart? _derivativeChart;
    private IResultTable? _suitabilityTable;
    private IResultTable? _metricsTable;

    public override CalculationType Calculation => CalculationType.RadiochemicalIdentity;

    public override string Name => "Radiochemical identity [Rf]";

    public IChart? ChromatogramChart { get => _chromatogramChart; init => _chromatogramChart = AddChart(value); }

    public IChart? DerivativeChart { get => _derivativeChart; init => _derivativeChart = AddChart(value); }

    public IResultTable? SuitabilityTable { get => _suitabilityTable; init => _suitabilityTable = AddTable(value); }

    public IResultTable? MetricsTable { get => _metricsTable; init => _metricsTable = AddTable(value); }
}
