using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.Results;

public sealed class EndotoxinCalculationResult : CalculationResultBase
{
    private IChart? _rawDataChart;
    private IChart? _normalisedDataChart;
    private IChart? _calibrationChart;
    private IResultTable? _suitabilityTable;
    private IResultTable? _metricsTable;

    public override CalculationType Calculation => CalculationType.Endotoxin;

    public override string Name => "Endotoxin [EU/mL]";

    public IChart? RawDataChart { get => _rawDataChart; init => _rawDataChart = AddChart(value); }

    public IChart? NormalisedDataChart { get => _normalisedDataChart; init => _normalisedDataChart = AddChart(value); }

    public IChart? CalibrationChart { get => _calibrationChart; init => _calibrationChart = AddChart(value); }

    public IResultTable? SuitabilityTable { get => _suitabilityTable; init => _suitabilityTable = AddTable(value); }

    public IResultTable? MetricsTable { get => _metricsTable; init => _metricsTable = AddTable(value); }
}
