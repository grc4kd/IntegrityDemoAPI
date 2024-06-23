namespace Core.Currency;

public record struct USD : ICurrency
{
    public readonly string CurrencyCode => "USD";
    public readonly decimal MinimumDenomination => 0.01m;
}