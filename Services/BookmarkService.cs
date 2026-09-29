using BookmarkManager.Common;
using BookmarkManager.Interfaces;
using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Mapper;

namespace BookmarkManager.Services
{
    public class BookmarkService(IBookmarkRepository bookmarkRepository) : IBookmarkService
    {
        private readonly IBookmarkRepository _bookmarkRepository = bookmarkRepository;

        public async Task<ServiceResult<PagedResult<BookmarkResponseDto>>> GetPagedAsync(
        BookmarkQuery query,
        CancellationToken cancellationToken = default)
        {
            var result = await _bookmarkRepository.GetPagedAsync(
                query,
                cancellationToken);

            var pagedResult = new PagedResult<BookmarkResponseDto>
            {
                Items = result.Items.ToDto().ToList(),
                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };

            return new ServiceResult<PagedResult<BookmarkResponseDto>>
            {
                IsSuccess = true,
                Data = pagedResult
            };
        }
    }
}
