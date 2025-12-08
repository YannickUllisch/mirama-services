
using AccountService.Domain.Organization;
using AccountService.Domain.Organization.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Infrastructure.Persistence.Configurations;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Ignore(e => e.DomainEvents);

        builder.Property(o => o.Id).HasConversion(
            orgId => orgId.Value,
            val => new OrganizationId(val));

        builder.OwnsOne(o => o.Address, a =>
        {
            a.Property(x => x.Street).HasColumnName("Street");
            a.Property(x => x.City).HasColumnName("City");
            a.Property(x => x.Country).HasColumnName("Country");
            a.Property(x => x.ZipCode).HasColumnName("ZipCode");
        });

        builder.HasMany(o => o.Invitations)
           .WithOne()
           .HasForeignKey(i => i.OrganizationId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(o => o.Members)
            .WithOne()
            .HasForeignKey(i => i.OrganizationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}