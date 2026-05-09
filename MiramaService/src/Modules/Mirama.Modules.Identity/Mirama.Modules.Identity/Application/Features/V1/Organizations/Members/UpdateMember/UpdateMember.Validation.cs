using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Members.UpdateMember;

internal class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(c => c.IamRoleId).NotEmpty();
    }
}
