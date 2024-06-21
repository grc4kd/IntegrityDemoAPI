using DataContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataContext;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new AccountContext(
            serviceProvider.GetRequiredService<DbContextOptions<AccountContext>>());

        if (context.Accounts.Any())
        {
            return; // DB has been seeded
        }

        var customers = new Customer[]
        {
            new() {Name = "Frank Miranda"}
        };

        foreach (Customer c in customers)
        {
            context.Customers.Add(c);
        }
        context.SaveChanges();

        var accounts = new List<CustomerAccount>();

        foreach (Customer c in customers)
        {
            accounts.Add(new CustomerAccount() { Customer = c });
        }

        foreach (CustomerAccount a in accounts)
        {
            context.Accounts.Add(a);
        }
        context.SaveChanges();
    }
}