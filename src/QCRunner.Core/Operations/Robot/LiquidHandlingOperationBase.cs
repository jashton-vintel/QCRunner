namespace QCRunner.Core.Operations.Robot;

/// <summary>
/// Shared settings for moving liquid to or from a well. Offsets are in millimetres from the
/// nominal well position; <see cref="Position"/> is the deck position holding the plate.
/// </summary>
public abstract class LiquidHandlingOperationBase : RobotOperationBase
{
    public int LabwareId { get; set; }

    public virtual Core.Labware.Labware Plate { get; set; } = null!;

    /// <summary>Volume in microlitres.</summary>
    public double Volume { get; set; }

    /// <summary>Named deck point, or a well reference such as "A1" when the plate is the target.</summary>
    public string Point { get; set; } = string.Empty;

    public int Position { get; set; }

    public int MixCycles { get; set; }

    public double MixVolume { get; set; }

    public double XOffset { get; set; }

    public double YOffset { get; set; }

    public double ZOffset { get; set; }
}
