using QCRunner.Core.Operations.Reader;
using QCRunner.Core.ReaderData;

namespace QCRunner.Core.Runners;

/// <summary>
/// The outcome of an absorbance or luminescence operation together with the readings it produced.
/// </summary>
public sealed class MeasurementOperationResult : OperationResult
{
    public MeasurementOperationResult(MeasurementOperationBase operation, IReadOnlyList<IReaderData> readings)
        : base(operation, OperationResultStatus.Successful)
    {
        ArgumentNullException.ThrowIfNull(readings);
        Readings = readings;
    }

    public IReadOnlyList<IReaderData> Readings { get; }
}
