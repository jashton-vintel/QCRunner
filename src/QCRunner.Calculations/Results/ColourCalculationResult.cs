using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.Results;

public sealed class ColourCalculationResult : CalculationResultBase
{
    private IChart? _rawDataChart;
    private IChart? _correctedDataChart;
    private IResultTable? _suitabilityTable;
    private IResultTable? _metricsTable;

    public override CalculationType Calculation => CalculationType.Colour;

    public override string Name => "Colour [mAU]";

    public IChart? RawDataChart { get => _rawDataChart; init => _rawDataChart = AddChart(value); }

    public IChart? CorrectedDataChart { get => _correctedDataChart; init => _correctedDataChart = AddChart(value); }

    public IResultTable? SuitabilityTable { get => _suitabilityTable; init => _suitabilityTable = AddTable(value); }

    public IResultTable? MetricsTable { get => _metricsTable; init => _metricsTable = AddTable(value); }
}
