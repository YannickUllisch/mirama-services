using FluentValidation;

namespace Mirama.Modules.Clients.Application.Features.V1.Portal.InviteContact;

internal class InviteContactCommandValidator : AbstractValidator<InviteContactCommand>
{
    public InviteContactCommandValidator()
    {
        RuleFor(x => x.ClientId).NotEmpty();
        RuleFor(x => x.ContactId).NotEmpty();
    }
}
