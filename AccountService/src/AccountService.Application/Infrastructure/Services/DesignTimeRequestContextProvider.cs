
using AccountService.Application.Infrastructure.Common.Interfaces;

namespace AccountService.Application.Infrastructure.Services;

internal class DesignTimeRequestContextProvider : IRequestContextProvider
{
    Guid IRequestContextProvider.UserId => Guid.NewGuid();

    Guid IRequestContextProvider.TenantId => Guid.NewGuid();

    Guid? IRequestContextProvider.OrganizationId => Guid.NewGuid();
}