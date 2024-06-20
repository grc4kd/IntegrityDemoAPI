namespace core;

public class USD : ICurrency
{
    public static string CurrencyCode => "USD";

    public bool Equals(ICurrency? other)
    {
        if (other is USD) {
            return true;
        }

        return false;
    }

    public double MinimumAmount()
    {
        return 0.01d;
    }
}