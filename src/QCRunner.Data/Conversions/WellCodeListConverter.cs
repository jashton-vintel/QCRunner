using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Data.Conversions;

/// <summary>
/// Stores a well mask as a comma separated list of codes ("A1,A2,B1") rather than a child table.
/// </summary>
public sealed class WellCodeListConverter : ValueConverter<List<WellCode>, string>
{
    public WellCodeListConverter()
        : base(
            wells => string.Join(",", wells.Select(well => well.ToString())),
            text => Parse(text))
    {
    }

    private static List<WellCode> Parse(string text)
    {
        return text
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(code => Enum.Parse<WellCode>(code, ignoreCase: true))
            .ToList();
    }
}
