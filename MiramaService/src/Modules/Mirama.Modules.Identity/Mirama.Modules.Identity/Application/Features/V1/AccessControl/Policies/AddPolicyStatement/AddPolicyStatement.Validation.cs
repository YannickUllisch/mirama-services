using FluentValidation;
using Mirama.Modules.Identity.Domain.Aggregates.Policy;
using Mirama.SharedKernel.Models.Permissions;

namespace Mirama.Modules.Identity.Application.Features.V1.AccessControl.Policies.AddPolicyStatement;

internal class AddPolicyStatementCommandValidator : AbstractValidator<AddPolicyStatementCommand>
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

    public AddPolicyStatementCommandValidator()
    {
        RuleFor(c => c.Action)
            .NotEmpty()
            .Must(ValidActions.Contains)
            .WithMessage("Action is not a recognized permission.");

        RuleFor(c => c.Resource)
            .NotEmpty()
            .Must(ValidResources.Contains)
            .WithMessage("Resource is not a recognized resource pattern.");

        RuleFor(c => c.Effect)
            .Must(e => Enum.TryParse<Effect>(e, ignoreCase: true, out _))
            .WithMessage("Effect must be 'Allow' or 'Deny'");
    }
}
