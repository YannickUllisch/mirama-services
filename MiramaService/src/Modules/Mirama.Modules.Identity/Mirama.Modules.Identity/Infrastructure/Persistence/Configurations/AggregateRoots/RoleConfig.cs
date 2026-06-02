using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Aggregates.Role;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.AggregateRoots;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasConversion(
                id => id.Value,
                v => new RoleId(v))
            .IsRequired();

        builder.Property(r => r.Name).IsRequired().HasMaxLength(100);
        builder.Property(r => r.Description).HasMaxLength(500);
        builder.Property(r => r.Scope).IsRequired();

        // Nullable - null means system-level role
        builder.Property(r => r.TenantId);
        builder.HasIndex(r => r.TenantId);

        // Many-to-many with Policy stored as JSON array of PolicyId GUIDs
        builder.Property(r => r.Policies)
            .HasConversion(
                v => JsonSerializer.Serialize(v.Select(p => p.Value).ToList(), (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null)!
                                   .Select(g => new PolicyId(g)).ToList())
            .HasColumnType("jsonb")
            .HasColumnName("PolicyIds")
            .Metadata.SetValueComparer(new ValueComparer<List<PolicyId>>(
                (a, b) => a != null && b != null && a.Select(p => p.Value).SequenceEqual(b.Select(p => p.Value)),
                v => v.Aggregate(0, (h, p) => HashCode.Combine(h, p.Value.GetHashCode())),
                v => v.ToList()));
    }
}
