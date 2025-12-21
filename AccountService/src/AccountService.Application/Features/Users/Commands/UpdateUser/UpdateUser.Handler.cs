

using AccountService.Application.Domain.Aggregates.User;
using AccountService.Application.Infrastructure.Persistence;
using ErrorOr;
using MediatR;

namespace AccountService.Application.Features.Users.Commands.UpdateUser;

internal class UpdateUserCommandHandler(ApplicationDbContext context) : IRequestHandler<UpdateUserCommand, ErrorOr<UserResponse>>
{
    private readonly ApplicationDbContext _context = context;

    public async Task<ErrorOr<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        User? user = await _context.Users.FindAsync([request.Id], cancellationToken);

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

        await _context.SaveChangesAsync(cancellationToken);

        return user.MapResponse();
    }
}