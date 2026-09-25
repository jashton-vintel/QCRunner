using QCRunner.Calculations.Common.Enumerations;
using QCRunner.Core.Operations.Calculation;

namespace QCRunner.Core.ReaderData;

public sealed class ReaderDataStore : IReaderDataStore
{
    private readonly object _syncRoot = new();
    private readonly List<IReaderData> _readings = new();

    public int Count
    {
        get
        {
            lock (_syncRoot)
            {
                return _readings.Count;
            }
        }
    }

    public void Add(IReaderData data)
    {
        ArgumentNullException.ThrowIfNull(data);

        lock (_syncRoot)
        {
            _readings.Add(data);
        }
    }

    public IReadOnlyList<IReaderData> Snapshot()
    {
        lock (_syncRoot)
        {
            return _readings.ToArray();
        }
    }

    public IReadOnlyList<IReaderData> GetWellData(WellCode well, ReaderDataType dataType)
    {
        lock (_syncRoot)
        {
            return _readings
                .Where(reading => reading.Well == well && reading.DataType == dataType)
                .OrderBy(reading => reading.MeasuredAt)
                .ToArray();
        }
    }

    public IReadOnlyList<WellReaderData> SelectWellData(CalculationOperation operation)
    {
        ArgumentNullException.ThrowIfNull(operation);

        var selected = new List<WellReaderData>();
        var missing = new List<CalculationWell>();

        foreach (CalculationWell well in operation.Wells)
        {
            IReadOnlyList<IReaderData> readings = GetWellData(well.Code, well.DataType);

            if (readings.Count == 0)
            {
                missing.Add(well);
            }
            else
            {
                selected.Add(new WellReaderData(well, readings));
            }
        }

        if (missing.Count > 0)
        {
            throw new InsufficientWellDataException(operation.CalculationType, missing);
        }

        return selected;
    }

    public void Clear()
    {
        lock (_syncRoot)
        {
            _readings.Clear();
        }
    }
}
