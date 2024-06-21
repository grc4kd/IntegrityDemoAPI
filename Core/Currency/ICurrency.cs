namespace Core.Currency;

public interface ICurrency
{
    public string CurrencyCode { get; }
    public decimal MinimumDenomination { get; }
}