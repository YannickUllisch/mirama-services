using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth.LinkUserExternalId;

internal class LinkUserExternalIdCommandValidator : AbstractValidator<LinkUserExternalIdCommand>
{
    public LinkUserExternalIdCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.ExternalId).NotEmpty();
    }
}
