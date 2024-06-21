using DataContext.Models;
using DataContext.Repositories;

namespace Tests.WebApi;

public class AccountRepositoryTests(TestDatabaseFixture Fixture) : IClassFixture<TestDatabaseFixture>
{
    private readonly AccountRepository _repository = new(Fixture.CreateContext());

    [Fact]
    public async Task MakeDeposit_InspectResponse()
    {
        decimal openingBalance = 2175.13m;
        decimal expectedBalance = 2287.13m;
        decimal depositAmount = 112;

        const long expectedAccountId = 17;
        const long expectedCustomerId = 5;
        var customer = new Customer() { Name = "Hank Rodgers" };
        var customerAccount = new CustomerAccount()
        {
            Customer = customer,
            Id = expectedAccountId,
            OpeningBalance = openingBalance
        };

        var depositResponse = await _repository.MakeDepositAsync(customerAccount, depositAmount);

        Assert.NotNull(depositResponse);

        Assert.Multiple(() =>
        {
            Assert.Equal(expectedAccountId, depositResponse.AccountId);
            Assert.Equal(expectedCustomerId, depositResponse.CustomerId);
            Assert.Equal(expectedBalance, depositResponse.Balance);
            Assert.True(depositResponse.Succeeded);
        });
    }
}