using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;

namespace BookmarkManager.Interfaces;

public interface ITagService
{
    Task<ServiceResult<IReadOnlyList<TagResponseDto>>> GetAllAsync(
        CancellationToken cancellationToken = default);
}
