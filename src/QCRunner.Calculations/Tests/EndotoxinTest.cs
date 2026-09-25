using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class EndotoxinTest : AbsorbanceTestBase
{
    public override string Name => "Endotoxin";

    public override CalculationType CalculationType => CalculationType.Endotoxin;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 100 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new AbsorbanceWell("Sample", WellRole.Sample, WellCode.B1),
        new AbsorbanceWell("Standard", WellRole.Standard, WellCode.B2),
        new AbsorbanceWell("Background", WellRole.Background, WellCode.B3)
    };

    protected override ICalculationResult CalculateCore()
    {
        AbsorbanceWell sample = GetAbsorbanceWell("Sample");

        // Kinetic onset detection and the standard curve fit are proprietary and have been removed.
        var measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Spectrum), EndotoxinUnit.EndotoxinUnitsPerMillilitre);

        AcceptanceWindow window = CreateAcceptanceWindow(EndotoxinUnit.EndotoxinUnitsPerMillilitre, decimals: 2);
        CalculationAssayResult outcome = Assess(window, measured);

        return new EndotoxinCalculationResult
        {
            Result = outcome,
            NumericValue = measured.Value,
            NumericValueUnit = measured.Unit,
            ReportValue = measured.ToString(window.Format),
            AcceptanceCriteriaRange = window.ToString(),
            Warnings = WarningText,
            RawDataChart = BuildSpectrumChart("Raw data", AbsorbanceWells),
            NormalisedDataChart = BuildSpectrumChart("Normalised data", new[] { sample }),
            CalibrationChart = BuildSpectrumChart("Calibration", Wells.OfType<AbsorbanceWell>().Where(well => well.Role == WellRole.Standard)),
            SuitabilityTable = BuildSuitabilityTable(),
            MetricsTable = BuildMetricsTable("Endotoxin concentration", window, measured, outcome)
        };
    }
}
