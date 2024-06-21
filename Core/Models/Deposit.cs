using Core.Currency;

namespace Core.Models;

public class Deposit(decimal amount, ICurrency? currency = null)
{
    public ICurrency Currency { get; init; } = currency ?? CurrencyFactory.DefaultCurrency;
    public decimal Amount { get; init; } = amount;
}
