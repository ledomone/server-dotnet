using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using server_dotnet.Controllers.Validators;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Data;
using server_dotnet.Infrastructure.Repositories;
using server_dotnet.Services;
using System.Text;
using System.Threading.RateLimiting;

namespace server_dotnet.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IRepository<Organization>, OrganizationRepository>();
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Order>, OrderRepository>();

            return services;
        }

        public static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<OrganizationDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<UserDTOValidator>();
            services.AddValidatorsFromAssemblyContaining<OrderDTOValidator>();

            return services;
        }

        public static IServiceCollection AddDbHealthChecks(this IServiceCollection services, string connectionString)
        {
            services.AddHealthChecks()
            .AddSqlServer(connectionString, name: "Database");
            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>(name: "DB Context");

            return services;
        }

        public static IServiceCollection AddRateLimiters(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("OrganizationGetLimit", context =>
                {
                    var organizationId = context.Request.RouteValues["id"]?.ToString() ?? "anonymous";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: organizationId,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 30,
                            Window = TimeSpan.FromMinutes(1),
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                            QueueLimit = 0,
                            AutoReplenishment = true
                        });
                });

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    await context.HttpContext.Response.WriteAsync(
                        "Too many requests for this organization. Try again later.", cancellationToken);
                };
            });

            return services;
        }

        public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "bearerAuth"
                            }
                        },
                        new string[] {}
                    }
                });
            });
            return services;
        }
    }
}
