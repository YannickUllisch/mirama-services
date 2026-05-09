using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams.CreateTeam;

internal class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
    }
}
