using System.Text.Json;
using BookmarkManager.Models;

namespace BookmarkManager.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log full exception with stack trace
            _logger.LogError(exception, "Unhandled exception occurred while processing request");

            var (statusCode, message, details) = ExceptionMapping.Map(exception);

            var includeStack = _env.IsDevelopment();

            var errors = new List<string>();
            if (details != null)
            {
                errors.AddRange(details);
            }
            else if (!string.IsNullOrWhiteSpace(exception.Message))
            {
                errors.Add(exception.Message);
            }

            if (includeStack)
            {
                errors.Add(exception.ToString());
            }

            var payload = ApiResponse<object>.Fail(errors, message ?? "An error occurred while processing the request.");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var json = JsonSerializer.Serialize(payload, options);
            await context.Response.WriteAsync(json);
        }
    }
}
