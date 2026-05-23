using ErrorOr;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Identity.Application.Features.V1.Auth.LinkUserExternalId;

public sealed record LinkUserExternalIdCommand(Guid UserId, Guid ExternalId) : ICommand<ErrorOr<Success>>;

public sealed record LinkExternalIdBody(Guid ExternalId);
