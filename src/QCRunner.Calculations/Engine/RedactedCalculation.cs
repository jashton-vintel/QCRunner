using QCRunner.Calculations.Common.Wells;

namespace QCRunner.Calculations.Engine;

/// <summary>
/// Stands in for the proprietary signal processing that has been removed from this
/// demonstration copy. Each member returns the plain mean of its input, so results still vary
/// with the data supplied without revealing anything about the real algorithms.
/// </summary>
internal static class RedactedCalculation
{
    public static double SampleValue(AbsorbanceSpectrum spectrum)
    {
        return spectrum.Count == 0 ? double.NaN : spectrum.Points.Values.Average();
    }

    public static double SampleValue(LuminescenceSeries counts)
    {
        return counts.Count == 0 ? double.NaN : counts.Points.Values.Average();
    }
}
