using Microsoft.EntityFrameworkCore;
using Serilog;
using server_dotnet.Extensions;
using server_dotnet.Infrastructure.Data;
using server_dotnet.Middleware;


public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers();
        builder.Services.AddResponseCaching();
        builder.Services.AddOpenApi();

        builder.Services.AddRepositories();
        builder.Services.AddAppServices();
        builder.Services.AddValidators();

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services);
        });

        builder.Services.AddDbHealthChecks(builder.Configuration.GetConnectionString("DefaultConnection")!);

        var app = builder.Build();

        app.UseResponseCaching();
        app.MapOpenApi();
        app.UseSwaggerUi(options =>
        {
            options.DocumentPath = "/openapi/v1.json";
        });

        app.MapControllers();

        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<HttpHeadersLoggingMiddleware>();

        app.MapHealthcheckEndpoints();
        app.Run();
    }
}

// Created for testing purposes
public partial class Program { }