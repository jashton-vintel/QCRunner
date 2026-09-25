using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.Results;

public sealed class KryptofixCalculationResult : CalculationResultBase
{
    private IChart? _scatterCorrectedDataChart;
    private IChart? _calibrationChart;
    private IResultTable? _suitabilityTable;
    private IResultTable? _metricsTable;

    public override CalculationType Calculation => CalculationType.Kryptofix;

    public override string Name => "Kryptofix [ug/mL]";

    public IChart? ScatterCorrectedDataChart { get => _scatterCorrectedDataChart; init => _scatterCorrectedDataChart = AddChart(value); }

    public IChart? CalibrationChart { get => _calibrationChart; init => _calibrationChart = AddChart(value); }

    public IResultTable? SuitabilityTable { get => _suitabilityTable; init => _suitabilityTable = AddTable(value); }

    public IResultTable? MetricsTable { get => _metricsTable; init => _metricsTable = AddTable(value); }
}
