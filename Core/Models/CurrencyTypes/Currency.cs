namespace Core.Models.CurrencyTypes;

public record Currency(string CurrencyCode, decimal MinimumDenomination)
{
    public void ValidateAmount(decimal amount) {
        if (amount.Scale > MinimumDenomination.Scale) 
        {
            throw new ArgumentOutOfRangeException(nameof(amount), $"Balance amount: {amount} has fractional value less than the minimum denomination for currency code: {CurrencyCode}.");
        }

        if (amount % MinimumDenomination > 0) 
        {
            throw new ArgumentOutOfRangeException(nameof(amount), $"Balance amount: {amount} cannot be broken down into the minimum denomination: {MinimumDenomination}.");
        }
    }
}