using BookmarkManager.Common;
using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;

namespace BookmarkManager.Interfaces
{
    public interface IBookmarkService
    {
        Task<ServiceResult<PagedResult<BookmarkResponseDto>>> GetPagedAsync(
            BookmarkQuery query,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<BookmarkResponseDto>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<BookmarkResponseDto>> CreateAsync(
            BookmarkCreateDto request,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<BookmarkResponseDto>> UpdateAsync(
            int id,
            BookmarkUpdateDto request,
            CancellationToken cancellationToken = default);

        Task<ServiceResult<bool>> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default);
    }
}
