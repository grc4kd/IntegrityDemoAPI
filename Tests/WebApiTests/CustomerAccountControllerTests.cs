using DataContext.Repositories;
using DataContext.Requests;
using DataContext.Responses;
using Moq;
using WebApi.Controllers;

namespace Tests.WebApiTests;

public class CustomerAccountControllerTests
{
    [Fact]
    public void AccountController_MakeWithdrawal_CheckBalance()
    {
        double withdrawalAmount = 100.00d;
        var testCustomerAccount = CreateTestCustomerAccount();

        var withdrawalRequest = new WithdrawalRequest()
        {
            CustomerId = testCustomerAccount.Customer.Id,
            AccountId = testCustomerAccount.Id,
            Amount = withdrawalAmount
        };

        decimal expectedBalance = 999900.00m;
        
        var repositoryMock = new Mock<ICustomerAccountRepository>();
        repositoryMock.Setup(r => r.MakeWithdrawal(withdrawalRequest))
            .Returns(new WithdrawalResponse(
                withdrawalRequest.CustomerId,
                withdrawalRequest.AccountId,
                Balance: expectedBalance,
                true
            ));
        repositoryMock.Setup(r => r.GetCustomerAccount(testCustomerAccount.Id))
            .Returns(new DataContext.Models.CustomerAccount() {
                AccountStatus = Core.Models.AccountStatusCode.OPEN.ToString(),
                AccountTypeId = Core.Models.AccountType.AccountTypeId("Savings"),
                Balance = expectedBalance,
                Customer = testCustomerAccount.Customer,
                Id = testCustomerAccount.Id
            });

        var controller = new CustomerAccountController(repositoryMock.Object);

        var response = controller.MakeWithdrawal(withdrawalRequest);

        var getCustomerAccount = controller.GetCustomerAccount(testCustomerAccount.Id);

        Assert.NotNull(getCustomerAccount);
        Assert.Equal(expectedBalance, getCustomerAccount.Balance);

        Assert.IsType<WithdrawalResponse>(response);
        var withdrawalResponse = response as WithdrawalResponse;
        Assert.NotNull(withdrawalResponse);
        Assert.Equal(expectedBalance, withdrawalResponse.Balance);
    }

    [Fact]
    public void AccountController_MakeDeposit_CheckBalance()
    {
        double depositAmount = 100.00d;
        decimal expectedBalance = 1000100.00m;

        var testCustomerAccount = CreateTestCustomerAccount();

        var depositRequest = new DepositRequest()
        {
            CustomerId = testCustomerAccount.Customer.Id,
            AccountId = testCustomerAccount.Id,
            Amount = depositAmount
        };

        var repositoryMock = new Mock<ICustomerAccountRepository>();
        repositoryMock.Setup(r => r.MakeDeposit(depositRequest))
            .Returns(new DepositResponse(
                depositRequest.CustomerId,
                depositRequest.AccountId,
                Balance: expectedBalance,
                Succeeded: true
            ));

        repositoryMock.Setup(r => r.GetCustomerAccount(testCustomerAccount.Id))
            .Returns(new DataContext.Models.CustomerAccount() {
                AccountStatus = Core.Models.AccountStatusCode.OPEN.ToString(),
                AccountTypeId = Core.Models.AccountType.AccountTypeId("Savings"),
                Balance = expectedBalance,
                Customer = testCustomerAccount.Customer,
                Id = testCustomerAccount.Id
            });

        var controller = new CustomerAccountController(repositoryMock.Object);

        var response = controller.MakeDeposit(depositRequest);

        var getCustomerAccount = controller.GetCustomerAccount(testCustomerAccount.Id);

        Assert.NotNull(getCustomerAccount);
        Assert.Equal(expectedBalance, getCustomerAccount.Balance);

        Assert.IsType<DepositResponse>(response);
        var depositResponse = response as DepositResponse;
        Assert.NotNull(depositResponse);
        Assert.Equal(expectedBalance, depositResponse.Balance);
    }

