using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class EthanolTest : AbsorbanceTestBase
{
    public override string Name => "Ethanol";

    public override CalculationType CalculationType => CalculationType.Ethanol;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 100 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new AbsorbanceWell("Sample", WellRole.Sample, WellCode.B4),
        new AbsorbanceWell("Standard", WellRole.Standard, WellCode.B5),
        new AbsorbanceWell("Background", WellRole.Background, WellCode.B6)
    };

    protected override ICalculationResult CalculateCore()
    {
        AbsorbanceWell sample = GetAbsorbanceWell("Sample");

        // Smoothing, baseline correction and the enzymatic calibration are proprietary and have been removed.
        var measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Spectrum), PercentageUnit.Percent);

        AcceptanceWindow window = CreateAcceptanceWindow(PercentageUnit.Percent, decimals: 2);
        CalculationAssayResult outcome = Assess(window, measured);

        return new EthanolCalculationResult
        {
            Result = outcome,
            NumericValue = measured.Value,
            NumericValueUnit = measured.Unit,
            ReportValue = measured.ToString(window.Format),
            AcceptanceCriteriaRange = window.ToString(),
            Warnings = WarningText,
            CorrectedDataChart = BuildSpectrumChart("Corrected data", AbsorbanceWells),
            CalibrationChart = BuildSpectrumChart("Calibration", Wells.OfType<AbsorbanceWell>().Where(well => well.Role == WellRole.Standard)),
            SuitabilityTable = BuildSuitabilityTable(),
            MetricsTable = BuildMetricsTable("Ethanol content", window, measured, outcome)
        };
    }
}
