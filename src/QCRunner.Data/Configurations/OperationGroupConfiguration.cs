using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Methods;

namespace QCRunner.Data.Configurations;

public sealed class OperationGroupConfiguration : IEntityTypeConfiguration<OperationGroup>
{
    public void Configure(EntityTypeBuilder<OperationGroup> builder)
    {
        builder.ToTable("OperationGroups");
        builder.HasKey(group => group.Id);
        builder.Property(group => group.Name).HasMaxLength(200).IsRequired();
        builder.Property(group => group.Description).HasMaxLength(1000);
        builder.HasIndex(group => new { group.PhaseId, group.Index }).IsUnique();

        builder.HasMany(group => group.Operations)
            .WithOne(operation => operation.OperationGroup)
            .HasForeignKey(operation => operation.OperationGroupId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