    [Fact]
    public void AccountController_TestAccount()
    {
        double initialDeposit = 1000000.00;
        double withdrawalAmount = 1000000.00;
        decimal expectedBalance = 0.00m;

        var testCustomerAccount = CreateTestCustomerAccount();

        var openAccountRequest = new OpenAccountRequest()
        {
            AccountTypeId = Core.Models.AccountType.AccountTypeId("Savings"),
            CustomerId = testCustomerAccount.Customer.Id,
            InitialDeposit = initialDeposit
        };

        var repositoryMock = new Mock<ICustomerAccountRepository>();
        repositoryMock.Setup(r => r.OpenCustomerAccount(openAccountRequest))
            .Returns(new OpenAccountResponse(
                testCustomerAccount.Customer.Id,
                testCustomerAccount.Id,
                Succeeded: true,
                testCustomerAccount.AccountTypeId,
                Balance: (decimal)initialDeposit
            ));

        repositoryMock.Setup(r => r.GetCustomerAccount(testCustomerAccount.Id))
            .Returns(new DataContext.Models.CustomerAccount() {
                AccountStatus = Core.Models.AccountStatusCode.OPEN.ToString(),
                AccountTypeId = Core.Models.AccountType.AccountTypeId("Savings"),
                Balance = expectedBalance,
                Customer = testCustomerAccount.Customer,
                Id = testCustomerAccount.Id
            });

        var controller = new CustomerAccountController(repositoryMock.Object);

        var openAccountResponse = controller.OpenCustomerAccount(openAccountRequest);

        var customerAccount = controller.GetCustomerAccount(testCustomerAccount.Id);

        var withdrawalRequest = new WithdrawalRequest() {
            AccountId = customerAccount!.Id,
            CustomerId = customerAccount.Customer.Id,
            Amount = withdrawalAmount
        };

        repositoryMock.Setup(r => r.MakeWithdrawal(withdrawalRequest))
            .Returns(new WithdrawalResponse(customerAccount));

        var withdrawalAccountResponse = controller.MakeWithdrawal(withdrawalRequest);

        var closeAccountRequest = new CloseAccountRequest() {
            AccountId = customerAccount.Id,
            CustomerId = customerAccount.Customer.Id
        };

        repositoryMock.Setup(r => r.CloseCustomerAccount(closeAccountRequest))
            .Returns(new CloseAccountResponse(customerAccount));

        var closeAccountResponse = controller.CloseCustomerAccount(closeAccountRequest);

        Assert.NotNull(customerAccount);

        Assert.True(openAccountResponse.Succeeded);
        Assert.Equal((decimal)initialDeposit, openAccountResponse.Balance);
        Assert.Equal(testCustomerAccount.Customer.Id, openAccountResponse.CustomerId);
        Assert.Equal(testCustomerAccount.Id, openAccountResponse.AccountId);

        Assert.Equal(testCustomerAccount.Customer.Id, withdrawalAccountResponse.CustomerId);
        Assert.Equal(testCustomerAccount.Id, withdrawalAccountResponse.AccountId);
        Assert.True(withdrawalAccountResponse.Succeeded);

        Assert.IsType<WithdrawalResponse>(withdrawalAccountResponse);
        var withdrawalResponse = withdrawalAccountResponse as WithdrawalResponse;
        Assert.NotNull(withdrawalResponse);
        Assert.Equal(expectedBalance, withdrawalResponse.Balance);

        Assert.True(closeAccountResponse.Succeeded);
        Assert.Equal(testCustomerAccount.Customer.Id, closeAccountResponse.CustomerId);
        Assert.Equal(testCustomerAccount.Id, closeAccountResponse.AccountId);
    }

    private static DataContext.Models.CustomerAccount CreateTestCustomerAccount()
    {
        var customer = new DataContext.Models.Customer() {
            Id = 17,
            Name = ""
        };
        var customerAccount = new DataContext.Models.CustomerAccount()
        {
            Id = 27,
            AccountStatus = Core.Models.AccountStatusCode.OPEN.ToString(),
            AccountTypeId = Core.Models.AccountType.AccountTypeId("Savings"),
            Balance = 1000000.00m,
            Customer = customer
        };

        return customerAccount;
    }
}