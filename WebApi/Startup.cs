using DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

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
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        if (Environment.IsDevelopment())
        {
            services.AddDbContext<AccountContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("AccountContext"),
                    a => a.MigrationsAssembly("WebApi")));
        }
        else
        {
            services.AddDbContext<AccountContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("ProductionAccountContext"),
                    a => a.MigrationsAssembly("WebApi")));
        }
    }

    public void Configure(IApplicationBuilder app)
    {
        if (Environment.IsDevelopment())
        {
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