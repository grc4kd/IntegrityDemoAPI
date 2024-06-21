using Microsoft.EntityFrameworkCore;
using DataContext.Models;

namespace DataContext;

public class AccountContext : DbContext
{
    public AccountContext(DbContextOptions<AccountContext> options) : base(options) { }
    
    public DbSet<CustomerAccount> Accounts { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
}