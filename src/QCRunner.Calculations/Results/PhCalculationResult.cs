using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.Results;

public sealed class PhCalculationResult : CalculationResultBase
{
    private IChart? _rawDataChart;
    private IChart? _correctedDataChart;
    private IResultTable? _suitabilityTable;
    private IResultTable? _controlTable;
    private IResultTable? _metricsTable;

    public override CalculationType Calculation => CalculationType.Ph;

    public override string Name => "pH";

    public IChart? RawDataChart { get => _rawDataChart; init => _rawDataChart = AddChart(value); }

    public IChart? CorrectedDataChart { get => _correctedDataChart; init => _correctedDataChart = AddChart(value); }

    public IResultTable? SuitabilityTable { get => _suitabilityTable; init => _suitabilityTable = AddTable(value); }

    public IResultTable? ControlTable { get => _controlTable; init => _controlTable = AddTable(value); }

    public IResultTable? MetricsTable { get => _metricsTable; init => _metricsTable = AddTable(value); }
}
