using BookmarkManager.Interfaces;
using BookmarkManager.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TagController(
    ITagService tagService) : BaseApiController
{
    private readonly ITagService _tagService = tagService;

    // GET: /api/Tag

    [HttpGet]
    public async Task<IActionResult> GetTags(
        CancellationToken cancellationToken)
    {
        var result = await _tagService.GetAllAsync(
            cancellationToken);

        return ParseResult(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTag(
        [FromBody] TagCreateDto request,
        CancellationToken cancellationToken)
    {
        var result = await _tagService.CreateAsync(
            request,
            cancellationToken);

        return ParseResult(result);
    }
}