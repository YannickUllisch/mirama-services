
using System.Text.Json.Serialization;
using AccountService.Application.Common.Model;
using ErrorOr;
using MediatR;

namespace AccountService.Application.Features.Users.Queries.GetUsers;

public sealed record GetUsersQuery : IRequest<ErrorOr<PaginatedList<UserResponse>>>
{
    [JsonPropertyName("pageSize")]
    public int PageSize { get; init; } = 10;

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; init; } = 0;
}