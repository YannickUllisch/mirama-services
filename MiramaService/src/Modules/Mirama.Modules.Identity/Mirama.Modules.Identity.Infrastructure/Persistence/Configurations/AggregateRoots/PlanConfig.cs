using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Plan;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.AggregateRoots;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
    public void Configure(EntityTypeBuilder<Plan> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                v => new PlanId(v))
            .IsRequired();

        builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
        builder.Property(p => p.Description).HasMaxLength(500);
        builder.Property(p => p.Price).IsRequired();
        builder.Property(p => p.Interval).IsRequired().HasMaxLength(50);

        builder.Property(p => p.Features)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null)!)
            .HasColumnType("jsonb");
    }
}
