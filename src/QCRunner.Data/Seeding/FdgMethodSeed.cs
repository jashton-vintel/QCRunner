using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Core.Methods;
using QCRunner.Core.Operations.Prompt;
using QCRunner.Core.Operations.Reader;
using static QCRunner.Data.Seeding.SeedOperations;

namespace QCRunner.Data.Seeding;

/// <summary>
/// An example fludeoxyglucose method: a preparation phase, a cold phase of colorimetric tests
/// and a hot phase of radioactivity tests.
/// </summary>
internal static class FdgMethodSeed
{
    public static Method Create(Core.Labware.Labware qcPlate, Core.Labware.Labware reagentPlate)
    {
        var method = new Method
        {
            Code = "FDG",
            Name = "FDG release testing",
            Description = "Example method covering appearance, chemical purity and radioactivity tests for an FDG batch.",
            Version = "1.0",
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
        return Phase("Preparation", "Bring the instruments to temperature and load the plates",
            Group("Instrument preparation", "Heaters on and plates loaded",
                HeatReader("Heat reader to 37 C", 37, HeatMode.WaitUntilReached),
                HeatNest("Heat nest 1 to 37 C", 1, 37),
                Prompt("Load plates", "Load plates", "Place the QC plate in position 1 and the reagent plate in position 3, then press OK.", PromptButtons.OkOnly)));
    }

    private static Phase Cold(Core.Labware.Labware qcPlate, Core.Labware.Labware reagentPlate)
    {
        return Phase("Cold", "Tests that run before the radioactive sample is added",
            Group("Reagent transfer", "Move reagents from the reagent plate into the QC plate",
                Aspirate("Aspirate colour reagent", reagentPlate, 3, "A1", 100),
                Dispense("Dispense colour reagent", qcPlate, 1, "A1", 100, mixCycles: 3),
                Aspirate("Aspirate pH indicator", reagentPlate, 3, "A2", 50),
                Dispense("Dispense pH indicator", qcPlate, 1, "A7", 50, mixCycles: 3)),
            Group("Colour, clarity and pH", "Read the appearance wells and evaluate them",
                Shake("Mix plate", qcPlate, ShakeIntensity.Medium, 10, 1),
                Absorbance("Read appearance wells", qcPlate, 10, 1, 0,
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
            Group("Endotoxin, ethanol and kryptofix", "Incubate, then read the chemistry wells",
                Pause("Incubate", 5),
                Absorbance("Read chemistry wells", qcPlate, 10, 2, 5000,
                    WellCode.B1, WellCode.B2, WellCode.B3, WellCode.B4, WellCode.B5, WellCode.B6, WellCode.B7, WellCode.B8, WellCode.B9),
                Calculation("Endotoxin", CalculationType.Endotoxin, 0, 100,
                    AbsorbanceWell("Sample", WellRole.Sample, WellCode.B1),
                    AbsorbanceWell("Standard", WellRole.Standard, WellCode.B2),
                    AbsorbanceWell("Background", WellRole.Background, WellCode.B3)),
                Calculation("Ethanol", CalculationType.Ethanol, 0, 100,
                    AbsorbanceWell("Sample", WellRole.Sample, WellCode.B4),
                    AbsorbanceWell("Standard", WellRole.Standard, WellCode.B5),
                    AbsorbanceWell("Background", WellRole.Background, WellCode.B6)),
                Calculation("Kryptofix", CalculationType.Kryptofix, 0, 100,
                    AbsorbanceWell("Sample", WellRole.Sample, WellCode.B7),
                    AbsorbanceWell("Standard", WellRole.Standard, WellCode.B8),
                    AbsorbanceWell("Background", WellRole.Background, WellCode.B9))));
    }

    private static Phase Hot(Core.Labware.Labware qcPlate)
    {
        return Phase("Hot", "Tests that need the radioactive sample",
            Group("Sample loading", "Operator adds the radioactive sample",
                Prompt("Add radioactive sample", "Add sample", "Add the radioactive sample to wells C1 to C3 and D1 to D2, then press Yes to continue.", PromptButtons.YesCancel)),
            Group("Half-life and concentration", "Count the decay wells over several cycles",
                Luminescence("Count decay wells", qcPlate, 2, 5, WellCode.C1, WellCode.C2, WellCode.C3),
                Calculation("Half-life", CalculationType.HalfLife, 0, 1000000,
                    LuminescenceWell("Sample", WellRole.Sample, WellCode.C1)),
                Calculation("Radioactive concentration", CalculationType.Concentration, 0, 1000000,
                    LuminescenceWell("Sample", WellRole.Sample, WellCode.C2),
                    LuminescenceWell("Standard", WellRole.Standard, WellCode.C3))),
            Group("Radiochemical purity and identity", "Scan the TLC strips",
                Luminescence("Scan TLC strips", qcPlate, 1, 3, WellCode.D1, WellCode.D2),
                Calculation("Radiochemical purity", CalculationType.RadiochemicalPurity, 0, 1000000,
                    LuminescenceWell("Sample", WellRole.Sample, WellCode.D1)),
                Calculation("Radiochemical identity", CalculationType.RadiochemicalIdentity, 0, 1000000,
                    LuminescenceWell("Sample", WellRole.Sample, WellCode.D1),
                    LuminescenceWell("Standard", WellRole.Standard, WellCode.D2))));
    }
}
