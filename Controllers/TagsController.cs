using BookmarkManager.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookmarkManager.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagsController(ITagService tagService) : BaseApiController
{
    private readonly ITagService _tagService = tagService;

    /// <summary>
    /// Get all tags
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> GetTags(CancellationToken cancellationToken)
    {
        var tags = await _tagService.GetTagsAsync(cancellationToken);
        return ParseResult(tags);
    }
}
