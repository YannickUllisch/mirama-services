using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Clients.Application.Features.V1.Clients.CreateClient;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Clients.Application.Features.V1.Clients.GetClients;

public class GetClientsController : OrganizationControllerBase
{
    [HttpGet("clients")]
    public async Task<IActionResult> Get(CancellationToken ct)
    {
        var result = await Dispatcher.Send(new GetClientsQuery(), ct);
        return result.Match(Ok, Problem);
    }
}

internal class GetClientsQueryHandler(
    IClientsQueryRepository<Client, ClientId> repo)
    : IRequestHandler<GetClientsQuery, ErrorOr<List<ClientResponse>>>
{
    public async Task<ErrorOr<List<ClientResponse>>> HandleAsync(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var clients = await repo.Query()
            .Select(c => new ClientResponse(
                c.Id.Value,
                c.Name,
                c.Type.ToString(),
                c.Status.ToString(),
                c.Website,
                c.Industry))
            .ToListAsync(cancellationToken);

        return clients;
    }
}
