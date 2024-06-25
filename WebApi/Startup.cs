using DataContext;
using DataContext.Models;
using DataContext.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    public Startup(IConfiguration configuration, IWebHostEnvironment env)
    {
        Environment = env;
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllersWithViews();
        services.AddScoped<ICustomerAccountRepository, CustomerAccountRepository>();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        if (Environment.IsDevelopment())
        {
            services.AddDbContext<CustomerAccountContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("AccountContext"),
                    a => a.MigrationsAssembly("WebApi")));
        }
        else
        {
            services.AddDbContext<CustomerAccountContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ProductionAccountContext"),
                    a => a.MigrationsAssembly("WebApi")));
        }
    }

    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
            // DEMO add seed data from demo project in Development environment only
            var contextOptions = new DbContextOptionsBuilder<CustomerAccountContext>()
                .UseSqlite(Configuration.GetConnectionString("AccountContext"));
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

            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        
        app.UseEndpoints(endpoints => 
        {
            endpoints.MapControllers();
        });
    }
}