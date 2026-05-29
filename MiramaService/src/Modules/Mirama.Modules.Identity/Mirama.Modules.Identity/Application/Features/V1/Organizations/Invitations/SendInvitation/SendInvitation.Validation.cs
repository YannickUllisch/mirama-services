using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Invitations.SendInvitation;

internal class SendInvitationCommandValidator : AbstractValidator<SendInvitationCommand>
{
    public SendInvitationCommandValidator()
    {
        RuleFor(c => c.Email).NotEmpty().EmailAddress().MaximumLength(320);
        RuleFor(c => c.Name).NotEmpty().MaximumLength(200);
        RuleFor(c => c.IamRoleId).NotEmpty();
    }
}
