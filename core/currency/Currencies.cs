namespace core.currency;

public static class Currencies
{
    public static ICurrency GetCurrency(string currencyCode) {
        return currencyDict[currencyCode];
    }

    private static readonly Dictionary<string, ICurrency> currencyDict = new()
    {
        {"USD", new USD()}
    };
}