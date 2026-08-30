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

        builder.Property(t => t.Name).IsRequired().HasMaxLength(100);
        builder.Property(t => t.IsActive).IsRequired();

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
    }
}
