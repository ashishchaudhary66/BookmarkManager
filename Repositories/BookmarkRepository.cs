using BookmarkManager.Common;
using BookmarkManager.Common.Constants;
using BookmarkManager.Data;
using BookmarkManager.Interfaces;
using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManager.Repositories;

public class BookmarkRepository(AppDbContext dbContext) : IBookmarkRepository
{
    private readonly AppDbContext _context = dbContext;

    public async Task<PagedResult<Bookmark>> GetPagedAsync(
        BookmarkQuery query,
        CancellationToken cancellationToken = default)
    {
        IQueryable<Bookmark> bookmarks = _context.Bookmarks
            .AsNoTracking()
            .Include(b => b.BookmarkTags)
            .ThenInclude(bt => bt.Tag);

        // Search by title or URL
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            var pattern = Constants.Pattern(search);

            bookmarks = bookmarks.Where(b =>
                (b.Title != null &&
                 EF.Functions.Like(b.Title, pattern)) ||
                EF.Functions.Like(b.Url, pattern));
        }

        // Filter by tag - case-insensitive partial match
        if (!string.IsNullOrWhiteSpace(query.Tag))
        {
            var tag = query.Tag.Trim();
            var pattern = Constants.Pattern(tag);

            bookmarks = bookmarks.Where(b =>
                b.BookmarkTags.Any(bt =>
                    EF.Functions.Like(bt.Tag.Name, pattern)));
        }
        // Newest first
        bookmarks = bookmarks
            .OrderByDescending(b => b.CreatedAt);

        return await bookmarks.ToPagedResultAsync(
            query.Page,
            query.PageSize,
            cancellationToken);
    }
}