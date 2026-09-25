namespace QCRunner.Core.Labware;

/// <summary>
/// A microplate or other vessel the instruments work with. Dimensions are in millimetres and
/// describe the geometry the robot and reader need to locate each well.
/// </summary>
public interface ILabware
{
    string Name { get; }

    int RowCount { get; }

    int ColumnCount { get; }

    int WellCount { get; }

    /// <summary>Distance from the plate edge to the centre of the first row.</summary>
    double RowOffset { get; }

    /// <summary>Distance from the plate edge to the centre of the first column.</summary>
    double ColumnOffset { get; }

    /// <summary>Centre-to-centre distance between neighbouring wells.</summary>
    double WellSpacing { get; }

    double WellDiameter { get; }

    double WellDepth { get; }

    double Height { get; }
}
