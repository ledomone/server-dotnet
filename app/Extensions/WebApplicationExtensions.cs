using Microsoft.AspNetCore.Diagnostics.HealthChecks;

namespace server_dotnet.Extensions
{
    public static class WebApplicationExtensions
    {
        public static WebApplication MapHealthcheckEndpoints(this WebApplication app)
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

            return app;
        }
    }
}
