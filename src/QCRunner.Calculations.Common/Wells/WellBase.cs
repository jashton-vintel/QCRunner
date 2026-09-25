using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Wells;

public abstract class WellBase : IWell
{
    protected WellBase(string name, WellRole role, WellCode code, double trueValue)
    {
        Name = name;
        Role = role;
        Code = code;
        TrueValue = trueValue;
        EnoughLiquid = true;
        CalculatedValue = double.NaN;
        Signal = double.NaN;
    }

    public string Name { get; set; }

    public WellRole Role { get; set; }

    public WellCode Code { get; set; }

    public abstract ReaderDataType MeasurementType { get; }

    public double TrueValue { get; set; }

    public bool IsSelected { get; set; }

    public double Signal { get; set; }

    public double CalculatedValue { get; set; }

    public bool EnoughLiquid { get; set; }

    public bool IsOutlier { get; set; }

    public bool Scattering { get; set; }

    public bool OtherProblems { get; set; }

    public bool IsValid => EnoughLiquid && !IsOutlier && !Scattering && !OtherProblems;

    public override string ToString() => $"{Name} ({Code}, {Role})";
}
