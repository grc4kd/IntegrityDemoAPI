namespace core.currency;

public interface ICurrency
{
    public string CurrencyCode { get; }
    public decimal MinimumDenomination { get; }
}