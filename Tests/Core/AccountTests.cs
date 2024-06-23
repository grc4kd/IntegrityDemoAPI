using Core.Models;

namespace Tests.CoreTest;

public class AccountTests
{
    [Fact]
    public void DepositAmount_CheckBalance()
    {
        decimal amount = 112;
        var openingBalance = 2175.13m;
        var expectedBalance = 2287.13m;

        var customer = new Customer(name: "Joe Stevenson");
        var account = new CustomerAccount(customer, openingBalance);
        var deposit = new Deposit(amount);

        account.MakeDeposit(deposit);

        Assert.Equal(expectedBalance, account.Balance);
    }

    [Fact]
    public void WithdrawAmount_CheckBalance() 
    {
        decimal amount = 112;
        decimal openingBalance = 2399.13m;
        decimal expectedBalance = 2287.13m;
        
        var account = new CustomerAccount(new(name: "Steve Tulip"), openingBalance);
        var withdrawal = new Withdrawal(amount);

        account.MakeWithdrawal(withdrawal);

        Assert.Equal(expectedBalance, account.Balance);
    }

    [Fact]
    public void NewAccount_CheckStatus()
    {
        var account = new CustomerAccount(new(name: "Vijaya Singh"));

        Assert.Equal(AccountStatusCode.OPEN, account.AccountStatus.StatusCode);
    }

    [Fact]
    public void CloseAccount_CheckStatus()
    {
        var account = new CustomerAccount(new(name: "Roger Wilco"));

        account.CloseAccount();

        Assert.Equal(AccountStatusCode.CLOSED, account.AccountStatus.StatusCode);
    }

    [Fact]
    public void CloseAccount_WithNonZeroBalance_CheckException() 
    {
        var account = new CustomerAccount(new(name: "Butch Harris"), 1234);

        Assert.Throws<InvalidOperationException>(account.CloseAccount);
    }

    [Fact]
    public void CloseAccount_AlreadyClosed_CheckException()
    {
        var account = new CustomerAccount(new(name: "James Torvich"));
        account.CloseAccount();

        Assert.Throws<InvalidOperationException>(account.CloseAccount);
    }
}