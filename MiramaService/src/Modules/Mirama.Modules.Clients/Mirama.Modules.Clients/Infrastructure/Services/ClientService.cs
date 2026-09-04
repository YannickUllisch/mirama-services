using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Clients.Contracts;
using Mirama.Modules.Clients.Contracts.Dtos;
using Mirama.Modules.Clients.Domain.Enums;
using Mirama.Modules.Clients.Infrastructure.Persistence;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Clients.Infrastructure.Services;

// Implements IModuleService so it is picked up automatically by the IModuleService scan in
// ConfigureServices.AddInfrastructure (same convention Workspace uses for IViewStateService)
// and exposed to other modules purely through Contracts, per the synchronous cross-module
// pattern described in docs/mirama/modules/cross-module-communication.md.
internal sealed class ClientService(ClientsDbContext dbContext) : IClientService, IModuleService
{
    public async Task<List<ClientSummaryDto>> GetClientSummariesAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default)
    {
        // Global org query filter isn't reliable here - callers of a cross-module contract
        // may not be running inside an HTTP request carrying that ambient context, so the
        // organizationId is required explicitly and filters are bypassed instead.
        return await dbContext.Clients
            .AsNoTracking()
            .IgnoreQueryFilters()
            .Where(c => c.OrganizationId == organizationId && c.Status != ClientStatus.Archived)
            .OrderBy(c => c.Name)
            .Select(c => new ClientSummaryDto(
                c.Id.Value,
                c.Name,
                c.Type.ToString(),
                c.Status.ToString()))
            .ToListAsync(cancellationToken);
    }
}
