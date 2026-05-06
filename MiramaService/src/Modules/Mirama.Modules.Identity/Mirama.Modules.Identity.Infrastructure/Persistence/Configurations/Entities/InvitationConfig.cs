using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Organization.Invitation;
using Mirama.Modules.Identity.Domain.Aggregates.Role;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.Entities;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(i => i.Id);
        builder.HasIndex(i => i.OrganizationId);

        builder.Property(i => i.Id).HasConversion(
            invId => invId.Value,
            val => new InvitationId(val));

        builder.Property(i => i.IamRoleId).HasConversion(
            rid => rid.Value,
            val => new RoleId(val))
            .IsRequired();

        builder.Property(i => i.OrganizationId).HasConversion(
            orgId => orgId,
            val => val);

        builder.Property(i => i.InviterId).IsRequired();
        builder.Property(i => i.Email).IsRequired();
        builder.Property(i => i.Name).IsRequired();
        builder.Property(i => i.ExpiresAt).IsRequired();
    }
}
