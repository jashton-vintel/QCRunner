using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Calculation;

namespace QCRunner.Data.Configurations;

public sealed class CalculationWellConfiguration : IEntityTypeConfiguration<CalculationWell>
{
    public void Configure(EntityTypeBuilder<CalculationWell> builder)
    {
        builder.ToTable("CalculationWells");
        builder.HasKey(well => well.Id);
        builder.Property(well => well.Name).HasMaxLength(100).IsRequired();
    }
}
