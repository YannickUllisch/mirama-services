using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Mirama.Modules.Identity.Domain.Aggregates.Plan;
using Mirama.Modules.Identity.Domain.Aggregates.Tenant.Subscription;
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

        builder.OwnsOne(t => t.Subscription, sub =>
        {
            sub.Property(s => s.Id)
                .HasConversion(id => id.Value, v => new SubscriptionId(v))
                .HasColumnName("SubscriptionId");

            sub.Property(s => s.PlanId)
                .HasConversion(id => id.Value, v => new PlanId(v))
                .HasColumnName("SubscriptionPlanId")
                .IsRequired();

            sub.HasOne<Plan>()
                .WithMany()
                .HasForeignKey("SubscriptionPlanId")
                .OnDelete(DeleteBehavior.Restrict);

            sub.Property(s => s.Status)
                .HasColumnName("SubscriptionStatus")
                .IsRequired();

            sub.Property(s => s.StripeSubscriptionId)
                .HasColumnName("StripeSubscriptionId")
                .HasMaxLength(200);

            sub.HasIndex("StripeSubscriptionId")
                .IsUnique()
                .HasFilter("\"StripeSubscriptionId\" IS NOT NULL");

            sub.Property(s => s.PeriodStart)
                .HasColumnName("SubscriptionPeriodStart")
                .IsRequired();

            sub.Property(s => s.PeriodEnd)
                .HasColumnName("SubscriptionPeriodEnd")
                .IsRequired();

            sub.Property(s => s.CancelAtPeriodEnd)
                .HasColumnName("SubscriptionCancelAtPeriodEnd")
                .IsRequired();
        });

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
