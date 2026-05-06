using ErrorOr;
using Mirama.SharedKernel.Abstractions.Domain.Core;

namespace Mirama.Modules.Identity.Domain.Aggregates.Tenant;

public sealed class TenantSettings : Entity<TenantSettingsId>
{
    public Guid TenantId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public string Timezone { get; private set; } = "UTC";
    public string? BrandingColor { get; private set; }
    public string? LogoUrl { get; private set; }
    public bool ReceiveNotifications { get; private set; } = true;

    private TenantSettings() { }

    private TenantSettings(
        string name, 
        string timezone, 
        string? brandingColor, 
        string? logoUrl, 
        bool receiveNotifications)
    {
        this.Name = name;
        this.Timezone = timezone;
        this.BrandingColor = brandingColor;
        this.LogoUrl = logoUrl;
        this.ReceiveNotifications = receiveNotifications;
        this.IsActive = true;
    }

    internal static TenantSettings Create(TenantSettingsDetails details)
    {
        if (string.IsNullOrWhiteSpace(details.Name))
        {
            throw new ArgumentException("Tenant Name cannot be empty.", nameof(details));
        }

        return new TenantSettings(
            details.Name.Trim(),
            details.Timezone ?? "UTC",
            details.BrandingColor,
            details.LogoUrl,
            details.ReceiveNotifications
        );
    }

    public void Update(TenantSettingsDetails details)
    {
        if (string.IsNullOrWhiteSpace(details.Name))
        {
            throw new ArgumentException("Name is required.");
        }

        this.Name = details.Name.Trim();
        this.BrandingColor = details.BrandingColor;
        this.LogoUrl = details.LogoUrl;
        this.ReceiveNotifications = details.ReceiveNotifications;
        this.Timezone = details.Timezone;
    }

    public void SetActive(bool isActive)
    {
        IsActive = isActive;
    }
}
