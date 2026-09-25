using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Core.Methods;
using QCRunner.Core.Operations.Prompt;
using QCRunner.Core.Operations.Reader;
using static QCRunner.Data.Seeding.SeedOperations;

namespace QCRunner.Data.Seeding;

/// <summary>
/// An example gallium-68 method. Shorter than the FDG method: no kryptofix or endotoxin
/// chemistry, and the reader runs at room temperature.
/// </summary>
internal static class GalliumMethodSeed
{
    public static Method Create(Core.Labware.Labware qcPlate, Core.Labware.Labware reagentPlate)
    {
        var method = new Method
        {
            Code = "GA68",
            Name = "Ga-68 release testing",
            Description = "Example method covering appearance, ethanol and radioactivity tests for a gallium-68 batch.",
            Version = "1.2",
            Published = true
        };

        method.Phases.Add(Preparation());
        method.Phases.Add(Cold(qcPlate, reagentPlate));
        method.Phases.Add(Hot(qcPlate));

        for (int index = 0; index < method.Phases.Count; index++)
        {
            method.Phases.ElementAt(index).Index = index + 1;
        }

        return method;
    }

    private static Phase Preparation()
    {
        return Phase("Preparation", "Set the reader to room temperature and load the plates",
            Group("Instrument preparation", "Reader set point and plate loading",
                HeatReader("Set reader to 25 C", 25, HeatMode.SetAndDiscard),
                Prompt("Load plates", "Load plates", "Place the QC plate in position 1 and the reagent plate in position 3, then press OK.", PromptButtons.OkOnly)));
    }

    private static Phase Cold(Core.Labware.Labware qcPlate, Core.Labware.Labware reagentPlate)
    {
        return Phase("Cold", "Appearance and ethanol tests",
            Group("Reagent transfer", "Move the colour reagent into the QC plate",
                Aspirate("Aspirate colour reagent", reagentPlate, 3, "A1", 100),
                Dispense("Dispense colour reagent", qcPlate, 1, "A1", 100, mixCycles: 2)),
            Group("Appearance", "Read and evaluate the appearance wells",
                Shake("Mix plate", qcPlate, ShakeIntensity.Low, 5, 1),
                Absorbance("Read appearance wells", qcPlate, 5, 1, 0,
                    WellCode.A1, WellCode.A2, WellCode.A3, WellCode.A4, WellCode.A5, WellCode.A6, WellCode.A7, WellCode.A8, WellCode.A9),
                Calculation("Colour", CalculationType.Colour, 0, 1000,
                    AbsorbanceWell("Sample", WellRole.Sample, WellCode.A1),
                    AbsorbanceWell("Standard", WellRole.Standard, WellCode.A2),
                    AbsorbanceWell("Background", WellRole.Background, WellCode.A3)),
                Calculation("Clarity", CalculationType.Clarity, 0, 10,
                    AbsorbanceWell("Sample", WellRole.Sample, WellCode.A4),
                    AbsorbanceWell("Standard", WellRole.Standard, WellCode.A5),
                    AbsorbanceWell("Background", WellRole.Background, WellCode.A6)),
                Calculation("pH", CalculationType.Ph, 0, 14,
                    AbsorbanceWell("Sample", WellRole.Sample, WellCode.A7),
                    AbsorbanceWell("Standard", WellRole.Standard, WellCode.A8),
                    AbsorbanceWell("Background", WellRole.Background, WellCode.A9))),
            Group("Ethanol", "Read and evaluate the ethanol wells",
                Absorbance("Read ethanol wells", qcPlate, 5, 1, 0, WellCode.B4, WellCode.B5, WellCode.B6),
                Calculation("Ethanol", CalculationType.Ethanol, 0, 100,
                    AbsorbanceWell("Sample", WellRole.Sample, WellCode.B4),
                    AbsorbanceWell("Standard", WellRole.Standard, WellCode.B5),
                    AbsorbanceWell("Background", WellRole.Background, WellCode.B6))));
    }

    private static Phase Hot(Core.Labware.Labware qcPlate)
    {
        return Phase("Hot", "Radioactivity tests",
            Group("Sample loading", "Operator adds the radioactive sample",
                Prompt("Add radioactive sample", "Add sample", "Add the radioactive sample to wells C1 to C3 and D1 to D2, then press Yes to continue.", PromptButtons.YesCancel)),
            Group("Half-life and concentration", "Count the decay wells",
                Luminescence("Count decay wells", qcPlate, 1, 4, WellCode.C1, WellCode.C2, WellCode.C3),
                Calculation("Half-life", CalculationType.HalfLife, 0, 1000000,
                    LuminescenceWell("Sample", WellRole.Sample, WellCode.C1)),
                Calculation("Radioactive concentration", CalculationType.Concentration, 0, 1000000,
                    LuminescenceWell("Sample", WellRole.Sample, WellCode.C2),
                    LuminescenceWell("Standard", WellRole.Standard, WellCode.C3))),
            Group("Radiochemical purity and identity", "Scan the TLC strips",
                Luminescence("Scan TLC strips", qcPlate, 1, 2, WellCode.D1, WellCode.D2),
                Calculation("Radiochemical purity", CalculationType.RadiochemicalPurity, 0, 1000000,
                    LuminescenceWell("Sample", WellRole.Sample, WellCode.D1)),
                Calculation("Radiochemical identity", CalculationType.RadiochemicalIdentity, 0, 1000000,
                    LuminescenceWell("Sample", WellRole.Sample, WellCode.D1),
                    LuminescenceWell("Standard", WellRole.Standard, WellCode.D2))));
    }
}
