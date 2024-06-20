namespace core;

public class Deposit(ICurrency currency, decimal amount)
{
    public ICurrency Currency { get; init; } = currency;
    public decimal Amount { get; init; } = amount;
}
