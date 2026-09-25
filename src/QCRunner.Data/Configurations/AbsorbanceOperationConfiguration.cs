using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Reader;

namespace QCRunner.Data.Configurations;

public sealed class AbsorbanceOperationConfiguration : IEntityTypeConfiguration<AbsorbanceOperation>
{
    public void Configure(EntityTypeBuilder<AbsorbanceOperation> builder)
    {
        builder.Property(operation => operation.Flashes).HasColumnName("Flashes");
        builder.Property(operation => operation.Delay).HasColumnName("Delay");
    }
}
