namespace QCRunner.Infrastructure.Vendors.Hidex;

/// <summary>
/// The shake speeds the vendor firmware supports. The method designer works with the coarser
/// <see cref="Core.Operations.Reader.ShakeIntensity"/>; the reader maps between the two.
/// </summary>
public enum HidexShakeIntensity
{
    Normal = 0,
    Gentle = 1,
    Fast = 2,
    Intensive = 3
}
