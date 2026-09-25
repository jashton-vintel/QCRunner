namespace QCRunner.Core.Operations.Reader;

/// <summary>
/// A reader operation that acts on a plate loaded in the reader tray.
/// </summary>
public abstract class PlateOperationBase : ReaderOperationBase
{
    public int LabwareId { get; set; }

    public virtual Core.Labware.Labware Plate { get; set; } = null!;
}
