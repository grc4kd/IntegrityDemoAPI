using DataContext;
using DataContext.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Tests;

public class TestDatabaseFixture
{
    private static readonly SqliteConnection _connection = new("Filename=:memory:");
    private readonly DbContextOptions<AccountContext> _contextOptions;

    public TestDatabaseFixture()
    {
        var customerId = 5;
        var accountId = 17;
        var openingBalance = 2175.13m;

        SqliteConnection _connection = new("Filename=:memory:");
        _connection.Open();

        _contextOptions = new DbContextOptionsBuilder<AccountContext>()
            .UseSqlite(_connection)
            .Options;

        using var db = new AccountContext(_contextOptions);

        if (db.Database.EnsureCreated())
        {
            var customer = new Customer() { Id = customerId, Name = "Frank Barbara" };
            db.Add(customer);

            var account = new CustomerAccount() {Customer = customer, Id = accountId, OpeningBalance = openingBalance};

            db.Add(account);
            db.SaveChanges();
        }
    }

    public AccountContext CreateContext() => new AccountContext(_contextOptions);

    public void Dispose() => _connection.Dispose();
}