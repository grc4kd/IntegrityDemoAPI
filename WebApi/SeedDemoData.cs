using DataContext;
using DataContext.Models;

namespace WebApi;

public class SeedDemoData
{
    private readonly CustomerAccountContext _context;
    public SeedDemoData(CustomerAccountContext context)
    {
        _context = context;
    }

    public void SeedDemoDatabase()
    {
        // DEMO add seed data from demo project in Development environment only
        var customer = _context.Customers.Find(5);
        if (customer == null)
        {
            customer = new Customer(5);
            _context.Customers.Add(customer);
        }

        var customerAccount = _context.Accounts.Find(17);
        if (customerAccount == null)
        {
            _context.Accounts.Add(new CustomerAccount()
            {
                Id = 17,
                Customer = customer
            });
        }

        _context.SaveChanges();
    }
}