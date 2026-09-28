using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;

namespace BookmarkManager.Interfaces;

public interface ITagService
{
    /// <summary>
    /// Get all tags
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<ServiceResult<IEnumerable<TagResponseDto>>> GetTagsAsync(CancellationToken cancellationToken);
}
