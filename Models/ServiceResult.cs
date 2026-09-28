using BookmarkManager.Common.Enum;

namespace BookmarkManager.Models;

public record ServiceResult<T>(bool IsSuccess, T? Data, ServiceErrorType ResponseType, string? ErrorMessage, List<ValidationError>? ValidationErrors, string? Message)
{
    public Dictionary<string, object?>? Extensions { get; init; }

    public ServiceResult()
        : this(IsSuccess: false, default(T), ServiceErrorType.None, (string?)null, (List<ValidationError>?)null, (string?)null)
    {
    }

    public static ServiceResult<T> Success(T data, string? message = null)
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Data = data,
            ResponseType = ServiceErrorType.None,
            Message = message
        };
    }

    public static ServiceResult<T> Created(T data)
    {
        return new ServiceResult<T>
        {
            IsSuccess = true,
            Data = data,
            ResponseType = ServiceErrorType.Created
        };
    }

    public static ServiceResult<T> NotFound(string message = "Resource not found")
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            ResponseType = ServiceErrorType.NotFound,
            ErrorMessage = message
        };
    }

    public static ServiceResult<T> Conflict(string error)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            ResponseType = ServiceErrorType.Conflict,
            ErrorMessage = error
        };
    }

    public static ServiceResult<T> Failure(string error)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            ResponseType = ServiceErrorType.InternalError,
            ErrorMessage = error
        };
    }

    public static ServiceResult<T> Unauthorized(string message = "Unauthorized access")
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            ResponseType = ServiceErrorType.Unauthorized,
            ErrorMessage = message
        };
    }

    public static ServiceResult<T> Forbidden(string message = "Access denied")
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            ResponseType = ServiceErrorType.Forbidden,
            ErrorMessage = message
        };
    }

    public static ServiceResult<T> ValidationFailure(List<ValidationError> errors)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            ResponseType = ServiceErrorType.Validation,
            ErrorMessage = "One or more validation errors occurred.",
            ValidationErrors = errors
        };
    }

    public static ServiceResult<T> ValidationFailure(string field, string error)
    {
        return ValidationFailure(new List<ValidationError>
        {
            new ValidationError(field, error)
        });
    }

    public static ServiceResult<T> Conflict(string error, List<ValidationError> validationErrors)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            ResponseType = ServiceErrorType.Conflict,
            ErrorMessage = error,
            ValidationErrors = validationErrors
        };
    }

    public static ServiceResult<T> NotFound(string error, List<ValidationError> validationErrors)
    {
        return new ServiceResult<T>
        {
            IsSuccess = false,
            ResponseType = ServiceErrorType.NotFound,
            ErrorMessage = error,
            ValidationErrors = validationErrors
        };
    }
}
