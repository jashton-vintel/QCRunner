using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QCRunner.Core.Operations.Prompt;

namespace QCRunner.Data.Configurations;

public sealed class PromptOperationConfiguration : IEntityTypeConfiguration<PromptOperation>
{
    public void Configure(EntityTypeBuilder<PromptOperation> builder)
    {
        builder.Property(operation => operation.MessageTitle).HasColumnName("MessageTitle").HasMaxLength(200);
        builder.Property(operation => operation.MessageText).HasColumnName("MessageText").HasMaxLength(2000);
        builder.Property(operation => operation.Buttons).HasColumnName("PromptButtons");
    }
}
