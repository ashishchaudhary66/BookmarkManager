using System.Security.Claims;
using BookmarkManager.Common.Enum;
using BookmarkManager.Interfaces;
using BookmarkManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        protected ActionResult ParseResult<T>(ServiceResult<T> result)
        {
            if (result.IsSuccess && result.ResponseType == ServiceErrorType.Created)
            {
                var payload = ApiResponse<T>.Ok(result.Data, result.Message);
                return Created(string.Empty, payload);
            }

            if (result.IsSuccess)
            {
                var payload = ApiResponse<T>.Ok(result.Data, result.Message);
                return Ok(payload);
            }

            // Map validation errors to simple strings when present
            IEnumerable<string>? errors = null;
            if (result.ValidationErrors != null && result.ValidationErrors.Any())
            {
                errors = result.ValidationErrors.Select(e => $"{e.Field}: {e.Message}");
            }

            var errorResponse = ApiResponse<T>.Fail(errors, result.ErrorMessage ?? result.Message);

            return result.ResponseType switch
            {
                ServiceErrorType.NotFound => NotFound(errorResponse),
                ServiceErrorType.Conflict => Conflict(errorResponse),
                ServiceErrorType.Unauthorized => Unauthorized(errorResponse),
                ServiceErrorType.Validation => BadRequest(errorResponse),
                ServiceErrorType.Forbidden => StatusCode(403, errorResponse),
                _ => StatusCode(500, errorResponse),
            };
        }
    }
}
