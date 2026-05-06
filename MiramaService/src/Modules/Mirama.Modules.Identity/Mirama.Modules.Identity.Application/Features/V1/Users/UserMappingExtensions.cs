using ErrorOr;
using Mirama.Modules.Identity.Domain.Aggregates.User;

namespace Mirama.Modules.Identity.Application.Features.V1.Users;

internal static class UserMapper
{
    internal static ErrorOr<UserResponse> MapResponse(this User userModel)
    {
        string? role = Enum.GetName(userModel.Role);

        if (role == null)
        {
            return Error.Unexpected("An unexpected error occurred.");
        }

        return new UserResponse
        {
            Email = userModel.Email,
            Id = userModel.Id.Value,
            Image = userModel.Image,
            Name = userModel.Name,
            Role = role,
        };
    }
}
