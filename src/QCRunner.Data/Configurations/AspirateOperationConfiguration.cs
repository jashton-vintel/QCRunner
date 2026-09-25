using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Robot;

namespace QCRunner.Data.Configurations;

public sealed class AspirateOperationConfiguration : IEntityTypeConfiguration<AspirateOperation>
{
    public void Configure(EntityTypeBuilder<AspirateOperation> builder)
    {
        builder.Property(operation => operation.AspirateHeight).HasColumnName("AspirateHeight");
    }
}
