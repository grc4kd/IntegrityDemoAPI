using DataContext.Models;
using Microsoft.EntityFrameworkCore;

namespace DataContext;

public static class SeedData
{
    public static void Initialize(DbContextOptions<AccountContext> contextOptions)
    {
        using var context = new AccountContext(contextOptions);

        context.Database.EnsureCreated();

        if (context.Accounts.Any())
        {
            return; // DB has been seeded
        }

        var specialCustomer = new Customer() { Name = "Hank Dune", Id = 5 };
        var specialAccount = new CustomerAccount() { 
            Customer = specialCustomer, 
            Id = 17,
            OpeningBalance = 2399.13m
        };

        var customers = new Customer[]
        {
            new() {Name = "Frank Miranda"},
            specialCustomer
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

        // shift records and insert special test data
        if (accounts.Exists(a => a.Id == specialAccount.Id)) {
            // move element with unique ID to the last ID in seed data
            var extent = accounts.Single(a => a.Id == specialAccount.Id);
            
            accounts.Remove(extent);
            extent.Id = accounts.Max(a => a.Id) + 1;
            accounts.Add(extent);
        }
        // make sure special customer seed matches expected test data
        accounts.Add(specialAccount);

        foreach (CustomerAccount a in accounts)
        {
            context.Accounts.Add(a);
        }
        context.SaveChanges();
    }
}