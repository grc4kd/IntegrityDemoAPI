using DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DataContext;

public class SeedData
{
    private static readonly Random random = new(517552500);

    public static void Initialize(DbContextOptions<CustomerAccountContext> contextOptions)
    {
        using var context = new CustomerAccountContext(contextOptions);

        context.Database.EnsureCreated();

        if (context.Accounts.Any())
        {
            return; // DB has been seeded
        }

        var customers = new Customer[]
        {
            new() {Name = "Frank Miranda"},
            new() {Name = "Hank Dune"}
        };

        foreach (Customer c in customers)
        {
            context.Customers.Add(c);
        }

        var accounts = new List<CustomerAccount>();
        foreach (Customer c in customers)
        {
            context.Accounts.Add(new CustomerAccount{
                Customer = c, 
                Balance = random.Next(100, 1000000000)
            });
        }

        context.SaveChanges();
    }
}