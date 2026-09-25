using QCRunner.Calculations.Common.Results;

namespace QCRunner.Calculations.ResultBuilding;

public sealed class ResultTable : IResultTable
{
    private readonly List<IReadOnlyList<string>> _rows = new();

    public ResultTable(string name, params string[] columns)
    {
        if (columns.Length == 0)
        {
            throw new ArgumentException("A result table needs at least one column.", nameof(columns));
        }

        Name = name;
        Columns = columns;
    }

    public string Name { get; }

    public IReadOnlyList<string> Columns { get; }

    public IReadOnlyList<IReadOnlyList<string>> Rows => _rows;

    public void AddRow(params string[] cells)
    {
        if (cells.Length != Columns.Count)
        {
            throw new ArgumentException($"Table '{Name}' has {Columns.Count} columns but the row supplied {cells.Length} cells.", nameof(cells));
        }

        _rows.Add(cells);
    }
}
