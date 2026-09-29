using BookmarkManager.Interfaces;
using BookmarkManager.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BookmarkController(IBookmarkService bookmarkService) : BaseApiController
{
    // inject service here for tag management (e.g., ITagService)
    private readonly IBookmarkService _bookmarkService = bookmarkService;

    // GET: api/bookmark
    [HttpGet]
    public async Task<IActionResult> GetBookmarkList([FromQuery] BookmarkQuery query, CancellationToken cancellationToken)
    {
        var bookmark = await _bookmarkService.GetPagedAsync(query, cancellationToken);
        return ParseResult(bookmark);
    }
}
