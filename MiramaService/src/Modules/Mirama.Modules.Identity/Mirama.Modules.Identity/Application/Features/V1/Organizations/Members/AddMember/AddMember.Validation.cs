using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members.AddMember;

internal class AddMemberCommandValidator : AbstractValidator<AddMemberCommand>
{
    public AddMemberCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
        RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(c => c.IamRoleId).NotEmpty();
    }
}
