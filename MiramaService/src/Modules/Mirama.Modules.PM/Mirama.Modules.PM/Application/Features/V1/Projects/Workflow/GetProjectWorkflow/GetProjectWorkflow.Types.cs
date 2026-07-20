using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.Workflow.GetProjectWorkflow;

public sealed record GetProjectWorkflowQuery(Guid ProjectId) : IQuery<ErrorOr<WorkflowResponse>>;
