using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class HalfLifeTest : LuminescenceTestBase
{
    public override string Name => "Half-life";

    public override CalculationType CalculationType => CalculationType.HalfLife;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 1000000 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new LuminescenceWell("Sample", WellRole.Sample, WellCode.C1)
    };

    protected override ICalculationResult CalculateCore()
    {
        LuminescenceWell sample = GetLuminescenceWell("Sample");

        // The exponential decay fit and its residual analysis are proprietary and have been removed.
        var measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Counts), TimeUnit.Minutes);

        AcceptanceWindow window = CreateAcceptanceWindow(TimeUnit.Minutes, decimals: 1);
        CalculationAssayResult outcome = Assess(window, measured);

        return new HalfLifeCalculationResult
        {
            Result = outcome,
            NumericValue = measured.Value,
            NumericValueUnit = measured.Unit,
            ReportValue = measured.ToString(window.Format),
            AcceptanceCriteriaRange = window.ToString(),
            Warnings = WarningText,
            DecayChart = BuildCountsChart("Decay", LuminescenceWells),
            ResidualsChart = BuildCountsChart("Residuals", new[] { sample }),
            SuitabilityTable = BuildSuitabilityTable(),
            MetricsTable = BuildMetricsTable("Half-life", window, measured, outcome)
        };
    }
}
