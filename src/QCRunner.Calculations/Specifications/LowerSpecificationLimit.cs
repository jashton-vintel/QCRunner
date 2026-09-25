using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Specifications;

public sealed class LowerSpecificationLimit : SpecificationLimit
{
    private LowerSpecificationLimit(IDimensionedValue value, bool isClosed, int? decimals)
        : base(value, isClosed, decimals)
    {
    }

    public override string Symbol => IsClosed ? ">=" : ">";

    public static LowerSpecificationLimit Open(IDimensionedValue value, int? decimals = null) => new(value, isClosed: false, decimals);

    public static LowerSpecificationLimit Closed(IDimensionedValue value, int? decimals = null) => new(value, isClosed: true, decimals);

    protected override bool Compare(double measured, double limit) => IsClosed ? measured >= limit : measured > limit;
}
