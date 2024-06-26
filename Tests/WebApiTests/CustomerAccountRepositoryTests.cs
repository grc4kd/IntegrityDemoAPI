using DataContext;
using DataContext.Models;
using DataContext.Repositories;
using DataContext.Requests;
using DataContext.Responses;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Tests.Fixtures;

namespace Tests.WebApiTests;

public class CustomerAccountRepositoryTests : IDisposable, IClassFixture<DefaultCurrencyCodeFixture>
{
    private readonly SqliteConnection _connection;
    private readonly DbContextOptions<CustomerAccountContext> _contextOptions;

    public CustomerAccountRepositoryTests()
    {
        _connection = new SqliteConnection("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<CustomerAccountContext>()
            .UseSqlite(_connection)
            .Options;

        SeedData.Initialize(_contextOptions);
    }

    CustomerAccountContext CreateContext() => new(_contextOptions);

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _connection.Dispose();
    }

    private static CustomerAccount CreateTestAccount() =>
        new()
        {
            Customer = new Customer(Name: "Hank Rodgers"),
            Balance = 525.00m
        };

    [Fact]
    public void GetAccount_CustomerNotNull()
    {
        var repository = new CustomerAccountRepository(CreateContext());

        var account = repository.GetCustomerAccount(1);

        Assert.NotNull(account);
        Assert.NotNull(account.Customer);
    }

    [Fact]
    public void MakeDeposit_InspectResponse()
    {
        decimal expectedBalance = 2287.13m;
        double amount = 112.00d;
        decimal openingBalance = 2175.13m;

        using var resetContext = CreateContext();
        var testAccount = CreateTestAccount();
        resetContext.Attach(testAccount);

        testAccount.Balance = openingBalance;

        resetContext.SaveChanges();

        var repository = new CustomerAccountRepository(CreateContext());

        var request = new DepositRequest()
        {
            AccountId = testAccount.Id,
            CustomerId = testAccount.Customer.Id,
            Amount = amount
        };

        var response = repository.MakeDeposit(request);

        Assert.NotNull(response);
        Assert.True(response.Succeeded);
        Assert.Equal(testAccount.Id, response.AccountId);
        Assert.Equal(testAccount.Customer.Id, response.CustomerId);
    
        // the web controller would be a good place to implement the mediator patterrn
        Assert.IsType<DepositResponse>(response);
        var depositResponse = response as DepositResponse;
        Assert.NotNull(depositResponse);
        Assert.Equal(expectedBalance, depositResponse.Balance);
    }

    [Fact]
    public void MakeWithdrawal_InspectResponse()
    {
        decimal expectedBalance = 2287.13m;
        double withdrawalAmount = 112.00d;
        decimal openingBalance = 2399.13m;

        using var resetContext = CreateContext();
        var testAccount = CreateTestAccount();
        resetContext.Attach(testAccount);

        testAccount.Balance = openingBalance;

        resetContext.SaveChanges();

        var repository = new CustomerAccountRepository(CreateContext());

        var request = new WithdrawalRequest()
        {
            AccountId = testAccount.Id,
            CustomerId = testAccount.Customer.Id,
            Amount = withdrawalAmount
        };

        var response = repository.MakeWithdrawal(request);

        Assert.True(response.Succeeded);
        Assert.Equal(testAccount.Id, response.AccountId);
        Assert.Equal(testAccount.Customer.Id, response.CustomerId);
        
        Assert.IsType<WithdrawalResponse>(response);
        var withdrawalResponse = response as WithdrawalResponse;
        Assert.NotNull(withdrawalResponse);
        Assert.Equal(expectedBalance, withdrawalResponse.Balance);
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

        var repository = new CustomerAccountRepository(CreateContext());
        
        var request = new CloseAccountRequest() {
            AccountId = testAccount.Id,
            CustomerId = testAccount.Customer.Id
        };

        var response = repository.CloseCustomerAccount(request);
        
        Assert.IsType<CloseAccountResponse>(response);
        Assert.True(response.Succeeded);
        Assert.Equal(testAccount.Id, response.AccountId);
        Assert.Equal(testAccount.Customer.Id, response.CustomerId);
    }

