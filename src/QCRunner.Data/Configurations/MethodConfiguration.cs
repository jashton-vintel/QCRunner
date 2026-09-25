using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Methods;

namespace QCRunner.Data.Configurations;

public sealed class MethodConfiguration : IEntityTypeConfiguration<Method>
{
    public void Configure(EntityTypeBuilder<Method> builder)
    {
        builder.ToTable("Methods");
        builder.HasKey(method => method.Id);
        builder.Property(method => method.Code).HasMaxLength(20).IsRequired();
        builder.Property(method => method.Name).HasMaxLength(200).IsRequired();
        builder.Property(method => method.Description).HasMaxLength(1000);
        builder.Property(method => method.Version).HasMaxLength(20);
        builder.HasIndex(method => method.Code).IsUnique();

        builder.HasMany(method => method.Phases)
            .WithOne(phase => phase.Method)
            .HasForeignKey(phase => phase.MethodId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
