using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Clients.Domain.Aggregates.Contract;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Configurations;

internal class ContractConfiguration : IEntityTypeConfiguration<Contract>
{
    public void Configure(EntityTypeBuilder<Contract> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .HasConversion(id => id.Value, v => new ContractId(v))
            .IsRequired();

        builder.Property(c => c.PartyType).IsRequired();
        builder.Property(c => c.PartyId).IsRequired();
        builder.Property(c => c.Title).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Status).IsRequired();
        builder.Property(c => c.EffectiveFrom).IsRequired();
        builder.Property(c => c.EffectiveTo);
        builder.Property(c => c.OrganizationId).IsRequired();

        builder.HasIndex(c => c.OrganizationId);
        builder.HasIndex(c => new { c.PartyType, c.PartyId });

        builder.HasMany(c => c.Terms)
            .WithOne()
            .HasForeignKey("ContractId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
