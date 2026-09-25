using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.Results;

public sealed class EthanolCalculationResult : CalculationResultBase
{
    private IChart? _correctedDataChart;
    private IChart? _calibrationChart;
    private IResultTable? _suitabilityTable;
    private IResultTable? _metricsTable;

    public override CalculationType Calculation => CalculationType.Ethanol;

    public override string Name => "Ethanol [%]";

    public IChart? CorrectedDataChart { get => _correctedDataChart; init => _correctedDataChart = AddChart(value); }

    public IChart? CalibrationChart { get => _calibrationChart; init => _calibrationChart = AddChart(value); }

    public IResultTable? SuitabilityTable { get => _suitabilityTable; init => _suitabilityTable = AddTable(value); }

    public IResultTable? MetricsTable { get => _metricsTable; init => _metricsTable = AddTable(value); }
}
