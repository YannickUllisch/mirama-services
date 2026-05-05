

using MediatR;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.SharedKernel.Models;
using Mirama.Modules.Identity.Infrastructure.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Users.UpdateUser;

public class UpdateUserController : ApiControllerBase
{
    [HttpPut("users/{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateUserCommand command)
    {
        var cmd = command with { Id = id };
        var result = await Mediator.Send(command);

        return result.Match(Ok, Problem);
    }
}

internal class UpdateUserCommandHandler(IIdentityCommandRepository<User, UserId> repo) : IRequestHandler<UpdateUserCommand, ErrorOr<UserResponse>>
{
    private readonly IIdentityCommandRepository<User, UserId> _repo = repo;

    public async Task<ErrorOr<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repo.Query()
            .FirstOrDefaultAsync(u => u.Id.Value == request.Id, cancellationToken);

        if (user == null)
        {
            return Error.NotFound("User could not be found");
        }

        if (!Enum.TryParse<GlobalRole>(request.Role, true, out var parsedRole))
        {
            return Error.Validation("Invalid role value.");
        }

        user.Update(
            request.Name,
            request.Email,
            parsedRole,
            request.ContactEmail,
            request.ContactPhoneNumber,
            request.Image
        );

        return user.MapResponse();
    }
}