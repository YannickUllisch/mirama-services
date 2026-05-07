using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.Entities;

public class PolicyStatementConfiguration : IEntityTypeConfiguration<PolicyStatement>
{
    public void Configure(EntityTypeBuilder<PolicyStatement> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                v => new PolicyStatementId(v))
            .IsRequired();

        builder.Property(s => s.PolicyId)
            .HasConversion(
                id => id.Value,
                v => new PolicyId(v))
            .IsRequired();

        builder.Property(s => s.Effect).IsRequired();
        builder.Property(s => s.Action).IsRequired().HasMaxLength(100);
        builder.Property(s => s.Resource).IsRequired().HasMaxLength(200);

        builder.HasIndex(s => new { s.PolicyId, s.Action, s.Resource }).IsUnique();
    }
}
