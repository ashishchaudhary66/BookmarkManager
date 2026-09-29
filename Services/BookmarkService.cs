using BookmarkManager.Common;
using BookmarkManager.Common.Enum;
using BookmarkManager.Interfaces;
using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Entities;
using BookmarkManager.Models.Mapper;
using BookmarkManager.Repositories;

namespace BookmarkManager.Services
{
    public class BookmarkService(IBookmarkRepository bookmarkRepository, IPageTitleService pageTitleService, ITagRepository tagRepository) : IBookmarkService
    {
        private readonly IBookmarkRepository _bookmarkRepository = bookmarkRepository;
        private readonly IPageTitleService _pageTitleService = pageTitleService;
        private readonly ITagRepository _tagRepository = tagRepository;

        public async Task<ServiceResult<PagedResult<BookmarkResponseDto>>> GetPagedAsync(
            BookmarkQuery query,
            CancellationToken cancellationToken = default)
        {
            var result = await _bookmarkRepository.GetPagedAsync(
                query,
                cancellationToken);

            var data = new PagedResult<BookmarkResponseDto>
            {
                Items = result.Items
                    .ToDto()
                    .ToList(),

                Page = result.Page,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages
            };

            return ServiceResult<PagedResult<BookmarkResponseDto>>.Success(
                data);
        }

        public async Task<ServiceResult<BookmarkResponseDto>> GetByIdAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var bookmark = await _bookmarkRepository.GetByIdReadOnlyAsync(
                id,
                cancellationToken);

            if (bookmark == null)
            {
                return ServiceResult<BookmarkResponseDto>.NotFound("Bookmark not found.");
            }

            return ServiceResult<BookmarkResponseDto>.Success(
                bookmark.ToDto(),
                "Bookmark retrieved successfully.");
        }

        public async Task<ServiceResult<BookmarkResponseDto>> CreateAsync(
            BookmarkCreateDto request,
            CancellationToken cancellationToken = default)
        {
            // Validate URL
            if (!Uri.TryCreate(
                    request.Url?.Trim(),
                    UriKind.Absolute,
                    out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp &&
                 uri.Scheme != Uri.UriSchemeHttps))
            {
                return ServiceResult<BookmarkResponseDto>.ValidationFailure(
                    "Url",
                    "Please provide a valid HTTP or HTTPS URL.");
            }

            var normalizedUrl = uri.AbsoluteUri
                .TrimEnd('/')
                .ToLowerInvariant();

            // Duplicate check
            var existing = await _bookmarkRepository.GetByNormalizedUrlAsync(
                normalizedUrl,
                cancellationToken);

            if (existing != null)
            {
                return ServiceResult<BookmarkResponseDto>.Conflict(
                    "A bookmark with this URL already exists.");
            }

            // Validate tags
            var tagIds = request.TagIds?
                .Where(id => id > 0)
                .Distinct()
                .ToList()
                ?? [];

            var tags = await _tagRepository.GetActiveByIdsAsync(
                tagIds,
                cancellationToken);

            if (tags.Count != tagIds.Count)
            {
                var foundTagIds = tags
                    .Select(t => t.Id)
                    .ToHashSet();

                var invalidTagIds = tagIds
                    .Where(id => !foundTagIds.Contains(id))
                    .ToList();

                return ServiceResult<BookmarkResponseDto>.ValidationFailure(
                    "TagIds",
                    $"The following tag IDs are invalid or deleted: {string.Join(", ", invalidTagIds)}");
            }

            // Title
            var title = request.Title?.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                title = await _pageTitleService.TryGetTitleAsync(
                    uri,
                    cancellationToken);
            }

            var bookmark = new Bookmark
            {
                Url = uri.AbsoluteUri,
                NormalizedUrl = normalizedUrl,
                Title = title,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Associate existing tags using FK only
            foreach (var tag in tags)
            {
                bookmark.BookmarkTags.Add(new BookmarkTag
                {
                    TagId = tag.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                });
            }

            await _bookmarkRepository.AddAsync(
                bookmark,
                cancellationToken);

            // Reload bookmark with Tag relationships
            var createdBookmark = await _bookmarkRepository.GetByIdReadOnlyAsync(
                bookmark.Id,
                cancellationToken);

            if (createdBookmark == null)
            {
                return ServiceResult<BookmarkResponseDto>.Failure(
                    "Unable to retrieve the created bookmark.");
            }

            return ServiceResult<BookmarkResponseDto>.Created(
                createdBookmark.ToDto());
        }

