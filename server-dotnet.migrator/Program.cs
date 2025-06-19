using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using server_dotnet.Infrastructure.Data;

public class Migrator
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args);
        builder.ConfigureServices((hostContext, services) =>
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection")));
        });

        using var host = builder.Build();
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        Console.WriteLine("Applying database migrations...");
        context.Database.Migrate();
        Console.WriteLine("Migrations applied successfully.");
    }
}
