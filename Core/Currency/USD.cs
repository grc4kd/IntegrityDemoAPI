namespace Core.Currency;

record struct USD : ICurrency
{
    public const string s_CurrencyCode = "USD";
    public readonly string CurrencyCode => s_CurrencyCode;
    public readonly decimal MinimumDenomination => 0.01m;
}