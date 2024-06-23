namespace Core.Currency;

public static class CurrencyFactory {
    public static ICurrency Create() => DefaultCurrency;
    private static readonly ICurrency DefaultCurrency = new USD();
}