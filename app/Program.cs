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

        builder.Services.AddRateLimiters();

        builder.Services.AddControllers(options =>
        {
            // Global authorization filter
            options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter());
        });
        builder.Services.AddResponseCaching();

        builder.Services.AddSwagger();

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
        builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

        var app = builder.Build();

        app.UseRateLimiter();
        app.UseResponseCaching();
        
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.UseMiddleware<ErrorHandlingMiddleware>();
        app.UseMiddleware<HttpHeadersLoggingMiddleware>();

        app.MapHealthcheckEndpoints();
        app.Run();
    }
}

// Created for testing purposes
public partial class Program { }