
using AccountService.Application.Infrastructure.Common.Interfaces;

namespace AccountService.Application.Infrastructure.Services;

internal class DesignTimeTenantContext: ITenantContextService
{
    public Guid OrganizationId => Guid.NewGuid();
}