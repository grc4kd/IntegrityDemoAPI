namespace core;

public class Deposit(ICurrency currency, double amount)
{
    public ICurrency Currency { get; init; } = currency;
    public double Amount { get; init; } = amount;
}
