using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;
using Mirama.Modules.Identity.Contracts.Organizations;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Persistence;
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
    IClientsCommandRepository<Client, ClientId> repo,
    IMemberService memberService,
    IRequestContextProvider context)
    : IRequestHandler<CreateClientCommand, ErrorOr<ClientResponse>>
{
    public async Task<ErrorOr<ClientResponse>> HandleAsync(CreateClientCommand request, CancellationToken cancellationToken)
    {
        var member = await memberService.GetMemberByUserIdAsync(context.OrganizationId!.Value, context.UserId, cancellationToken);
        if (member is null)
            return Error.NotFound("Member.NotFound", "Member not found.");

        var client = Client.Create(new ClientDetails(
            request.Name,
            request.Type,
            request.Website,
            request.Industry,
            request.Notes,
            member.Id));

        repo.Add(client);

        return new ClientResponse(
            client.Id.Value,
            client.Name,
            client.Type.ToString(),
            client.Status.ToString(),
            client.Website,
            client.Industry);
    }
}
