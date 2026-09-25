using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class ConcentrationTest : LuminescenceTestBase
{
    public override string Name => "Radioactive concentration";

    public override CalculationType CalculationType => CalculationType.Concentration;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 1000000 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new LuminescenceWell("Sample", WellRole.Sample, WellCode.C2),
        new LuminescenceWell("Standard", WellRole.Standard, WellCode.C3)
    };

    protected override ICalculationResult CalculateCore()
    {
        LuminescenceWell sample = GetLuminescenceWell("Sample");

        // The count-to-activity calibration and decay correction to the reference time are proprietary and have been removed.
        var measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Counts), ActivityConcentrationUnit.MegabecquerelsPerMillilitre);

        AcceptanceWindow window = CreateAcceptanceWindow(ActivityConcentrationUnit.MegabecquerelsPerMillilitre, decimals: 1);
        CalculationAssayResult outcome = Assess(window, measured);

        return new ConcentrationCalculationResult
        {
            Result = outcome,
            NumericValue = measured.Value,
            NumericValueUnit = measured.Unit,
            ReportValue = measured.ToString(window.Format),
            AcceptanceCriteriaRange = window.ToString(),
            Warnings = WarningText,
            CountsChart = BuildCountsChart("Counts", LuminescenceWells),
            SuitabilityTable = BuildSuitabilityTable(),
            MetricsTable = BuildMetricsTable("Radioactive concentration", window, measured, outcome)
        };
    }
}
