using core;

namespace tests;

public class AccountTests
{
    [Fact]
    public void DepositAmount_CheckBalance()
    {
        decimal amount = 112;
        var openingBalance = 2175.13m;
        var expectedBalance = 2287.13m;

        var customer = new Customer(name: "Joe Stevenson");
        var account = new CustomerAccount(openingBalance) { Customer = customer };
        var deposit = new Deposit(currency: Currencies.currencyDict[USD.CurrencyCode], amount);

        account.MakeDeposit(deposit);

        Assert.Equal(expectedBalance, account.GetBalance());
    }
}