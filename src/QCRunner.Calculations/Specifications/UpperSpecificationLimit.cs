using QCRunner.Calculations.Units;

namespace QCRunner.Calculations.Specifications;

public sealed class UpperSpecificationLimit : SpecificationLimit
{
    private UpperSpecificationLimit(IDimensionedValue value, bool isClosed, int? decimals)
        : base(value, isClosed, decimals)
    {
    }

    public override string Symbol => IsClosed ? "<=" : "<";

    public static UpperSpecificationLimit Open(IDimensionedValue value, int? decimals = null) => new(value, isClosed: false, decimals);

    public static UpperSpecificationLimit Closed(IDimensionedValue value, int? decimals = null) => new(value, isClosed: true, decimals);

    protected override bool Compare(double measured, double limit) => IsClosed ? measured <= limit : measured < limit;
}
