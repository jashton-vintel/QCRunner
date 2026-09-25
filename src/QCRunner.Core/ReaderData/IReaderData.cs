using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Core.ReaderData;

/// <summary>
/// One reading of one well. Measurement operations with several cycles produce one instance
/// per well per cycle.
/// </summary>
public interface IReaderData
{
    WellCode Well { get; }

    ReaderDataType DataType { get; }

    int Cycle { get; }

    DateTime MeasuredAt { get; }

    /// <summary>Time since the measurement operation started.</summary>
    TimeSpan ElapsedTime { get; }
}
