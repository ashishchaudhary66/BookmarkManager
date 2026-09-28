using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;

namespace BookmarkManager.Interfaces;

public interface ITagService
{
    Task<ServiceResult<IEnumerable<TagResponseDto>>> GetTagsAsync(CancellationToken cancellationToken);
}
