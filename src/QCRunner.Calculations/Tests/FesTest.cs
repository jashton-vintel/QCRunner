using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class FesTest : LuminescenceTestBase
{
    public override string Name => "FES";

    public override CalculationType CalculationType => CalculationType.Fes;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 1000000 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new LuminescenceWell("Sample", WellRole.Sample, WellCode.D3),
        new LuminescenceWell("Standard", WellRole.Standard, WellCode.D4)
    };

    protected override ICalculationResult CalculateCore()
    {
        LuminescenceWell sample = GetLuminescenceWell("Sample");

        // System suitability checks and the peak area ratio are proprietary and have been removed.
        var measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Counts), PercentageUnit.Percent);

        AcceptanceWindow window = CreateAcceptanceWindow(PercentageUnit.Percent, decimals: 1);
        CalculationAssayResult outcome = Assess(window, measured);

        return new FesCalculationResult
        {
            Result = outcome,
            NumericValue = measured.Value,
            NumericValueUnit = measured.Unit,
            ReportValue = measured.ToString(window.Format),
            AcceptanceCriteriaRange = window.ToString(),
            Warnings = WarningText,
            ChromatogramChart = BuildCountsChart("Chromatogram", LuminescenceWells),
            SuitabilityTable = BuildSuitabilityTable(),
            MetricsTable = BuildMetricsTable("FES purity", window, measured, outcome)
        };
    }
}
