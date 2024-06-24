using DataContext;
using DataContext.Models;
using DataContext.Repositories;
using DataContext.Requests;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests.WebApiTests;

public class AccountRepositoryTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<AccountContext> _contextOptions;

    public AccountRepositoryTests()
    {
        Environment.SetEnvironmentVariable("DefaultCurrencyCode", "USD");

        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<AccountContext>()
            .UseSqlite(_connection)
            .Options;

        SeedData.Initialize(_contextOptions);
    }

    AccountContext CreateContext() => new AccountContext(_contextOptions);

    public void Dispose() => _connection.Dispose();

    private static CustomerAccount CreateTestAccount() =>
        new()
        {
            Customer = new Customer(Name: "Hank Rodgers"),
            Balance = 525.00m
        };

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

        resetContext.SaveChanges();

        var repository = new AccountRepository(_contextOptions);
        var response = repository.MakeDeposit(testAccount, depositAmount);

        Assert.NotNull(response);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedBalance, response.Balance);
            Assert.True(response.Succeeded);
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

        resetContext.SaveChanges();

        var repository = new AccountRepository(_contextOptions);
        var response = repository.MakeWithdrawal(testAccount, withdrawalAmount);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedBalance, response.Balance);
            Assert.True(response.Succeeded);
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

        Assert.Multiple(() =>
        {
            Assert.True(closeAccountResponse.Succeeded);
        });
    }

    [Fact]
    public void OpenAccount_InspectResponse()
    {
        double initialDeposit = 525.00d;
        decimal expectedBalance = 525.00m;
        var testCustomer = CreateTestAccount().Customer;

        using var resetContext = CreateContext();

        resetContext.Add(testCustomer);

        resetContext.SaveChanges();

        var repository = new AccountRepository(_contextOptions);
        var request = new OpenAccountRequest() {
            CustomerId = testCustomer.Id,
            InitialDeposit = initialDeposit,
            AccountTypeId = Core.Models.AccountType.AccountTypeCode("Savings")
        };
        var openAccountResponse = repository.OpenAccount(request);

        Assert.Multiple(() =>
        {
            //Assert.Equal(Core.Models.AccountType.AccountTypeCode("Savings"), openAccountResponse.AccountTypeId);
            Assert.Equal("Savings", Core.Models.AccountType.AccountTypeName(openAccountResponse.AccountTypeId));
            Assert.Equal(expectedBalance, openAccountResponse.Balance);
            Assert.True(openAccountResponse.Succeeded);
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