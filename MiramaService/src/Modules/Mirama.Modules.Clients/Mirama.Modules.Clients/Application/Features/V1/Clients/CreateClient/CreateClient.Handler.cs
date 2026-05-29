using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Clients.Application.Features.V1.Clients.CreateClient;

public class CreateClientController : OrganizationControllerBase
{
    [HttpPost("/clients")]
    public async Task<IActionResult> Create([FromBody] CreateClientCommand command, CancellationToken ct)
    {
        var result = await Dispatcher.Send(command, ct);
        return result.Match(r => CreatedAtAction(nameof(Create), new { id = r.ClientId }, r), Problem);
    }
}

internal class CreateClientCommandHandler(
    IClientsCommandRepository<Client, ClientId> repo)
    : IRequestHandler<CreateClientCommand, ErrorOr<ClientResponse>>
{
    public Task<ErrorOr<ClientResponse>> HandleAsync(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var client = Client.Create(new ClientDetails(
            request.Name,
            request.Type,
            request.Website,
            request.Industry,
            request.Notes));

        repo.Add(client);

        return Task.FromResult<ErrorOr<ClientResponse>>(new ClientResponse(
            client.Id.Value,
            client.Name,
            client.Type.ToString(),
            client.Status.ToString(),
            client.Website,
            client.Industry));
    }
}
