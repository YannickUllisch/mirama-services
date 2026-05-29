using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Application.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Users.UpdateUser;

public class UpdateUserController : ApiControllerBase
{
    [HttpPut("users/{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateUserCommand command)
    {
        var cmd = command with { Id = id };
        var result = await this.Dispatcher.Send(cmd);

        return result.Match(Ok, Problem);
    }
}

internal class UpdateUserCommandHandler(IIdentityCommandRepository<User, UserId> repo) : IRequestHandler<UpdateUserCommand, ErrorOr<UserResponse>>
{
    private readonly IIdentityCommandRepository<User, UserId> _repo = repo;

    public async Task<ErrorOr<UserResponse>> HandleAsync(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repo.Query()
            .FirstOrDefaultAsync(u => u.Id.Value == request.Id, cancellationToken);

        if (user == null)
        {
            return Error.NotFound("User could not be found");
        }

        if (!Enum.TryParse<TenantRole>(request.Role, true, out var parsedRole))
        {
            return Error.Validation("Invalid role value.");
        }

        user.Update(new UserDetails(request.Name, request.Email, parsedRole, request.Image));

        return user.MapResponse();
    }
}
