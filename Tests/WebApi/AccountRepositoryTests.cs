using DataContext;
using DataContext.Models;
using DataContext.Repositories;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests.WebApiTest;

public class AccountRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AccountContext> _contextOptions;

    // all current tests expect these values
    private static readonly long expectedCustomerId = 5;
    private static readonly long expectedAccountId = 17;

    public AccountRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<AccountContext>()
            .UseSqlite(_connection)
            .Options;

        SeedData.Initialize(_contextOptions);
    }

    AccountContext CreateContext() => new AccountContext(_contextOptions);

    public void Dispose() => _connection.Dispose();

    private static CustomerAccount CreateTestAccount() {
        return new CustomerAccount
        {
            Customer = new() { 
                Id = expectedCustomerId, 
                Name = "Hank Rodgers" 
            },
            Id = expectedAccountId
        };
    }

    [Fact]
    public void GetAccount_CustomerNotNull()
    {
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
        var testAccount = CreateTestAccount();
        resetContext.Attach(testAccount);

        testAccount.Balance = openingBalance;
        testAccount.OpeningBalance = openingBalance;

        resetContext.SaveChanges();

        var repository = new AccountRepository(_contextOptions);
        var depositResponse = repository.MakeDeposit(testAccount, depositAmount);

        Assert.NotNull(depositResponse);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedAccountId, depositResponse.AccountId);
            Assert.Equal(expectedCustomerId, depositResponse.CustomerId);
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
        var testAccount = CreateTestAccount();
        resetContext.Attach(testAccount);

        testAccount.Balance = openingBalance;
        testAccount.OpeningBalance = openingBalance;

        resetContext.SaveChanges();

        var repository = new AccountRepository(_contextOptions);
        var depositResponse = repository.MakeWithdrawal(testAccount, withdrawalAmount);

        Assert.NotNull(depositResponse);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedAccountId, depositResponse.AccountId);
            Assert.Equal(expectedCustomerId, depositResponse.CustomerId);
            Assert.Equal(expectedBalance, depositResponse.Balance);
            Assert.True(depositResponse.Succeeded);
        });
    }

    [Fact]
    public void CloseAccount_InspectResponse()
    {
        decimal balance = 0.00m;

        using var resetContext = CreateContext();
        var testAccount = CreateTestAccount();
        resetContext.Attach(testAccount);

        testAccount.Balance = balance;
        testAccount.AccountStatus = Core.Models.AccountStatusCode.OPEN.ToString();

        resetContext.SaveChanges();

        var repository = new AccountRepository(_contextOptions);
        var closeAccountResponse = repository.CloseAccount(testAccount.Id);

        Assert.NotNull(closeAccountResponse);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedAccountId, closeAccountResponse.AccountId);
            Assert.Equal(expectedCustomerId, closeAccountResponse.CustomerId);
            Assert.True(closeAccountResponse.Succeeded);
        });
    }

    [Fact]
    public void InvalidStatusCode_Read_CheckException() 
    {
        using var resetContext = CreateContext();
        var testAccount = CreateTestAccount();
        resetContext.Attach(testAccount);

        // database contains string values for status codes
        testAccount.AccountStatus = "BOGUS";

        resetContext.SaveChanges();

        var repository = new AccountRepository(_contextOptions);
        Assert.Throws<InvalidOperationException>(() => repository.CloseAccount(testAccount.Id));
    }
}