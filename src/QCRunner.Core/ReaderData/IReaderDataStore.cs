using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Core.Operations.Calculation;

namespace QCRunner.Core.ReaderData;

/// <summary>
/// The in-memory list of everything the reader has reported during a batch. Reader events
/// arrive on the instrument thread while calculations read on the runner thread, so every
/// member is safe to call concurrently.
/// </summary>
public interface IReaderDataStore
{
    int Count { get; }

    void Add(IReaderData data);

    IReadOnlyList<IReaderData> Snapshot();

    IReadOnlyList<IReaderData> GetWellData(WellCode well, ReaderDataType dataType);

    /// <summary>
    /// Gathers the readings for every well a calculation needs.
    /// </summary>
    /// <exception cref="InsufficientWellDataException">One or more wells have no readings of the requested type.</exception>
    IReadOnlyList<WellReaderData> SelectWellData(CalculationOperation operation);

    void Clear();
}
