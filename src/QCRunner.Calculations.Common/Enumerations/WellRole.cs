namespace QCRunner.Calculations.Common.Enumerations;

/// <summary>
/// The part a well plays in a calculation.
/// </summary>
public enum WellRole
{
    Undefined = 0,
    Sample = 1,
    Standard = 2,
    Background = 3,
    SampleBackground = 4
}
