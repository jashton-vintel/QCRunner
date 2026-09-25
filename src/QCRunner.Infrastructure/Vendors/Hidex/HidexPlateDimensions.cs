namespace QCRunner.Infrastructure.Vendors.Hidex;

/// <summary>
/// Plate geometry in the orientation the reader tray uses. Dimensions are in millimetres.
/// </summary>
public sealed record HidexPlateDimensions(int Rows, int Columns, double RowOffset, double ColumnOffset, double WellSpacing, double WellDiameter);
