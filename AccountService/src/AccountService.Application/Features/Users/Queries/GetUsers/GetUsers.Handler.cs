
using AccountService.Application.Common.Extensions;
using AccountService.Application.Common.Models;
using AccountService.Application.Infrastructure.Persistence;
using ErrorOr;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Application.Features.Users.Queries.GetUsers;

internal class GetUsersQueryHandler(ApplicationDbContext context) : IRequestHandler<GetUsersQuery, ErrorOr<PaginatedList<UserResponse>>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ErrorOr<PaginatedList<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = _context.User
            .OrderBy(user => user.Name)
            .Select(u => u.MapResponse().Value);

        if (request.PageNumber != null && request.PageSize != null)
        {
            return await query.PaginatedListAsync(request.PageNumber.Value, request.PageSize.Value);
        }

        var users = await query.ToListAsync(cancellationToken);
        var paginatedList = new PaginatedList<UserResponse>(users, users.Count, 1, users.Count > 0 ? users.Count : 1);
        return paginatedList;
    }
}