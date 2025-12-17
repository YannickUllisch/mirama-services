
using System.Text.Json.Serialization;
using AccountService.Application.Common.Models;
using ErrorOr;
using MediatR;

namespace AccountService.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery : IRequest<ErrorOr<PaginatedList<UserResponse>>>
{
    [JsonPropertyName("pageSize")]
    public int? PageSize { get; init; } = null;

    [JsonPropertyName("pageNumber")]
    public int? PageNumber { get; init; } = null;
}