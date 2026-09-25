using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Reader;

namespace QCRunner.Data.Configurations;

public sealed class LuminescenceOperationConfiguration : IEntityTypeConfiguration<LuminescenceOperation>
{
    public void Configure(EntityTypeBuilder<LuminescenceOperation> builder)
    {
        builder.Property(operation => operation.CountingTime).HasColumnName("CountingTime");
    }
}
