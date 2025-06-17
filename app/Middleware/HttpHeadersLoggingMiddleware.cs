namespace server_dotnet.Middleware
{
    public class HttpHeadersLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HttpHeadersLoggingMiddleware> _logger;

        public HttpHeadersLoggingMiddleware(RequestDelegate next, ILogger<HttpHeadersLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log request headers
            _logger.LogDebug("Request Headers:");
            foreach (var header in context.Request.Headers)
            {
                _logger.LogDebug("{Header}: {Value}", header.Key, header.Value);
            }

            // Call the next middleware in the pipeline
            await _next(context);

            // Log response headers
            _logger.LogDebug("Response Headers:");
            foreach (var header in context.Response.Headers)
            {
                _logger.LogDebug("{Header}: {Value}", header.Key, header.Value);
            }
        }
    }
}
