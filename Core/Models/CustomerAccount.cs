namespace Core.Models;

public class CustomerAccount : IAccount
{
    private readonly Customer customer;
    private decimal balance = 0.00m;
    private int lastIdSeed = -1;


    public long Id
    {
        get
        {
            var seed = lastIdSeed;

            object _lock = new();

            lock (_lock) 
            {
                if (lastIdSeed == seed)
                {
                    checked {
                        seed++;
                    }

                    lastIdSeed = ++seed;   
                }
                else
                {
                    throw new InvalidOperationException(
                        $"Expected {nameof(lastIdSeed)}: {lastIdSeed} to equal {nameof(seed)}: seed in lock.");
                }
            }
            if (lastIdSeed != seed) {
                throw new InvalidOperationException(
                    $"Expected {nameof(lastIdSeed)}: {lastIdSeed} to equal {nameof(seed)}: seed after lock.");
            }

            return seed;
        }
    }

    private bool ValidateConstructorParameters(decimal? openingBalance)
    {
        if (openingBalance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(openingBalance),
                $"{nameof(openingBalance)} cannot be negative. Value: {openingBalance}");
        }

        return true;
    }

    public CustomerAccount(Customer customer, decimal openingBalance = 0)
    {
        this.customer = customer;

        if (!ValidateConstructorParameters(openingBalance))
        {
            return;
        }

        if (openingBalance > 0)
        {
            balance += openingBalance;
        }
    }

    public void MakeDeposit(Deposit deposit)
    {
        if (deposit.Currency is Currency.USD)
        {
            balance += deposit.Amount;
        }
    }

    public decimal GetBalance()
    {
        return balance;
    }
}
