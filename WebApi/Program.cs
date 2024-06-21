using Microsoft.AspNetCore;

namespace WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        BuildWebHost(args).Run();
    }

    // EF Core uses this method at design time to access the DbContext
    public static IWebHost BuildWebHost(string[] args)
        => WebHost.CreateDefaultBuilder(args)
            .UseStartup<Startup>()
            .Build();
}