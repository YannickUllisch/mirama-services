using FluentValidation;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.Modules.Identity.Domain.Enums;
using Mirama.SharedKernel.Models.Permissions;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.CreatePolicy;

internal class CreatePolicyCommandValidator : AbstractValidator<CreatePolicyCommand>
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

    public CreatePolicyCommandValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(c => c.Description)
            .MaximumLength(500)
            .When(c => c.Description is not null);

        RuleFor(c => c.Scope)
            .NotEmpty()
            .Must(s => Enum.TryParse<AccessScope>(s, ignoreCase: true, out _))
            .WithMessage("Scope must be 'Organization' or 'Project'");

        RuleForEach(c => c.Statements).ChildRules(stmt =>
        {
            stmt.RuleFor(s => s.Action)
                .NotEmpty()
                .Must(a => ValidActions.Contains(a))
                .WithMessage("Action is not a recognized permission.");

            stmt.RuleFor(s => s.Resource)
                .NotEmpty()
                .Must(r => ValidResources.Contains(r))
                .WithMessage("Resource is not a recognized resource pattern.");

            stmt.RuleFor(s => s.Effect)
                .Must(e => Enum.TryParse<Effect>(e, ignoreCase: true, out _))
                .WithMessage("Effect must be 'Allow' or 'Deny'");
        });
    }
}
