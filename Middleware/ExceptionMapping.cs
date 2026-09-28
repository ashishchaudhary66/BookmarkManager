namespace BookmarkManager.Middleware
{
    internal static class ExceptionMapping
    {
        public static (int StatusCode, string? Message, IEnumerable<string>? Details) Map(Exception ex)
        {
            // Map known exception types by type name to avoid depending on domain exception types
            var typeName = ex.GetType().Name;

            return typeName switch
            {

                "NotFoundException" => (404, ex.Message, null),
                "ValidationException" => (400, ex.Message, null),
                "ConflictException" => (409, ex.Message, null),
                "UnauthorizedException" => (401, ex.Message, null),
                _ => MapFrameworkExceptions(ex)
            };
        }

        private static (int StatusCode, string? Message, IEnumerable<string>? Details) MapFrameworkExceptions(Exception ex)
        {
            if (ex is BadHttpRequestException)
            {
                return (400, ex.Message, null);
            }

            if (ex is UnauthorizedAccessException)
            {
                return (401, ex.Message, null);
            }

            if (ex is KeyNotFoundException)
            {
                return (404, ex.Message, null);
            }

            // Default: internal server error
            return (500, "An unexpected error occurred.", null);
        }
    }
}
