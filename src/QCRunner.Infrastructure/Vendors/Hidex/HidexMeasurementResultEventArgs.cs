namespace QCRunner.Infrastructure.Vendors.Hidex;

public sealed class HidexMeasurementResultEventArgs : EventArgs
{
    public HidexMeasurementResultEventArgs(HidexMeasurementTechnology technology, int wellIndex, int cycle, TimeSpan elapsedTime, int startingWavelength, int wavelengthStep, IReadOnlyList<double> values)
    {
        Technology = technology;
        WellIndex = wellIndex;
        Cycle = cycle;
        ElapsedTime = elapsedTime;
        StartingWavelength = startingWavelength;
        WavelengthStep = wavelengthStep;
        Values = values;
    }

    public HidexMeasurementTechnology Technology { get; }

    /// <summary>Row-major index of the well on the plate.</summary>
    public int WellIndex { get; }

    public int Cycle { get; }

    public TimeSpan ElapsedTime { get; }

    public int StartingWavelength { get; }

    public int WavelengthStep { get; }

    /// <summary>A spectrum for absorbance; a single count value for luminescence.</summary>
    public IReadOnlyList<double> Values { get; }
}
