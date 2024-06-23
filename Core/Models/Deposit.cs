using Core.Models.CurrencyTypes;

namespace Core.Models;

public class Deposit
{
    public Currency Currency { get; init; }
    public decimal Amount { get; init; }

    public Deposit(decimal amount, Currency currency) {
        currency.ValidateAmount(amount);
        
        Currency = currency;
        Amount = amount;
    }

    /// <summary>
    /// Allow constructor using the default currency from environment variables
    /// </summary>
    public Deposit(decimal amount) : this(amount, CurrencyFactory.Create()) {}
}
