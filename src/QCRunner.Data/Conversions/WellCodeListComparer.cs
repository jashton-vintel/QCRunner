using Microsoft.EntityFrameworkCore.ChangeTracking;
using QCRunner.Calculations.Common.Enumerations;

namespace QCRunner.Data.Conversions;

/// <summary>
/// Lets the change tracker detect edits inside a well list rather than only reference changes.
/// </summary>
public sealed class WellCodeListComparer : ValueComparer<List<WellCode>>
{
    public WellCodeListComparer()
        : base(
            (left, right) => (left ?? new List<WellCode>()).SequenceEqual(right ?? new List<WellCode>()),
            wells => wells.Aggregate(17, (hash, well) => HashCode.Combine(hash, well)),
            wells => wells.ToList())
    {
    }
}
