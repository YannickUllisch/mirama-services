using FluentValidation;

namespace Mirama.Modules.Workspace.Application.Features.V1.ViewStates.SaveViewState;

internal class SaveViewStateCommandValidator : AbstractValidator<SaveViewStateCommand>
{
    // Kept deliberately small: StateJson is view *configuration* (column order, filters,
    // collapsed groups) - never business rows - so a generous cap here is still a strong
    // guardrail against a client accidentally persisting something it shouldn't.
    private const int MaxStateJsonLength = 65_536;

    public SaveViewStateCommandValidator()
    {
        RuleFor(x => x.SurfaceKey)
            .NotEmpty()
            .MaximumLength(200)
            .Matches("^[a-zA-Z0-9:_-]+$")
            .WithMessage("Surface key may only contain letters, digits, ':', '_' and '-'.");

        RuleFor(x => x.ViewType)
            .IsInEnum();

        RuleFor(x => x.StateJson)
            .NotEmpty()
            .MaximumLength(MaxStateJsonLength);
    }
}
