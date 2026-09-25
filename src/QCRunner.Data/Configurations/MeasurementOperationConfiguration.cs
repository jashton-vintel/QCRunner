using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Reader;
using QCRunner.Data.Conversions;

namespace QCRunner.Data.Configurations;

public sealed class MeasurementOperationConfiguration : IEntityTypeConfiguration<MeasurementOperationBase>
{
    public void Configure(EntityTypeBuilder<MeasurementOperationBase> builder)
    {
        builder.Property(operation => operation.Cycles).HasColumnName("Cycles");

        builder.Property(operation => operation.Wells)
            .HasColumnName("Wells")
            .HasMaxLength(500)
            .HasConversion(new WellCodeListConverter(), new WellCodeListComparer());
    }
}
