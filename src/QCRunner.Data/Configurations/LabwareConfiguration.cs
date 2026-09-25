using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace QCRunner.Data.Configurations;

public sealed class LabwareConfiguration : IEntityTypeConfiguration<Core.Labware.Labware>
{
    public void Configure(EntityTypeBuilder<Core.Labware.Labware> builder)
    {
        builder.ToTable("Labware");
        builder.HasKey(labware => labware.Id);
        builder.Property(labware => labware.Name).HasMaxLength(200).IsRequired();
        builder.Property(labware => labware.Description).HasMaxLength(1000);
        builder.Property(labware => labware.Manufacturer).HasMaxLength(200);
        builder.Ignore(labware => labware.WellCount);
    }
}
