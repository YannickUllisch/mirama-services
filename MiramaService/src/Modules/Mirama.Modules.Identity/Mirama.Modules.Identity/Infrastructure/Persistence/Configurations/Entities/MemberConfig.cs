using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Member;
using Mirama.Modules.Identity.Domain.Aggregates.Role;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.Entities;

public class MemberConfiguration : IEntityTypeConfiguration<Member>
{
    public void Configure(EntityTypeBuilder<Member> builder)
    {
        builder.HasKey(m => m.Id);
        builder.HasIndex(m => m.OrganizationId);

        builder.Property(m => m.Id).HasConversion(
            uid => uid.Value,
            val => new MemberId(val));

        builder.Property(m => m.UserId).HasConversion(
            uid => uid != null ? uid.Value : (Guid?)null,
            val => val.HasValue ? new UserId(val.Value) : null);

        builder.Property(m => m.IamRoleIds)
            .HasConversion(
                v => JsonSerializer.Serialize(v.Select(r => r.Value).ToList(), (JsonSerializerOptions?)null),
                v => (JsonSerializer.Deserialize<List<Guid>>(v, (JsonSerializerOptions?)null) ?? new List<Guid>())
                                   .Select(g => new RoleId(g)).ToList())
            .HasColumnType("jsonb")
            .HasColumnName("IamRoleIds")
            .IsRequired();

        builder.Property(m => m.IamRoleIds).Metadata.SetValueComparer(
            new ValueComparer<List<RoleId>>(
                (a, b) => a != null && b != null && a.Select(r => r.Value).SequenceEqual(b.Select(r => r.Value)),
                v => v.Aggregate(0, (h, r) => HashCode.Combine(h, r.Value.GetHashCode())),
                v => v.ToList()));

        builder.Property(m => m.Name).IsRequired();
        builder.Property(m => m.Email).IsRequired();
    }
}
