using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Clients.Domain.Aggregates.Client;
using Mirama.Modules.Clients.Domain.Entities.Contact;
using Mirama.Modules.Clients.Infrastructure.Persistence.Repositories;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Clients.Application.Features.V1.Contacts.AddContact;

[ApiController]
[Route("api/v{version:apiVersion}/clients/{clientId:guid}/contacts")]
public class AddContactController : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromRoute] Guid clientId,
        [FromBody] AddContactCommand command,
        CancellationToken ct)
    {
        var cmd = command with { ClientId = clientId };
        var result = await Dispatcher.Send(cmd, ct);
        return result.Match(Ok, Problem);
    }
}

internal class AddContactCommandHandler(
    IClientsCommandRepository<Client, ClientId> commandRepo,
    IClientsQueryRepository<Client, ClientId> queryRepo)
    : IRequestHandler<AddContactCommand, ErrorOr<ContactResponse>>
{
    public async Task<ErrorOr<ContactResponse>> HandleAsync(AddContactCommand request, CancellationToken cancellationToken)
    {
        var client = await queryRepo.Query()
            .FirstOrDefaultAsync(c => c.Id.Value == request.ClientId, cancellationToken);

        if (client is null)
            return Error.NotFound("Client.NotFound", "Client not found.");

        var contact = client.AddContact(new ContactDetails(
            request.FirstName,
            request.LastName,
            request.Email,
            request.Phone,
            request.JobTitle,
            request.IsPrimary));

        commandRepo.Update(client);

        return new ContactResponse(
            contact.Id.Value,
            client.Id.Value,
            contact.FullName,
            contact.Email,
            contact.Phone,
            contact.JobTitle,
            contact.IsPrimary);
    }
}
