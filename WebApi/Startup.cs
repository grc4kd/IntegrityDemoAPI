using DataContext;
using DataContext.Repositories;
using Microsoft.EntityFrameworkCore;

namespace WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        services.AddDbContext<CustomerAccountContext>(
            options => options.UseSqlite("name=ConnectionStrings:AccountContext",
                a => a.MigrationsAssembly("WebApi")));
        services.AddScoped<ICustomerAccountRepository, CustomerAccountRepository>();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        if (env.IsDevelopment()) {
            var context = new CustomerAccountContext(new DbContextOptionsBuilder<CustomerAccountContext>()
                .UseSqlite(Configuration.GetConnectionString("AccountContext")).Options);
            SeedDemoData seedDemoData = new(context);
            seedDemoData.SeedDemoDatabase();
        }
    }
}
