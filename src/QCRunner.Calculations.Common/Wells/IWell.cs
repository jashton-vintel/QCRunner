using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Wells;

/// <summary>
/// A single well of reader data together with the flags a calculation sets while qualifying it.
/// </summary>
public interface IWell : ITestData
{
    /// <summary>The name the calculation refers to the well by, for example "Sample" or "Standard_1".</summary>
    string Name { get; set; }

    WellRole Role { get; set; }

    WellCode Code { get; set; }

    ReaderDataType MeasurementType { get; }

    /// <summary>The known value of a standard, or <see cref="double.NaN"/> for samples.</summary>
    double TrueValue { get; set; }

    bool IsSelected { get; set; }

    double Signal { get; set; }

    double CalculatedValue { get; set; }

    bool EnoughLiquid { get; set; }

    bool IsOutlier { get; set; }

    bool Scattering { get; set; }

    bool OtherProblems { get; set; }

    /// <summary>True when none of the exclusion flags have been raised against the well.</summary>
    bool IsValid { get; }
}
