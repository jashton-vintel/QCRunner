using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Calculations.Common.Parameters;
using QCRunner.Calculations.Common.Results;
using QCRunner.Calculations.Common.Wells;
using QCRunner.Calculations.Engine;
using QCRunner.Calculations.Results;
using QCRunner.Calculations.Specifications;
using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Tests;

internal sealed class ColourTest : AbsorbanceTestBase
{
    public override string Name => "Colour";

    public override CalculationType CalculationType => CalculationType.Colour;

    public override ITestParameters DefaultParameters => new TestParameters
    {
        { "LowerLimit", 0 },
        { "UpperLimit", 1000 }
    };

    public override IReadOnlyList<IWell> WellsNeeded => new IWell[]
    {
        new AbsorbanceWell("Sample", WellRole.Sample, WellCode.A1),
        new AbsorbanceWell("Standard", WellRole.Standard, WellCode.A2),
        new AbsorbanceWell("Background", WellRole.Background, WellCode.A3)
    };

    protected override ICalculationResult CalculateCore()
    {
        AbsorbanceWell sample = GetAbsorbanceWell("Sample");

        // Blank subtraction, replicate selection and the peak search are proprietary and have been removed.
        DimensionedValue measured = new DimensionedValue(RedactedCalculation.SampleValue(sample.Spectrum), AbsorbanceUnit.AbsorbanceUnits)
            .ConvertTo(AbsorbanceUnit.MilliAbsorbanceUnits);

        AcceptanceWindow window = CreateAcceptanceWindow(AbsorbanceUnit.MilliAbsorbanceUnits, decimals: 1);
        CalculationAssayResult outcome = Assess(window, measured);

        return new ColourCalculationResult
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
            MetricsTable = BuildMetricsTable("Sample maximum absorbance", window, measured, outcome)
        };
    }
}
