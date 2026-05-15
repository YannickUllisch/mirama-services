namespace Mirama.Modules.Clients.Contracts.Dtos;

public sealed record ClientSummaryDto(
    Guid ClientId,
    string Name,
    string ClientType,
    string Status);
