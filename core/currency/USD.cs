namespace core;

public record struct USD : ICurrency
{
    public static string CurrencyCode => "USD";

    public readonly bool Equals(ICurrency? other)
    {
        if (other is USD) {
            return true;
        }

        return false;
    }

    public readonly double MinimumAmount()
    {
        return 0.01d;
    }
}