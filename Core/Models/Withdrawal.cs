using Core.Currency;

namespace Core.Models;

public class Withdrawal(decimal amount, ICurrency currency)
{
    public ICurrency Currency { get; init; } = currency;
    public decimal Amount { get; init; } = amount;

    /// <summary>
    /// Allow constructor using the implied default currency.
    /// </summary>
    /// <param name="amount"></param>
    public Withdrawal(decimal amount) : this(amount, CurrencyFactory.Create()) {}
}
