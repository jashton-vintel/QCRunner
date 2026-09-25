using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.ResultBuilding;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class PhTest : AbsorbanceTestBase
{
    public override string Name => "pH";

    public override CalculationType CalculationType => CalculationType.Ph;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 14 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new AbsorbanceWell("Sample", WellRole.Sample, WellCode.A7),
        new AbsorbanceWell("Standard", WellRole.Standard, WellCode.A8),
        new AbsorbanceWell("Background", WellRole.Background, WellCode.A9)
    };

    protected override ICalculationResult CalculateCore()
    {
        AbsorbanceWell sample = GetAbsorbanceWell("Sample");
        AbsorbanceWell standard = GetAbsorbanceWell("Standard");

        // The indicator ratio and the kit calibration curve are proprietary and have been removed.
        var measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Spectrum), DimensionlessUnit.None);

        AcceptanceWindow window = CreateAcceptanceWindow(DimensionlessUnit.None, decimals: 2);
        CalculationAssayResult outcome = Assess(window, measured);

        var controlTable = new ResultTable("Control", "Well", "Signal");
        controlTable.AddRow(standard.Name, RedactedCalculation.SampleValue(standard.Spectrum).ToString("0.000"));

        return new PhCalculationResult
        {
            Result = outcome,
            NumericValue = measured.Value,
            NumericValueUnit = measured.Unit,
            ReportValue = measured.ToString(window.Format),
            AcceptanceCriteriaRange = window.ToString(),
            Warnings = WarningText,
            RawDataChart = BuildSpectrumChart("Raw data", AbsorbanceWells),
            CorrectedDataChart = BuildSpectrumChart("Corrected data", new[] { sample }),
            SuitabilityTable = BuildSuitabilityTable(),
            ControlTable = controlTable,
            MetricsTable = BuildMetricsTable("pH", window, measured, outcome)
        };
    }
}
