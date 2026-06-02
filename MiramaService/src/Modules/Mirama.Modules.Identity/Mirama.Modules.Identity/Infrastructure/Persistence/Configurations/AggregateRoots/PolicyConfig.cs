using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.AggregateRoots;

public class PolicyConfiguration : IEntityTypeConfiguration<Policy>
{
    public void Configure(EntityTypeBuilder<Policy> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                v => new PolicyId(v))
            .IsRequired();

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.IsManaged).IsRequired();
        builder.Property(p => p.Scope).IsRequired();

        // Nullable - null means system-level policy
        builder.Property(p => p.TenantId);
        builder.HasIndex(p => p.TenantId);

        builder.HasMany(p => p.Statements)
            .WithOne()
            .HasForeignKey(s => s.PolicyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
