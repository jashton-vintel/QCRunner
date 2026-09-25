using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Methods;

namespace QCRunner.Data.Configurations;

public sealed class PhaseConfiguration : IEntityTypeConfiguration<Phase>
{
    public void Configure(EntityTypeBuilder<Phase> builder)
    {
        builder.ToTable("Phases");
        builder.HasKey(phase => phase.Id);
        builder.Property(phase => phase.Name).HasMaxLength(200).IsRequired();
        builder.Property(phase => phase.Description).HasMaxLength(1000);
        builder.HasIndex(phase => new { phase.MethodId, phase.Index }).IsUnique();

        builder.HasMany(phase => phase.OperationGroups)
            .WithOne(group => group.Phase)
            .HasForeignKey(group => group.PhaseId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
