using BookmarkManager.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController(ITagService tagService) : BaseApiController
{
    // inject service here for tag management (e.g., ITagService)
    private readonly ITagService _tagService = tagService;

    // GET: api/Tags
    [HttpGet]
    public async Task<IActionResult> GetTags(CancellationToken cancellationToken)
    {
        var tags = await _tagService.GetTagsAsync(cancellationToken);
        return ParseResult(tags);
    }
}
