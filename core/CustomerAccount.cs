namespace core;

public class CustomerAccount
{
    public required Customer Customer { get; init; } = new Customer("NONE");
    private double balance = 0.00;

    private bool ValidateConstructorParameters(double openingBalance) {
        if (openingBalance < 0) {
            throw new ArgumentOutOfRangeException(nameof(openingBalance), 
                $"{nameof(openingBalance)} cannot be negative. Value: {openingBalance}");
        }

        return true;
    }

    public CustomerAccount(double openingBalance = 0) {
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

    public double GetBalance() {
        return balance;
    }
}
