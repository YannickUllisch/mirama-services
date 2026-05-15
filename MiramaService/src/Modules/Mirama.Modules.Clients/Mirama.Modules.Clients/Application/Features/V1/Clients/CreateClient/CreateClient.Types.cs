using ErrorOr;
using Mirama.Modules.Clients.Domain.Enums;
using Mirama.SharedKernel.Abstractions.Common.Interfaces;

namespace Mirama.Modules.Clients.Application.Features.V1.Clients.CreateClient;

public sealed record CreateClientCommand(
    string Name,
    ClientType Type,
    string? Website,
    string? Industry,
    string? Notes) : ICommand<ErrorOr<ClientResponse>>;

public sealed record ClientResponse(
    Guid ClientId,
    string Name,
    string Type,
    string Status,
    string? Website,
    string? Industry);
