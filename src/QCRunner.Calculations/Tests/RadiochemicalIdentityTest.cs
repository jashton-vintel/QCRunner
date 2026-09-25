using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class RadiochemicalIdentityTest : LuminescenceTestBase
{
    public override string Name => "Radiochemical identity";

    public override CalculationType CalculationType => CalculationType.RadiochemicalIdentity;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 1000000 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new LuminescenceWell("Sample", WellRole.Sample, WellCode.D1),
        new LuminescenceWell("Standard", WellRole.Standard, WellCode.D2)
    };

    protected override ICalculationResult CalculateCore()
    {
        LuminescenceWell sample = GetLuminescenceWell("Sample");

        // Retention factor location against the reference standard is proprietary and has been removed.
        var measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Counts), DimensionlessUnit.RetentionFactor);

        AcceptanceWindow window = CreateAcceptanceWindow(DimensionlessUnit.RetentionFactor, decimals: 2);
        CalculationAssayResult outcome = Assess(window, measured);

        return new RadiochemicalIdentityCalculationResult
        {
            Result = outcome,
            NumericValue = measured.Value,
            NumericValueUnit = measured.Unit,
            ReportValue = measured.ToString(window.Format),
            AcceptanceCriteriaRange = window.ToString(),
            Warnings = WarningText,
            ChromatogramChart = BuildCountsChart("Chromatogram", LuminescenceWells),
            DerivativeChart = BuildCountsChart("Derivative", new[] { sample }),
            SuitabilityTable = BuildSuitabilityTable(),
            MetricsTable = BuildMetricsTable("Retention factor", window, measured, outcome)
        };
    }
}
