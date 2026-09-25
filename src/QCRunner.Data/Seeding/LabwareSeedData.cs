namespace QCRunner.Data.Seeding;

internal static class LabwareSeedData
{
    public static Core.Labware.Labware QcPlate()
    {
        return new Core.Labware.Labware
        {
            Name = "96-well QC plate",
            Description = "Clear flat-bottom plate read in the plate reader",
            Manufacturer = "Generic",
            RowCount = 8,
            ColumnCount = 12,
            RowOffset = 11.24,
            ColumnOffset = 14.38,
            WellSpacing = 9.0,
            WellDiameter = 6.4,
            WellDepth = 10.7,
            Height = 14.4
        };
    }

    public static Core.Labware.Labware ReagentPlate()
    {
        return new Core.Labware.Labware
        {
            Name = "96-well reagent plate",
            Description = "Deep-well plate holding the kit reagents",
            Manufacturer = "Generic",
            RowCount = 8,
            ColumnCount = 12,
            RowOffset = 11.24,
            ColumnOffset = 14.38,
            WellSpacing = 9.0,
            WellDiameter = 8.2,
            WellDepth = 39.0,
            Height = 44.0
        };
    }
}