        public async Task<ServiceResult<BookmarkResponseDto>> UpdateAsync(
            int id,
            BookmarkUpdateDto request,
            CancellationToken cancellationToken = default)
        {
            // 1. Get tracked bookmark
            var bookmark = await _bookmarkRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (bookmark == null)
            {
                return ServiceResult<BookmarkResponseDto>.NotFound(
                    "Bookmark not found.");
            }

            // 2. Validate URL
            if (!Uri.TryCreate(
                    request.Url?.Trim(),
                    UriKind.Absolute,
                    out var uri) ||
                (uri.Scheme != Uri.UriSchemeHttp &&
                 uri.Scheme != Uri.UriSchemeHttps))
            {
                return ServiceResult<BookmarkResponseDto>.ValidationFailure(
                    "Url",
                    "Please provide a valid HTTP or HTTPS URL.");
            }

            // 3. Normalize URL
            var normalizedUrl = uri.AbsoluteUri
                .TrimEnd('/')
                .ToLowerInvariant();

            // 4. Duplicate check
            var existing = await _bookmarkRepository
                .GetByNormalizedUrlAsync(
                    normalizedUrl,
                    cancellationToken);

            if (existing != null && existing.Id != id)
            {
                return ServiceResult<BookmarkResponseDto>.Conflict(
                    "A bookmark with this URL already exists.");
            }

            // 5. Validate Tag IDs
            var tagIds = request.TagIds?
                .Where(tagId => tagId > 0)
                .Distinct()
                .ToList()
                ?? [];

            var tags = await _tagRepository.GetActiveByIdsAsync(
                tagIds,
                cancellationToken);

            if (tags.Count != tagIds.Count)
            {
                var foundTagIds = tags
                    .Select(t => t.Id)
                    .ToHashSet();

                var invalidTagIds = tagIds
                    .Where(tagId => !foundTagIds.Contains(tagId))
                    .ToList();

                return ServiceResult<BookmarkResponseDto>.ValidationFailure(
                    "TagIds",
                    $"The following tag IDs are invalid or deleted: " +
                    $"{string.Join(", ", invalidTagIds)}");
            }

            // 6. Get title
            var title = request.Title?.Trim();

            if (string.IsNullOrWhiteSpace(title))
            {
                title = await _pageTitleService.TryGetTitleAsync(
                    uri,
                    cancellationToken);
            }

            // 7. Update bookmark properties
            bookmark.Url = uri.AbsoluteUri;
            bookmark.NormalizedUrl = normalizedUrl;
            bookmark.Title = title;
            bookmark.UpdatedAt = DateTime.UtcNow;

            // 8. Update tags
            var requestedTagIds = tagIds.ToHashSet();

            var existingLinks = bookmark.BookmarkTags
                .ToList();

            // Soft-delete removed tags
            foreach (var link in existingLinks)
            {
                if (!requestedTagIds.Contains(link.TagId))
                {
                    link.IsDeleted = true;
                    link.UpdatedAt = DateTime.UtcNow;
                }
            }

            // Add or restore selected tags
            foreach (var tagId in requestedTagIds)
            {
                var existingLink = existingLinks
                    .FirstOrDefault(x => x.TagId == tagId);

                if (existingLink != null)
                {
                    // Restore previously deleted relationship
                    existingLink.IsDeleted = false;
                    existingLink.UpdatedAt = DateTime.UtcNow;
                }
                else
                {
                    bookmark.BookmarkTags.Add(new BookmarkTag
                    {
                        BookmarkId = bookmark.Id,
                        TagId = tagId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        IsDeleted = false
                    });
                }
            }

            // 9. Save tracked changes
            await _bookmarkRepository.UpdateAsync(
                bookmark,
                cancellationToken);

            // 10. Reload to get complete Tag navigation objects
            var updatedBookmark = await _bookmarkRepository.GetByIdReadOnlyAsync(
                id,
                cancellationToken);

            if (updatedBookmark == null)
            {
                return ServiceResult<BookmarkResponseDto>.NotFound(
                    "Bookmark could not be retrieved after update.");
            }

            return ServiceResult<BookmarkResponseDto>.Success(
                updatedBookmark.ToDto(),
                "Bookmark updated successfully.");
        }

        public async Task<ServiceResult<bool>> DeleteAsync(
            int id,
            CancellationToken cancellationToken = default)
        {
            var bookmark = await _bookmarkRepository.GetByIdAsync(
                id,
                cancellationToken);

            if (bookmark == null)
            {
                return ServiceResult<bool>.NotFound(
                    "Bookmark not found.");
            }

            bookmark.IsDeleted = true;
            bookmark.UpdatedAt = DateTime.UtcNow;

            await _bookmarkRepository.UpdateAsync(
                bookmark,
                cancellationToken);

            return ServiceResult<bool>.Success(
                true,
                "Bookmark deleted successfully.");
        }
    }
}
