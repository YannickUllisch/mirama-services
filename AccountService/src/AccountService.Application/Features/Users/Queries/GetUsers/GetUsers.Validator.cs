
using AccountService.Application.Common.Interfaces;
using FluentValidation;

namespace AccountService.Application.Features.Users.Queries.GetUsers;

internal class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
{
    public GetUsersQueryValidator(IGlobalRoleProvider roleProvider)
    {
        RuleFor(req => req.PageSize)
            .LessThanOrEqualTo(50);
    }
}