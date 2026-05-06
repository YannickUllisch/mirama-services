using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Subscription;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Infrastructure.Persistence.Configurations.AggregateRoots;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.AdminUserId)
            .HasConversion(
                id => id.Value,
                v => new UserId(v))
            .IsRequired();

        builder.HasIndex(t => t.AdminUserId).IsUnique();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Tenant>(t => t.AdminUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(t => t.SubscriptionId)
            .HasConversion(
                id => id != null ? id.Value : (Guid?)null,
                v => v.HasValue ? new SubscriptionId(v.Value) : null);

        builder.OwnsOne(t => t.Settings, s =>
        {
            s.Property(x => x.Id)
                .HasConversion(id => id.Value, v => new TenantSettingsId(v))
                .HasColumnName("SettingsId");
            s.Property(x => x.Name).HasColumnName("SettingsName");
            s.Property(x => x.IsActive).HasColumnName("SettingsIsActive");
            s.Property(x => x.Timezone).HasColumnName("SettingsTimezone");
            s.Property(x => x.BrandingColor).HasColumnName("BrandingColor");
            s.Property(x => x.LogoUrl).HasColumnName("LogoUrl");
            s.Property(x => x.ReceiveNotifications).HasColumnName("ReceiveNotifications");
        });
    }
}
