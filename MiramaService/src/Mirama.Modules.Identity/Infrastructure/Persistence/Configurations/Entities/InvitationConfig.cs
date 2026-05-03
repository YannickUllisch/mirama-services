
using Mirama.Domain.Aggregates.Organization;
using Mirama.Domain.Aggregates.Organization.Invitation;
using Mirama.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.Entities;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(i => i.Id);

        builder.HasIndex(i => i.OrganizationId);
        builder.Property(m => m.InviterId).IsRequired();

        builder.Property(i => i.OrganizationId).HasConversion(
                orgId => orgId.Value,
                val => new OrganizationId(val));

        builder.Property(m => m.Id).HasConversion(
            invId => invId.Value,
            val => new InvitationId(val));

        builder.Property(m => m.InviterId).HasConversion(
            invId => invId.Value,
            val => new UserId(val));

        builder.Property(m => m.AcceptedBy).HasConversion(
            invId => invId != null ? invId.Value : (Guid?)null,
            val => val.HasValue ? new UserId(val.Value) : null);
    }
}