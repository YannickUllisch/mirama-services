
using AccountService.Application.Domain.Organization.Invitation;
using AccountService.Application.Domain.Organization.Invitation.Valueobjects;
using AccountService.Application.Domain.Organization.ValueObjects;
using AccountService.Application.Domain.User.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Application.Infrastructure.Persistence.Configurations;

public class InvitationConfiguration : IEntityTypeConfiguration<Invitation>
{
    public void Configure(EntityTypeBuilder<Invitation> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(m => m.Id).HasConversion(
            invId => invId.Value,
            val => new InvitationId(val));

        builder.Property(m => m.InviterId).HasConversion(
            invId => invId.Value,
            val => new UserId(val));

        builder.Property(m => m.AcceptedBy).HasConversion(
            invId => invId != null ? invId.Value : (Guid?)null,
            val => val.HasValue ? new UserId(val.Value) : null);

        builder.Property(m => m.OrganizationId).HasConversion(
            orgId => orgId.Value,
            val => new OrganizationId(val));

    }
}