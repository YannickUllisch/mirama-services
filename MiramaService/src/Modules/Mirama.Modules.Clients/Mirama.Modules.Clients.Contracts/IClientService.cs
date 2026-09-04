using Mirama.Modules.Clients.Contracts.Dtos;

namespace Mirama.Modules.Clients.Contracts;

/// <summary>
/// Public surface other modules use to read client data without depending on the Clients
/// module's internals. See Mirama.Modules.Clients.Infrastructure.Services.ClientService for
/// the implementation.
/// </summary>
public interface IClientService
{
    /// <summary>
    /// All non-archived clients for an organization, ordered by name. Used by Workspace to
    /// resolve the sidebar's "Your clients" group live at read time rather than caching
    /// client names/ids anywhere - a rename or a newly created client is reflected
    /// immediately with no invalidation needed on the caller's side.
    /// </summary>
    Task<List<ClientSummaryDto>> GetClientSummariesAsync(
        Guid organizationId,
        CancellationToken cancellationToken = default);
}
