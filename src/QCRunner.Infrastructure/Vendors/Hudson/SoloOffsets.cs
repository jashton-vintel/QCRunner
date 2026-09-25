namespace QCRunner.Infrastructure.Vendors.Hudson;

/// <summary>Shifts from the nominal well position, in millimetres.</summary>
public sealed record SoloOffsets(double X, double Y, double Z)
{
    public static SoloOffsets None => new(0, 0, 0);
}
