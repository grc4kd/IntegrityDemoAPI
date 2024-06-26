using DataContext;
using DataContext.Models;
using DataContext.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        builder.Services.AddScoped<ICustomerAccountRepository, CustomerAccountRepository>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddDbContext<CustomerAccountContext>(options => options.
                UseSqlite(builder.Configuration.GetConnectionString("AccountContext"),
                a => a.MigrationsAssembly("WebApi")));

            // DEMO add seed data from demo project in Development environment only
            var contextOptions = new DbContextOptionsBuilder<CustomerAccountContext>()
                .UseSqlite(builder.Configuration.GetConnectionString("AccountContext"));
            var context = new CustomerAccountContext(contextOptions.Options);

            var customer = context.Customers.Find(5);
            if (customer == null) {
                customer = new Customer(5);
                context.Customers.Add(customer);
            }
            
            var customerAccount = context.Accounts.Find(17);
            if (customerAccount == null) {
                context.Accounts.Add(new CustomerAccount() {
                    Id = 17,
                    Customer = customer
                });
            }

            context.SaveChanges();
        }
        else
        {
            builder.Services.AddDbContext<CustomerAccountContext>(options => options.
                UseSqlite(builder.Configuration.GetConnectionString("ProductionAccountContext"),
                a => a.MigrationsAssembly("WebApi")));
        }        

        var app = builder.Build();
    
        app.UseSwagger();
        app.UseSwaggerUI();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }    

        app.UseAuthorization();

        app.MapControllers();

        app.Run();   
    }
}