


using AccountService.Application.Common;
using AccountService.Application.Features.Users.Commands.UpdateUser;
using AccountService.Application.Features.Users.Queries.GetUsers;
using Microsoft.AspNetCore.Mvc;

namespace AccountService.Application.Features.Users;

public class UsersController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<UserResponse>> Get([FromQuery] GetUsersQuery query)
    {
        var res = await Mediator.Send(query);
        return res.Match(Ok, Problem);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, UpdateUserCommand command)
    {
        var cmd = command with { Id = id };
        var result = await Mediator.Send(command);

        return result.Match(
            val => Ok(val),
            err => Problem(err)
        );
    }
}