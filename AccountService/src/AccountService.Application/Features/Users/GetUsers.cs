
using System.Text.Json.Serialization;
using AccountService.Application.Common;
using AccountService.Application.Common.Extensions;
using AccountService.Application.Common.Interfaces;
using AccountService.Application.Common.Models;
using AccountService.Application.Domain.Aggregates.User;
using ErrorOr;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Users;

public class GetUsersController : ApiControllerBase
{
    [HttpGet("user")]
    public async Task<ActionResult<PaginatedList<UserResponse>>> Get([FromQuery] GetUsersQuery query)
    {
        var res = await Mediator.Send(query);
        return res.Match(Ok, Problem);
    }
}

public sealed record GetUsersQuery : IRequest<ErrorOr<PaginatedList<UserResponse>>>
{
    [JsonPropertyName("pageSize")]
    public int? PageSize { get; init; } = null;

    [JsonPropertyName("pageNumber")]
    public int? PageNumber { get; init; } = null;
}

internal class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator(IGlobalRoleProvider roleProvider)
    {
        // RuleFor(req => req.PageSize)
        //     .LessThanOrEqualTo(50);
    }
}

internal class GetUsersQueryHandler(IReadRepository<User, UserId> userRepository) : IRequestHandler<GetUsersQuery, ErrorOr<PaginatedList<UserResponse>>>
{
    private readonly IReadRepository<User, UserId> _userRepository = userRepository;

    public async Task<ErrorOr<PaginatedList<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {   
        var query = _userRepository.Query().OrderBy(user => user.Name).Select(u => u.MapResponse().Value);

        if (request.PageNumber != null && request.PageSize != null)
        {
            return await query.PaginatedListAsync(request.PageNumber.Value, request.PageSize.Value);
        }

        var users = await query.ToListAsync(cancellationToken);
        var paginatedList = new PaginatedList<UserResponse>(users, users.Count, 1, users.Count > 0 ? users.Count : 1);
        return paginatedList;
    }
}