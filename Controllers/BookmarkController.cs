using BookmarkManager.Common;
using BookmarkManager.Interfaces;
using BookmarkManager.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookmarkController(
    IBookmarkService bookmarkService) : BaseApiController
{
    private readonly IBookmarkService _bookmarkService = bookmarkService;

    // GET: /api/Bookmark
    // GET: /api/Bookmark?Page=1&PageSize=10
    // GET: /api/Bookmark?Search=ef%20core
    // GET: /api/Bookmark?Tag=development
    // GET: /api/Bookmark?Search=ef&Tag=development&Page=1&PageSize=10

    [HttpGet]
    public async Task<IActionResult> GetBookmarkList(
        [FromQuery] BookmarkQuery query,
        CancellationToken cancellationToken)
    {
        var result = await _bookmarkService.GetPagedAsync(
            query,
            cancellationToken);

        return ParseResult<PagedResult<BookmarkResponseDto>>(result);
    }

    // GET: /api/Bookmark/10

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookmark(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _bookmarkService.GetByIdAsync(
            id,
            cancellationToken);

        return ParseResult(result);
    }

    // POST: /api/Bookmark

    [HttpPost]
    public async Task<IActionResult> CreateBookmark(
        [FromBody] BookmarkCreateDto request,
        CancellationToken cancellationToken)
    {
        var result = await _bookmarkService.CreateAsync(
            request,
            cancellationToken);

        return ParseResult(result);
    }

    // PUT: /api/Bookmark/10

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBookmark(
        int id,
        [FromBody] BookmarkUpdateDto request,
        CancellationToken cancellationToken)
    {
        var result = await _bookmarkService.UpdateAsync(
            id,
            request,
            cancellationToken);

        return ParseResult(result);
    }

    // DELETE: /api/Bookmark/10

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBookmark(
        int id,
        CancellationToken cancellationToken)
    {
        var result = await _bookmarkService.DeleteAsync(
            id,
            cancellationToken);

        return ParseResult(result);
    }
}