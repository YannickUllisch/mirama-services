

using AccountService.Application.Domain.Aggregates.Tenant;
using AccountService.Application.Domain.Aggregates.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountService.Application.Infrastructure.Persistence.Configurations.AggregateRoots;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(t => t.AdminUserId)
        .HasConversion(
            id => id.Value,
            v => new UserId(v))
        .IsRequired();

        builder.HasIndex(t => t.AdminUserId)
            .IsUnique();

        builder.HasOne<User>()
            .WithOne()
            .HasForeignKey<Tenant>(t => t.AdminUserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.OwnsOne(a => a.Settings, s =>
        {
            s.Property(x => x.BrandingColor).HasColumnName("BrandingColor");
            s.Property(x => x.LogoUrl).HasColumnName("LogoUrl");
            s.Property(x => x.ReceiveNotifications).HasColumnName("ReceiveNotifications");
        });

        builder.OwnsOne(t => t.BillingPlan, bp =>
        {
            bp.Property(x => x.Name).HasColumnName("BillingPlanName");
            bp.Property(x => x.QuotaTeams).HasColumnName("BillingPlanQuotaTeams");
            bp.Property(x => x.QuotaUsers).HasColumnName("BillingPlanQuotaUsers");
            bp.Property(x => x.QuotaOrganizations).HasColumnName("BillingPlanQuotaOrganizations");
        });
    }
}