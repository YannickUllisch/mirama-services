using FluentValidation;

namespace Mirama.Modules.Identity.Application.Features.V1.Organizations.Teams.UpdateTeam;

internal class UpdateTeamCommandValidator : AbstractValidator<UpdateTeamCommand>
{
    public UpdateTeamCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
    }
}
