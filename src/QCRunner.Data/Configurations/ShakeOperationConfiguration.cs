using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Reader;

namespace QCRunner.Data.Configurations;

public sealed class ShakeOperationConfiguration : IEntityTypeConfiguration<ShakeOperation>
{
    public void Configure(EntityTypeBuilder<ShakeOperation> builder)
    {
        builder.Property(operation => operation.Intensity).HasColumnName("ShakeIntensity");
        builder.Property(operation => operation.ShakeTime).HasColumnName("ShakeTime");
        builder.Property(operation => operation.ShakeCycles).HasColumnName("ShakeCycles");
    }
}
