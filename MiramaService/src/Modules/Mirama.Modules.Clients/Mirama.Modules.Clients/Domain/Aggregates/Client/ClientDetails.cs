using Mirama.Modules.Clients.Domain.Enums;

namespace Mirama.Modules.Clients.Domain.Aggregates.Client;

public record ClientDetails(
    string Name,
    ClientType Type,
    string? Website,
    string? Industry,
    string? Notes);
