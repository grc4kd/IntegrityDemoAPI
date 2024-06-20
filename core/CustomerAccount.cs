namespace core;

public class CustomerAccount
{
    public required Customer Customer { get; init; } = new Customer("NONE");
    private decimal balance = 0.00m;

    private bool ValidateConstructorParameters(decimal openingBalance) {
        if (openingBalance < 0) {
            throw new ArgumentOutOfRangeException(nameof(openingBalance), 
                $"{nameof(openingBalance)} cannot be negative. Value: {openingBalance}");
        }

        return true;
    }

    public CustomerAccount(decimal openingBalance = 0) {
        if (!ValidateConstructorParameters(openingBalance)) {
            return;
        }

        if (openingBalance > 0) {
            balance += openingBalance;
        }
    }

    public void MakeDeposit(Deposit deposit)
    {
        if (deposit.Currency is USD) {
            balance += deposit.Amount;
        }
    }

    public decimal GetBalance() {
        return balance;
    }
}
