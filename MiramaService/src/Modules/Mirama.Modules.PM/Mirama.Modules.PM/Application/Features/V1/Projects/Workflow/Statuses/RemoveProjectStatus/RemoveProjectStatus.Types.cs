using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.Statuses.RemoveProjectStatus;

public sealed record RemoveProjectStatusCommand(
    Guid ProjectId,
    Guid StatusId) : ICommand<ErrorOr<Deleted>>;
