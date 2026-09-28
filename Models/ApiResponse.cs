namespace BookmarkManager.Models;

/// <summary>
/// Standard API response wrapper for endpoints that return data.
/// </summary>
/// <typeparam name="T">Type of the payload in Data.</typeparam>
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public IEnumerable<string>? Errors { get; set; }
    public DateTimeOffset Timestamp { get; set; }

    public ApiResponse()
    {
        Timestamp = DateTimeOffset.UtcNow;
    }

    public static ApiResponse<T> Ok(T? data, string? message = null)
    {
        return new ApiResponse<T> { Success = true, Data = data, Message = message };
    }

    public static ApiResponse<T> Fail(IEnumerable<string>? errors, string? message = null)
    {
        return new ApiResponse<T> { Success = false, Errors = errors, Message = message };
    }
}

/// <summary>
/// Standard API response wrapper for endpoints that don't return a payload.
/// </summary>
public class ApiResponse : ApiResponse<object?>
{
    public static ApiResponse SuccessResponse(string? message = null)
    {
        return new ApiResponse { Success = true, Message = message };
    }

    public static ApiResponse FailureResponse(IEnumerable<string>? errors = null, string? message = null)
    {
        return new ApiResponse { Success = false, Errors = errors, Message = message };
    }
}
