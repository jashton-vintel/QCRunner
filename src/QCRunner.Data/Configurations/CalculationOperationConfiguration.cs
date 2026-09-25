using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Calculation;

namespace QCRunner.Data.Configurations;

public sealed class CalculationOperationConfiguration : IEntityTypeConfiguration<CalculationOperation>
{
    public void Configure(EntityTypeBuilder<CalculationOperation> builder)
    {
        builder.Property(operation => operation.CalculationType).HasColumnName("CalculationType");

        builder.HasMany(operation => operation.Parameters)
            .WithOne()
            .HasForeignKey(parameter => parameter.CalculationOperationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(operation => operation.Wells)
            .WithOne()
            .HasForeignKey(well => well.CalculationOperationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
