using FluentValidation;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using server_dotnet.Controllers.Validators;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Data;
using server_dotnet.Infrastructure.Repositories;
using server_dotnet.Middleware;
using server_dotnet.Services;


public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddScoped<IRepository<Organization>, OrganizationRepository>();
        builder.Services.AddScoped<IRepository<User>, UserRepository>();
        builder.Services.AddScoped<IRepository<Order>, OrderRepository>();

        builder.Services.AddScoped<IOrganizationService, OrganizationService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IOrderService, OrderService>();

        builder.Services.AddValidatorsFromAssemblyContaining<OrganizationDTOValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<OrderDTOValidator>();

        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services);
        });

        builder.Services.AddHealthChecks()
            .AddSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")!, name: "Database");

        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>(name: "DB Context");

        var app = builder.Build();

        app.MapOpenApi();
        app.UseSwaggerUi(options =>
        {
            options.DocumentPath = "/openapi/v1.json";
        });

        app.MapControllers();

        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<HttpHeadersLoggingMiddleware>();

        MapHealthCheckEndpoints(app);

        app.Run();
    }

    private static void MapHealthCheckEndpoints(WebApplication app)
    {
        app.MapHealthChecks("/health", new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(e => new
                        {
                            name = e.Key,
                            status = e.Value.Status.ToString(),
                            description = e.Value.Description
                        })
                    }));
            }
        });

        app.MapHealthChecks("/readiness", new HealthCheckOptions
        {
            Predicate = check => check.Tags.Contains("readiness"),
            ResponseWriter = async (context, report) =>
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        status = report.Status.ToString(),
                        checks = report.Entries.Select(e => new
                        {
                            name = e.Key,
                            status = e.Value.Status.ToString(),
                            description = e.Value.Description
                        })
                    }));
            }
        });
    }
}

// Created for testing purposes
public partial class Program { }