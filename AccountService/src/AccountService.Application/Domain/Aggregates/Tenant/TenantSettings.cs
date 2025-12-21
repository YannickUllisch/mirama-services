
using AccountService.Application.Domain.Abstractions.Core;
using ErrorOr;

namespace AccountService.Application.Domain.Aggregates.Tenant;

public class TenantSettings : ValueObject
{
    public string? BrandingColor { get; } = null;
    public string? LogoUrl { get; } = null;
    public bool ReceiveNotifications { get; } = true;

    private TenantSettings() { }

    public TenantSettings(
        string? brandingColor = null,
        string? logoUrl = null,
        bool receiveNotifications = true)
    {
        BrandingColor = brandingColor;
        LogoUrl = logoUrl;
        ReceiveNotifications = receiveNotifications;
    }

    public static ErrorOr<TenantSettings> Create(string? brandingColor, string? logoUrl, bool receiveNotifications)
    {
        return new TenantSettings(brandingColor, logoUrl, receiveNotifications);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return BrandingColor ?? string.Empty;
        yield return LogoUrl ?? string.Empty;
        yield return ReceiveNotifications;
    }
}