using Core.Models.CurrencyTypes;

namespace Core.Models;

public class Withdrawal(decimal amount, Currency currency)
{
    public Currency Currency { get; init; } = currency;
    public decimal Amount { get; init; } = amount;

    /// <summary>
    /// Allow constructor using the default currency from environment variables
    /// </summary>
    public Withdrawal(decimal amount) : this(amount, CurrencyFactory.Create()) {}
}
