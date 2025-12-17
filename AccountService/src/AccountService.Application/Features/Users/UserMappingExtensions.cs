
using AccountService.Application.Domain.User;
using ErrorOr;

namespace AccountService.Application.Features.Users;

internal static class UserMapper
{
    internal static ErrorOr<UserResponse> MapResponse(this User userModel, int? count = null)
    {
        string? role = Enum.GetName(userModel.Role);

        if (role == null)
        {
            return Error.Unexpected("An unexpected error occurred.");
        }

        return new UserResponse
        {
            ContactEmail = userModel.Contact.ContactEmail,
            ContactPhoneNumber = userModel.Contact.ContactPhoneNumber,
            Email = userModel.Email,
            Id = userModel.Id.Value,
            Image = userModel.Image,
            Name = userModel.Name,
            Role = role,
            Count = count,
        };
    }
}
