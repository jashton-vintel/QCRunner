using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Wells;

/// <summary>
/// A well read by absorbance: one spectrum of absorbance against wavelength.
/// </summary>
public sealed class AbsorbanceWell : WellBase
{
    public AbsorbanceWell(string name, WellRole role, WellCode code)
        : this(name, role, code, double.NaN)
    {
    }

    public AbsorbanceWell(string name, WellRole role, WellCode code, double trueValue)
        : base(name, role, code, trueValue)
    {
        Spectrum = new AbsorbanceSpectrum();
    }

    public AbsorbanceSpectrum Spectrum { get; init; }

    public override ReaderDataType MeasurementType => ReaderDataType.Absorbance;
}
