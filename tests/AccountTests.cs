using core;

namespace tests;

public class AccountTests
{
    [Fact]
    public void DepositAmount_CheckBalance()
    {
        var customer = new Customer(name: "Joe Stevenson");
        var account = new CustomerAccount(openingBalance: 2175.13) { Customer = customer };
        var deposit = new Deposit(currency: Currencies.currencyDict[USD.CurrencyCode], amount: 112.00);

        var expectedBalance = 2287.13d;

        account.MakeDeposit(deposit);

        Assert.Equal(expectedBalance, account.GetBalance());
    }
}