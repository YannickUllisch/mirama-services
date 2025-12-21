
using AccountService.Application.Domain.Aggregates.Organization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Application.Infrastructure.Persistence.Configurations.AggregateRoots;

public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasIndex(o => o.TenantId);

        builder.Property(o => o.Id).HasConversion(
            id => id.Value,
            v => new OrganizationId(v));
        builder.Property(o => o.TenantId).IsRequired();

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