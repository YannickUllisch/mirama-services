
using AccountService.Application.Common.Extensions;
using AccountService.Application.Common.Model;
using AccountService.Application.Infrastructure.Persistence;
using ErrorOr;
using MediatR;

namespace AccountService.Application.Features.Users.Queries.GetUsers;

public class GetUsersQueryHandler(ApplicationDbContext context) : IRequestHandler<GetUsersQuery, ErrorOr<PaginatedList<UserResponse>>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ErrorOr<PaginatedList<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _context.User
            .OrderBy(user => user.Name)
            .Select(u => u.MapResponse().Value)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return users;
    }
}