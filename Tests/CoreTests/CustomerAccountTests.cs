using Core.Models;
using Tests.Fixtures;

namespace Tests.CoreTests;

public class CustomerAccountTests : IClassFixture<DefaultCurrencyCodeFixture>
{
    [Fact]
    public void DepositAmount_CheckBalance()
    {
        decimal amount = 112;
        var expectedBalance = 2287.13m;

        var account = new CustomerAccount(
            accountTypeId: AccountType.AccountTypeId("Checking"),
            accountStatusCode: AccountStatusCode.OPEN,
            id: 0,
            balance: 2175.13m);
        var deposit = new Deposit(amount);

        account.MakeDeposit(deposit);

        Assert.Equal(expectedBalance, account.Balance);
    }

    [Fact]
    public void WithdrawAmount_CheckBalance()
    {
        decimal amount = 112;
        decimal expectedBalance = 2287.13m;

        var account = new CustomerAccount(
            id: 0,
            accountTypeId: AccountType.AccountTypeId("Checking"),
            accountStatusCode: AccountStatusCode.OPEN,
            balance: 2399.13m);
        var withdrawal = new Withdrawal(amount);

        account.MakeWithdrawal(withdrawal);

        Assert.Equal(expectedBalance, account.Balance);
    }

    [Fact]
    public void NewAccount_CheckStatus()
    {
        var accountId = 1;
        var account = new Account(accountId);

        Assert.Equal(AccountStatusCode.OPEN, account.AccountStatus.StatusCode);
    }

    [Fact]
    public void CloseAccount_CheckStatus()
    {
        var accountId = 2;
        var account = new Account(accountId);

        account.CloseAccount();

        Assert.Equal(AccountStatusCode.CLOSED, account.AccountStatus.StatusCode);
    }

    [Fact]
    public void CloseAccount_WithNonZeroBalance_CheckException()
    {
        var accountId = 3;
        decimal balance = 0.10m;
        var account = new Account(accountId, balance);

        Assert.Throws<InvalidOperationException>(account.CloseAccount);
    }

    [Fact]
    public void CloseAccount_AlreadyClosed_CheckException()
    {
        var accountId = 4;
        var account = new Account(accountId, statusCode: AccountStatusCode.CLOSED);

        Assert.Throws<InvalidOperationException>(account.CloseAccount);
    }
}