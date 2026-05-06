
using System.Text.Json.Serialization;
using ErrorOr;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mirama.Modules.Identity.Domain.Aggregates.User;
using Mirama.Modules.Identity.Infrastructure.Common.Interfaces;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;
using Mirama.SharedKernel.Extensions;
using Mirama.SharedKernel.Models;

namespace Mirama.Modules.Identity.Application.Features.V1.Users;

public class GetUsersController : TenantControllerBase
{
    [HttpGet("users")]
    public async Task<ActionResult<PaginatedList<UserResponse>>> Get([FromQuery] GetUsersQuery query)
    {
        var res = await this.Dispatcher.Send(query);
        return res.Match(Ok, Problem);
    }
}

public sealed record GetUsersQuery : IQuery<ErrorOr<PaginatedList<UserResponse>>>
{
    [JsonPropertyName("pageSize")]
    public int? PageSize { get; init; } = null;

    [JsonPropertyName("pageNumber")]
    public int? PageNumber { get; init; } = null;
}

internal class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator()
    {
        RuleFor(req => req.PageSize)
            .LessThanOrEqualTo(50);
    }
}

internal class GetUsersQueryHandler(IIdentityQueryRepository<User, UserId> userRepository) : IRequestHandler<GetUsersQuery, ErrorOr<PaginatedList<UserResponse>>>
{
    private readonly IIdentityQueryRepository<User, UserId> _userRepository = userRepository;

    public async Task<ErrorOr<PaginatedList<UserResponse>>> HandleAsync(GetUsersQuery request, CancellationToken cancellationToken)
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