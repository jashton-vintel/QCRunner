using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class RadiochemicalPurityTest : LuminescenceTestBase
{
    public override string Name => "Radiochemical purity";

    public override CalculationType CalculationType => CalculationType.RadiochemicalPurity;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 1000000 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new LuminescenceWell("Sample", WellRole.Sample, WellCode.D1)
    };

    protected override ICalculationResult CalculateCore()
    {
        LuminescenceWell sample = GetLuminescenceWell("Sample");

        // Peak detection along the strip and the region integration are proprietary and have been removed.
        var measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Counts), PercentageUnit.Percent);

        AcceptanceWindow window = CreateAcceptanceWindow(PercentageUnit.Percent, decimals: 1);
        CalculationAssayResult outcome = Assess(window, measured);

        return new RadiochemicalPurityCalculationResult
        {
            Result = outcome,
            NumericValue = measured.Value,
            NumericValueUnit = measured.Unit,
            ReportValue = measured.ToString(window.Format),
            AcceptanceCriteriaRange = window.ToString(),
            Warnings = WarningText,
            ChromatogramChart = BuildCountsChart("Chromatogram", LuminescenceWells),
            SuitabilityTable = BuildSuitabilityTable(),
            MetricsTable = BuildMetricsTable("Radiochemical purity", window, measured, outcome)
        };
    }
}
