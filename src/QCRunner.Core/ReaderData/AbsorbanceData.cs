using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Core.ReaderData;

/// <summary>
/// A full absorbance spectrum for one well: one value per wavelength step starting at
/// <see cref="StartingWavelength"/>.
/// </summary>
public sealed class AbsorbanceData : ReaderDataBase
{
    public AbsorbanceData(WellCode well, int cycle, DateTime measuredAt, TimeSpan elapsedTime, int startingWavelength, int wavelengthStep, IReadOnlyList<double> values)
        : base(well, cycle, measuredAt, elapsedTime)
    {
        ArgumentNullException.ThrowIfNull(values);

        if (wavelengthStep <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(wavelengthStep), wavelengthStep, "The wavelength step must be positive.");
        }

        StartingWavelength = startingWavelength;
        WavelengthStep = wavelengthStep;
        Values = values;
    }

    public override ReaderDataType DataType => ReaderDataType.Absorbance;

    public int StartingWavelength { get; }

    public int WavelengthStep { get; }

    public IReadOnlyList<double> Values { get; }

    public double WavelengthAt(int index) => StartingWavelength + index * WavelengthStep;
}
