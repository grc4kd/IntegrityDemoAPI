namespace core;

public interface ICurrency {
    static string CurrencyCode { get; } = string.Empty;

    static decimal MinimumDenomination { get; } = 0.01m;
}