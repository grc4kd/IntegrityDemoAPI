namespace Core.Currency;

public static class CurrencyFactory {
    public static ICurrency DefaultCurrency => Currencies.GetCurrency(USD.s_CurrencyCode);
}