using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Reader;

namespace QCRunner.Data.Configurations;

public sealed class HeatToTemperatureOperationConfiguration : IEntityTypeConfiguration<HeatToTemperatureOperation>
{
    public void Configure(EntityTypeBuilder<HeatToTemperatureOperation> builder)
    {
        builder.Property(operation => operation.Temperature).HasColumnName("Temperature");
        builder.Property(operation => operation.Mode).HasColumnName("HeatMode");
    }
}
