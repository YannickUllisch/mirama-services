using FluentValidation;

namespace Mirama.Modules.Clients.Application.Features.V1.Clients.CreateClient;

internal class CreateClientCommandValidator : AbstractValidator<CreateClientCommand>
{
    public CreateClientCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(150);

        RuleFor(x => x.Type)
            .IsInEnum();

        RuleFor(x => x.Website)
            .Must(w => w == null || Uri.TryCreate(w, UriKind.Absolute, out _))
            .WithMessage("Website must be a valid URL.");
    }
}
