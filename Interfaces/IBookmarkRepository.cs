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

        Task<Bookmark?> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<Bookmark?> GetByNormalizedUrlAsync(
            string normalizedUrl,
            CancellationToken cancellationToken = default);

        Task AddAsync(
            Bookmark bookmark,
            CancellationToken cancellationToken = default);

        Task UpdateAsync(
            Bookmark bookmark,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            Bookmark bookmark,
            CancellationToken cancellationToken = default);
    }
}
