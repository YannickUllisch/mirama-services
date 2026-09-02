using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Clients.Domain.Aggregates.Contract;

namespace Mirama.Modules.Clients.Infrastructure.Persistence.Configurations;

internal class ContractTermConfiguration : IEntityTypeConfiguration<ContractTerm>
{
    public void Configure(EntityTypeBuilder<ContractTerm> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(id => id.Value, v => new ContractTermId(v))
            .IsRequired();

        builder.Property(t => t.Type).IsRequired();
        builder.Property(t => t.Value).IsRequired().HasColumnType("numeric(18,2)");
        builder.Property(t => t.EffectiveFrom).IsRequired();
        builder.Property(t => t.EffectiveTo);
    }
}
