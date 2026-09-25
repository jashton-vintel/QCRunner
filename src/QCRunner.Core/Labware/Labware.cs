namespace QCRunner.Core.Labware;

public class Labware : ILabware
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string Manufacturer { get; set; } = string.Empty;

    public int RowCount { get; set; }

    public int ColumnCount { get; set; }

    public double RowOffset { get; set; }

    public double ColumnOffset { get; set; }

    public double WellSpacing { get; set; }

    public double WellDiameter { get; set; }

    public double WellDepth { get; set; }

    public double Height { get; set; }

    public int WellCount => RowCount * ColumnCount;

    public override string ToString() => $"{Name} ({RowCount} x {ColumnCount})";
}
