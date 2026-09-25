using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Core.Methods;
using QCRunner.Core.Operations;
using QCRunner.Core.Operations.Calculation;
using QCRunner.Core.Operations.Pause;
using QCRunner.Core.Operations.Prompt;
using QCRunner.Core.Operations.Reader;
using QCRunner.Core.Operations.Robot;

namespace QCRunner.Data.Seeding;

/// <summary>
/// Builders that keep the seeded methods readable. Indexes are assigned from the order the
/// operations, groups and phases are listed in.
/// </summary>
internal static class SeedOperations
{
    public static Phase Phase(string name, string description, params OperationGroup[] groups)
    {
        var phase = new Phase { Name = name, Description = description };

        for (int index = 0; index < groups.Length; index++)
        {
            groups[index].Index = index + 1;
            phase.OperationGroups.Add(groups[index]);
        }

        return phase;
    }

    public static OperationGroup Group(string name, string description, params OperationBase[] operations)
    {
        var group = new OperationGroup { Name = name, Description = description };

        for (int index = 0; index < operations.Length; index++)
        {
            operations[index].Index = index + 1;
            group.Operations.Add(operations[index]);
        }

        return group;
    }

    public static HeatToTemperatureOperation HeatReader(string name, double temperature, HeatMode mode)
    {
        return new HeatToTemperatureOperation { Name = name, Description = name, Temperature = temperature, Mode = mode };
    }

    public static HeatedPlateOperation HeatNest(string name, int position, double temperature)
    {
        return new HeatedPlateOperation { Name = name, Description = name, Position = position, Temperature = temperature };
    }

    public static PromptOperation Prompt(string name, string title, string text, PromptButtons buttons)
    {
        return new PromptOperation { Name = name, Description = name, MessageTitle = title, MessageText = text, Buttons = buttons };
    }

    public static PauseOperation Pause(string name, int seconds)
    {
        return new PauseOperation { Name = name, Description = name, DurationSeconds = seconds };
    }

    public static ShakeOperation Shake(string name, Core.Labware.Labware plate, ShakeIntensity intensity, int seconds, int cycles)
    {
        return new ShakeOperation
        {
            Name = name,
            Description = name,
            Plate = plate,
            Intensity = intensity,
            ShakeTime = TimeSpan.FromSeconds(seconds),
            ShakeCycles = cycles
        };
    }

    public static AbsorbanceOperation Absorbance(string name, Core.Labware.Labware plate, int flashes, int cycles, int delay, params WellCode[] wells)
    {
        return new AbsorbanceOperation
        {
            Name = name,
            Description = name,
            Plate = plate,
            Flashes = flashes,
            Cycles = cycles,
            Delay = delay,
            Wells = wells.ToList()
        };
    }

    public static LuminescenceOperation Luminescence(string name, Core.Labware.Labware plate, int countingSeconds, int cycles, params WellCode[] wells)
    {
        return new LuminescenceOperation
        {
            Name = name,
            Description = name,
            Plate = plate,
            CountingTime = TimeSpan.FromSeconds(countingSeconds),
            Cycles = cycles,
            Wells = wells.ToList()
        };
    }

    public static AspirateOperation Aspirate(string name, Core.Labware.Labware plate, int position, string point, double volume)
    {
        return new AspirateOperation
        {
            Name = name,
            Description = name,
            Plate = plate,
            Position = position,
            Point = point,
            Volume = volume,
            AspirateHeight = 2.0
        };
    }

    public static DispenseOperation Dispense(string name, Core.Labware.Labware plate, int position, string point, double volume, int mixCycles = 0)
    {
        return new DispenseOperation
        {
            Name = name,
            Description = name,
            Plate = plate,
            Position = position,
            Point = point,
            Volume = volume,
            DispenseHeight = 5.0,
            MixCycles = mixCycles,
            MixVolume = mixCycles > 0 ? volume / 2 : 0
        };
    }

    public static CalculationOperation Calculation(string name, CalculationType type, double lowerLimit, double upperLimit, params CalculationWell[] wells)
    {
        var operation = new CalculationOperation { Name = name, Description = name, CalculationType = type };

        operation.Parameters.Add(new CalculationParameter { Name = "LowerLimit", Value = lowerLimit });
        operation.Parameters.Add(new CalculationParameter { Name = "UpperLimit", Value = upperLimit });

        foreach (CalculationWell well in wells)
        {
            operation.Wells.Add(well);
        }

        return operation;
    }

    public static CalculationWell AbsorbanceWell(string name, WellRole role, WellCode code)
    {
        return new CalculationWell { Name = name, Role = role, Code = code, DataType = ReaderDataType.Absorbance };
    }

    public static CalculationWell LuminescenceWell(string name, WellRole role, WellCode code)
    {
        return new CalculationWell { Name = name, Role = role, Code = code, DataType = ReaderDataType.Luminescence };
    }
}
