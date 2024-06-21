namespace Core.CQRS.Response;

public record DepositResponse(long AccountId, decimal Balance);