    [Fact]
    public void OpenAccount_InspectResponse()
    {
        double initialDeposit = 525.00d;
        decimal expectedBalance = 525.00m;
        var testAccount = CreateTestAccount();
        var testCustomer = testAccount.Customer;

        using var resetContext = CreateContext();

        resetContext.Add(testCustomer);

        resetContext.SaveChanges();

        var repository = new CustomerAccountRepository(CreateContext());
        
        var request = new OpenAccountRequest()
        {
            CustomerId = testCustomer.Id,
            InitialDeposit = initialDeposit,
            AccountTypeId = Core.Models.AccountType.AccountTypeId("Savings")
        };
        
        var response = repository.OpenCustomerAccount(request);

        Assert.True(response.Succeeded);
        Assert.Equal(response.CustomerId, testAccount.Customer.Id);
        Assert.InRange(response.AccountId, 0, int.MaxValue);
        Assert.Equal(Core.Models.AccountType.AccountTypeId("Savings"), response.AccountTypeId);
        Assert.Equal(expectedBalance, response.Balance);
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

        var repository = new CustomerAccountRepository(CreateContext());

        var closeAccountRequest = new CloseAccountRequest() {
            AccountId = testAccount.Id,
            CustomerId = testAccount.Customer.Id
        };

        Assert.Throws<InvalidOperationException>(() => repository.CloseCustomerAccount(closeAccountRequest));
    }

    [Fact]
    public void Deposit_FractionalAmount_CheckException()
    {
        double depositAmount = 111.999d;
        decimal openingBalance = 2399.13m;

        using var resetContext = CreateContext();
        var testAccount = CreateTestAccount();
        resetContext.Attach(testAccount);

        testAccount.Balance = openingBalance;

        resetContext.SaveChanges();

        var repository = new CustomerAccountRepository(CreateContext());

        var request = new DepositRequest()
        {
            AccountId = testAccount.Id,
            CustomerId = testAccount.Customer.Id,
            Amount = depositAmount
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => repository.MakeDeposit(request));
    }

    [Fact]
    public void Withdrawal_FractionalAmount_CheckException()
    {
        double withdrawalAmount = 112.001d;
        decimal openingBalance = 2399.13m;

        using var resetContext = CreateContext();
        var testAccount = CreateTestAccount();
        resetContext.Attach(testAccount);

        testAccount.Balance = openingBalance;

        resetContext.SaveChanges();

        var repository = new CustomerAccountRepository(CreateContext());

        var request = new WithdrawalRequest()
        {
            AccountId = testAccount.Id,
            CustomerId = testAccount.Customer.Id,
            Amount = withdrawalAmount
        };

        Assert.Throws<ArgumentOutOfRangeException>(() => repository.MakeWithdrawal(request));
    }

    [Fact]
    public void Deposit_NoCustomerIdMatch_CheckError() 
    {
        double depositAmount = 113.99d;
        decimal openingBalance = 2399.13m;
        int invalidCustomerId = 0;

        using var resetContext = CreateContext();
        var testAccount = CreateTestAccount();
        resetContext.Attach(testAccount);

        testAccount.Balance = openingBalance;

        resetContext.SaveChanges();

        var repository = new CustomerAccountRepository(CreateContext());

        var request = new DepositRequest()
        {
            AccountId = testAccount.Id,
            CustomerId = invalidCustomerId,
            Amount = depositAmount
        };

        var response = repository.MakeDeposit(request);

        Assert.False(response.Succeeded);
        Assert.Equal(testAccount.Id, response.AccountId);
        Assert.Equal(invalidCustomerId, response.CustomerId);

        Assert.IsType<ErrorResponse>(response);
        var errorResponse = response as ErrorResponse;
        Assert.NotNull(errorResponse);
        Assert.NotEmpty(errorResponse.Error);
    }

    [Fact]
    public void Withdrawal_NoCustomerIdMatch_CheckError() 
    {
        double withdrawalAmount = 114.00d;
        decimal openingBalance = 2399.13m;
        int invalidCustomerId = 0;

        using var resetContext = CreateContext();
        var testAccount = CreateTestAccount();
        resetContext.Attach(testAccount);

        testAccount.Balance = openingBalance;

        resetContext.SaveChanges();

        var repository = new CustomerAccountRepository(CreateContext());

        var request = new WithdrawalRequest()
        {
            AccountId = testAccount.Id,
            CustomerId = invalidCustomerId,
            Amount = withdrawalAmount
        };

        var response = repository.MakeWithdrawal(request);

        Assert.False(response.Succeeded);
        Assert.Equal(testAccount.Id, response.AccountId);
        Assert.Equal(invalidCustomerId, response.CustomerId);

        Assert.IsType<ErrorResponse>(response);
        var errorResponse = response as ErrorResponse;
        Assert.NotNull(errorResponse);
        Assert.NotEmpty(errorResponse.Error);
    }
}