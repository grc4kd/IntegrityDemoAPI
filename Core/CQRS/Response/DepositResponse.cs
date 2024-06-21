namespace Core.CQRS.Response;

public record DepositResponse(decimal Balance)
{
    public decimal Balance { get; } = Balance;
}