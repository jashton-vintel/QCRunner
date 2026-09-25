using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class ClarityTest : AbsorbanceTestBase
{
    public override string Name => "Clarity";

    public override CalculationType CalculationType => CalculationType.Clarity;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 10 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new AbsorbanceWell("Sample", WellRole.Sample, WellCode.A4),
        new AbsorbanceWell("Standard", WellRole.Standard, WellCode.A5),
        new AbsorbanceWell("Background", WellRole.Background, WellCode.A6)
    };

    protected override ICalculationResult CalculateCore()
    {
        AbsorbanceWell sample = GetAbsorbanceWell("Sample");
        AbsorbanceWell standard = GetAbsorbanceWell("Standard");

        // The scattering model and its calibration against the standard are proprietary and have been removed.
        var measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Spectrum), TurbidityUnit.NephelometricTurbidityUnits);

        AcceptanceWindow window = CreateAcceptanceWindow(TurbidityUnit.NephelometricTurbidityUnits, decimals: 2);
        CalculationAssayResult outcome = Assess(window, measured);

        var controlTable = new ResultBuilding.ResultTable("Control", "Well", "Signal");
        controlTable.AddRow(standard.Name, RedactedCalculation.SampleValue(standard.Spectrum).ToString("0.000"));

        return new ClarityCalculationResult
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
            MetricsTable = BuildMetricsTable("Turbidity", window, measured, outcome)
        };
    }
}
