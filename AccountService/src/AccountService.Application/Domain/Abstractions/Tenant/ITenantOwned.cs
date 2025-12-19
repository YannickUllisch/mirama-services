
namespace AccountService.Application.Domain.Abstractions.Tenant;

public interface ITenantOwned
{
    Guid TenantId { get; }

    void SetTenantId(Guid tenantId);
}