namespace Mirama.Modules.Clients.Domain.Aggregates.Contract;

public sealed record ContractDetails(
    ContractPartyType PartyType,
    Guid PartyId,
    string Title,
    DateTime EffectiveFrom,
    DateTime? EffectiveTo);
