using BookmarkManager.Common;
using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;

namespace BookmarkManager.Interfaces
{
    public interface IBookmarkService
    {
        Task<ServiceResult<PagedResult<BookmarkResponseDto>>> GetPagedAsync(BookmarkQuery query, CancellationToken cancellationToken = default);
    }
}
