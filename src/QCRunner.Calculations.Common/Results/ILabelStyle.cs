namespace QCRunner.Calculations.Common.Results;

public interface ILabelStyle
{
    bool Enabled { get; }

    /// <summary>A .NET numeric format string applied to axis labels, for example "0.000".</summary>
    string Format { get; }
}
