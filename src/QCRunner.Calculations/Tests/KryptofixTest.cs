using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class KryptofixTest : AbsorbanceTestBase
{
    public override string Name => "Kryptofix";

    public override CalculationType CalculationType => CalculationType.Kryptofix;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 100 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new AbsorbanceWell("Sample", WellRole.Sample, WellCode.B7),
        new AbsorbanceWell("Standard", WellRole.Standard, WellCode.B8),
        new AbsorbanceWell("Background", WellRole.Background, WellCode.B9)
    };

    protected override ICalculationResult CalculateCore()
    {
        AbsorbanceWell sample = GetAbsorbanceWell("Sample");

        // Scatter correction and the colorimetric calibration are proprietary and have been removed.
        var measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Spectrum), MassConcentrationUnit.MicrogramsPerMillilitre);

        AcceptanceWindow window = CreateAcceptanceWindow(MassConcentrationUnit.MicrogramsPerMillilitre, decimals: 1);
        CalculationAssayResult outcome = Assess(window, measured);

        return new KryptofixCalculationResult
        {
            Result = outcome,
            NumericValue = measured.Value,
            NumericValueUnit = measured.Unit,
            ReportValue = measured.ToString(window.Format),
            AcceptanceCriteriaRange = window.ToString(),
            Warnings = WarningText,
            ScatterCorrectedDataChart = BuildSpectrumChart("Scatter corrected data", AbsorbanceWells),
            CalibrationChart = BuildSpectrumChart("Calibration", Wells.OfType<AbsorbanceWell>().Where(well => well.Role == WellRole.Standard)),
            SuitabilityTable = BuildSuitabilityTable(),
            MetricsTable = BuildMetricsTable("Kryptofix concentration", window, measured, outcome)
        };
    }
}
