using BookmarkManager.Common;
using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Entities;

namespace BookmarkManager.Interfaces
{
    public interface IBookmarkRepository
    {
        Task<PagedResult<Bookmark>> GetPagedAsync(
            BookmarkQuery query,
            CancellationToken cancellationToken = default);
    }
}
