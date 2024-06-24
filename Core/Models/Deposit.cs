using Core.Models.CurrencyTypes;

namespace Core.Models;

public class Deposit
{
    public Currency Currency { get; init; }
    public decimal Amount { get; init; }

    public Deposit(dynamic amount, Currency currency) {
        
        decimal? decimalAmount = null;
        if (amount is decimal or double or int) {
            decimalAmount = (decimal)amount;
        }

        if (!decimalAmount.HasValue) {
            throw new ArgumentException($"Could not cast dynamic constructor argument {amount} to decimal value type.", nameof(amount));
        }

        currency.ValidateAmount(decimalAmount.Value);
        
        Currency = currency;
        Amount = decimalAmount.Value;
    }

    /// <summary>
    /// Allow constructor using the default currency from environment variables
    /// </summary>
    public Deposit(decimal amount) : this(amount, CurrencyFactory.Create()) {}
}
