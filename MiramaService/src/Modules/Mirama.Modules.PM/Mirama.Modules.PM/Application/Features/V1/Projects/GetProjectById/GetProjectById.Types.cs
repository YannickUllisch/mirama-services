using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.GetProjectById;

public sealed record GetProjectByIdQuery(Guid Id) : IQuery<ErrorOr<ProjectResponse>>;
