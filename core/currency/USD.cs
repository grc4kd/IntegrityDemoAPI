namespace core;

public record struct USD : ICurrency
{
    public static string CurrencyCode => "USD";

    public static decimal MinimumDenomination
    {
        get { return .01m; }
    }
}