
namespace AccountService.Application.Domain.Abstractions.Core;

public interface ITenantOwned
{
    Guid TenantId { get; }

    void SetTenantId(Guid tenantId);
}