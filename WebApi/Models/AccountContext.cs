using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Models;

public class AccountContext(DbContextOptions<AccountContext> options) : DbContext(options)
{
    public DbSet<CustomerAccount> accounts { get; set;} = null!;
}