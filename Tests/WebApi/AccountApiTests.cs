using Core.Models;
using Core.Repositories;
using Moq;

namespace Tests.WebApi;

public class AccountApiTests
{
    private const long expectedAccountId = 17;
    private static readonly Customer customer = new("Bank of America");

    [Fact]
    public void DepositAmount_Response_Balance()
    {
        var expectedBalance = 2287.13m;

        decimal amount = 112;
        var openingBalance = 2175.13m;
        var deposit = new Deposit(amount);

        var customerAccount = new CustomerAccount(customer, openingBalance);

        var mockAccountSet = new Mock<IAccountSet>();
        mockAccountSet.Setup(mock => mock.GetAccount(expectedAccountId)).Returns(customerAccount);

        var accountRepository = new AccountRepository(mockAccountSet.Object);

        var depositResponse = accountRepository.MakeDeposit(customerAccount, amount);

        Assert.Equal(expectedBalance, depositResponse.Balance);
    }
}