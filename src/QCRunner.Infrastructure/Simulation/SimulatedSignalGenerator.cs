namespace QCRunner.Infrastructure.Simulation;

/// <summary>
/// Produces plausible looking reader signals for demonstrations. Nothing here resembles a real
/// assay; the shapes only need to plot well and pass the placeholder calculations.
/// </summary>
public sealed class SimulatedSignalGenerator
{
    private readonly object _syncRoot = new();
    private readonly Random _random;

    public SimulatedSignalGenerator(int seed)
    {
        _random = new Random(seed);
    }

    /// <summary>
    /// A smooth peak on a low baseline. The peak position depends on the well so different
    /// wells are distinguishable on a chart.
    /// </summary>
    public IReadOnlyList<double> AbsorbanceSpectrum(int wellIndex, int pointCount)
    {
        double centre = pointCount * (0.3 + 0.4 * (wellIndex % 7) / 7.0);
        double width = pointCount / 8.0;
        var values = new double[pointCount];

        lock (_syncRoot)
        {
            for (int index = 0; index < pointCount; index++)
            {
                double peak = 0.5 * Math.Exp(-Math.Pow((index - centre) / width, 2));
                double noise = (_random.NextDouble() - 0.5) * 0.004;

                values[index] = 0.05 + peak + noise;
            }
        }

        return values;
    }

    /// <summary>Counts that fall away slowly from cycle to cycle, like a decaying source.</summary>
    public double LuminescenceCounts(int wellIndex, int cycle)
    {
        double initial = 4000 + 250 * (wellIndex % 5);

        lock (_syncRoot)
        {
            double noise = (_random.NextDouble() - 0.5) * 40;

            return Math.Round(initial * Math.Pow(0.97, cycle - 1) + noise);
        }
    }
}
