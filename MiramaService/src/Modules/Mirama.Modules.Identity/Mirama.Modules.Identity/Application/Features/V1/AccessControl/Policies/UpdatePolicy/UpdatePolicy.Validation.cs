using FluentValidation;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.SharedKernel.Models.Permissions;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.UpdatePolicy;

internal class UpdatePolicyCommandValidator : AbstractValidator<UpdatePolicyCommand>
{
    public UpdatePolicyCommandValidator()
    {
        RuleFor(c => c.Id).NotEmpty();

        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(c => c.Description)
            .MaximumLength(500)
            .When(c => c.Description is not null);

        RuleForEach(c => c.AddStatements).SetValidator(new StatementDtoValidator());

        RuleForEach(c => c.RemoveStatementIds).NotEmpty();
    }
}

internal class StatementDtoValidator : AbstractValidator<StatementDto>
{
    private static readonly HashSet<string> ValidActions = new(
        Permissions.All
            .Concat(Permissions.AllGroups.Select(g => g.AllActionsPattern))
            .Append(Permissions.Wildcard)
            .Append(Permissions.ReadAll),
        StringComparer.OrdinalIgnoreCase);

    private static readonly HashSet<string> ValidResources = new(
        Permissions.AllGroups.Select(g => g.ResourcePattern).Append(Permissions.Wildcard),
        StringComparer.OrdinalIgnoreCase);

    public StatementDtoValidator()
    {
        RuleFor(s => s.Action)
            .NotEmpty()
            .Must(ValidActions.Contains)
            .WithMessage("Action is not a recognized permission.");

        RuleFor(s => s.Resource)
            .NotEmpty()
            .Must(ValidResources.Contains)
            .WithMessage("Resource is not a recognized resource pattern.");

        RuleFor(s => s.Effect)
            .Must(e => Enum.TryParse<Effect>(e, ignoreCase: true, out _))
            .WithMessage("Effect must be 'Allow' or 'Deny'.");
    }
}
