using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Calculations.Common.Wells;

public static class WellCodeExtensions
{
    public const int RowCount = 8;
    public const int ColumnCount = 12;

    public static int RowIndex(this WellCode code)
    {
        EnsureDefined(code);
        return ((int)code - 1) / ColumnCount;
    }

    public static int ColumnIndex(this WellCode code)
    {
        EnsureDefined(code);
        return ((int)code - 1) % ColumnCount;
    }

    public static WellCode FromRowAndColumn(int rowIndex, int columnIndex)
    {
        if (rowIndex < 0 || rowIndex >= RowCount)
        {
            throw new ArgumentOutOfRangeException(nameof(rowIndex), rowIndex, "Row index must be between 0 and 7.");
        }

        if (columnIndex < 0 || columnIndex >= ColumnCount)
        {
            throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex, "Column index must be between 0 and 11.");
        }

        return (WellCode)(rowIndex * ColumnCount + columnIndex + 1);
    }

    public static WellCode FromIndex(int wellIndex)
    {
        return FromRowAndColumn(wellIndex / ColumnCount, wellIndex % ColumnCount);
    }

    private static void EnsureDefined(WellCode code)
    {
        if (code == WellCode.Undefined)
        {
            throw new ArgumentException("An undefined well code has no position on the plate.", nameof(code));
        }
    }
}
