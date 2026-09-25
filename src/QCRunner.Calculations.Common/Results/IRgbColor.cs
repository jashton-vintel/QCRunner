namespace QCRunner.Calculations.Common.Results;

public interface IRgbColor
{
    byte A { get; }

    byte R { get; }

    byte G { get; }

    byte B { get; }

    /// <summary>True when the colour has not been set and the viewer should pick one.</summary>
    bool IsEmpty { get; }
}
