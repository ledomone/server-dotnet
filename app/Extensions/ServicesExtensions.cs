using FluentValidation;
using server_dotnet.Controllers.Validators;
using server_dotnet.Domain.Entities;
using server_dotnet.Infrastructure.Data;
using server_dotnet.Infrastructure.Repositories;
using server_dotnet.Services;

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
    }
}
