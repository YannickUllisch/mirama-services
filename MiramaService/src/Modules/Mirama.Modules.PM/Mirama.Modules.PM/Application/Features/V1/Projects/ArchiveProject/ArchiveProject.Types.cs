using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.PM.Application.Features.V1.Projects.ArchiveProject;

public sealed record ArchiveProjectCommand(Guid Id) : ICommand<ErrorOr<Success>>;
