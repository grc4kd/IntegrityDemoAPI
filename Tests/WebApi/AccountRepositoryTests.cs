using System.Text;
using DataContext;
using DataContext.Models;
using DataContext.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests.WebApi;

public class AccountRepositoryTests : IDisposable
{
    private static readonly Customer expectedCustomer = new() { Id = 5, Name = "Hank Rodgers" };
    private static readonly CustomerAccount testAccount = new CustomerAccount
    {
        Customer = expectedCustomer,
        Id = 17
    };
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AccountContext> _contextOptions;

    public AccountRepositoryTests() {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<AccountContext>()
            .UseSqlite(_connection)
            .Options;

        SeedData.Initialize(_contextOptions);
    }

    AccountContext CreateContext() => new AccountContext(_contextOptions);

    public void Dispose() => _connection.Dispose();

    [Fact]
    public void GetAccount_CustomerNotNull() {
        var repository = new AccountRepository(_contextOptions);
        
        var account = repository.GetAccount(1);

        Assert.NotNull(account);
        Assert.NotNull(account.Customer);
    }

    [Fact]
    public void MakeDeposit_InspectResponse()
    {
        decimal expectedBalance = 2287.13m;
        decimal depositAmount = 112;
        decimal openingBalance = 2175.13m;

        using var resetContext = CreateContext();
        var account = resetContext.Accounts.Find(testAccount.Id);
        if (account == null)
        {
            resetContext.Accounts.Add(testAccount);
        }
        if (account != null)
        {
            account.Balance = openingBalance;
            account.OpeningBalance = openingBalance;
        }
        resetContext.SaveChanges();

        var repository = new AccountRepository(_contextOptions);
        var depositResponse = repository.MakeDeposit(testAccount, depositAmount);

        Assert.NotNull(depositResponse);

        Assert.Multiple(() =>
        {
            Assert.Equal(testAccount.Id, depositResponse.AccountId);
            Assert.Equal(expectedCustomer.Id, depositResponse.CustomerId);
            Assert.Equal(expectedBalance, depositResponse.Balance);
            Assert.True(depositResponse.Succeeded);
        });
    }

    [Fact]
    public void MakeWithdrawal_InspectResponse()
    {
        decimal expectedBalance = 2287.13m;
        decimal withdrawalAmount = 112;
        decimal openingBalance = 2399.13m;

        using var resetContext = CreateContext();
        var account = resetContext.Accounts.Find(testAccount.Id);
        if (account == null)
        {
            resetContext.Accounts.Add(testAccount);
        }
        if (account != null)
        {
            account.Balance = openingBalance;
            account.OpeningBalance = openingBalance;
        }
        resetContext.SaveChanges();

        var repository = new AccountRepository(_contextOptions);
        var depositResponse = repository.MakeWithdrawal(testAccount, withdrawalAmount);

        Assert.NotNull(depositResponse);

        Assert.Multiple(() =>
        {
            Assert.Equal(testAccount.Id, depositResponse.AccountId);
            Assert.Equal(expectedCustomer.Id, depositResponse.CustomerId);
            Assert.Equal(expectedBalance, depositResponse.Balance);
            Assert.True(depositResponse.Succeeded);
        });
    }
}