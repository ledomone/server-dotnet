using Microsoft.EntityFrameworkCore;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Data;
using server_dotnet.Infrastructure.Repositories;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            options.UseInMemoryDatabase("TestDatabase"));

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();
        builder.Services.AddScoped<IRepository<Organization>, OrganizationRepository>();
        builder.Services.AddScoped<IRepository<User>, UserRepository>();
        builder.Services.AddScoped<IRepository<Order>, OrderRepository>();

        var app = builder.Build();

        app.MapOpenApi();
        app.UseSwaggerUi(options =>
        {
            options.DocumentPath = "/openapi/v1.json";
        });

        app.MapControllers();

        app.MapGet("/health", () => Results.Ok(new { status = "UP" }));

        app.Run();
    }
}

// Created for testing purposes
public partial class Program { }