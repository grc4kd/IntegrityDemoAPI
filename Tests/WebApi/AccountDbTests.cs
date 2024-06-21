namespace Tests.WebApi;

public class AccountDbTests(TestDatabaseFixture fixture) : IClassFixture<TestDatabaseFixture>
{
    private readonly TestDatabaseFixture Fixture = fixture;
    private const long expectedAccountId = 17;

    [Fact]
    public async void AccountDb_ExpectedId()
    {
        await using var db = Fixture.CreateContext();

        Assert.Equal(expectedAccountId, db.Accounts.First().Id);
    }
}