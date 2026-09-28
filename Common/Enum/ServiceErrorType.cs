namespace BookmarkManager.Common.Enum;

public enum ServiceErrorType
{
    None = 200,
    Created = 201,
    NotFound = 404,
    Validation = 400,
    Conflict = 409,
    Unauthorized = 401,
    Forbidden = 403,
    InternalError = 500
}
