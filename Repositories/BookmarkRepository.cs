using BookmarkManager.Common;
using BookmarkManager.Common.Constants;
using BookmarkManager.Data;
using BookmarkManager.Interfaces;
using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManager.Repositories;

public class BookmarkRepository(AppDbContext dbContext)
    : IBookmarkRepository
{
    private readonly AppDbContext _context = dbContext;

    public async Task<PagedResult<Bookmark>> GetPagedAsync(
        BookmarkQuery query,
        CancellationToken cancellationToken = default)
    {
        query.Normalize();

        IQueryable<Bookmark> bookmarks = _context.Bookmarks
            .AsNoTracking()
            .Where(b => !b.IsDeleted)
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

        // Filter by tag
        if (!string.IsNullOrWhiteSpace(query.Tag))
        {
            var tag = query.Tag.Trim();
            var pattern = Constants.Pattern(tag);

            bookmarks = bookmarks.Where(b =>
                b.BookmarkTags.Any(bt =>
                    !bt.IsDeleted &&
                    !bt.Tag.IsDeleted &&
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

    public async Task<Bookmark?> GetByIdReadOnlyAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        return await _context.Bookmarks
            .AsNoTracking()
            .Include(b => b.BookmarkTags)
            .ThenInclude(bt => bt.Tag)
            .FirstOrDefaultAsync(
                b => b.Id == id,
                cancellationToken);
    }

    public async Task<Bookmark?> GetByIdAsync(
    int id,
    CancellationToken cancellationToken = default)
    {
        return await _context.Bookmarks
            .Include(b => b.BookmarkTags)
            .ThenInclude(bt => bt.Tag)
            .FirstOrDefaultAsync(
                b => b.Id == id,
                cancellationToken);
    }

    public async Task<Bookmark?> GetByNormalizedUrlAsync(
        string normalizedUrl,
        CancellationToken cancellationToken = default)
    {
        return await _context.Bookmarks
            .FirstOrDefaultAsync(
                b => b.NormalizedUrl == normalizedUrl,
                cancellationToken);
    }

    public async Task AddAsync(
        Bookmark bookmark,
        CancellationToken cancellationToken = default)
    {
        await _context.Bookmarks.AddAsync(
            bookmark,
            cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(
        Bookmark bookmark,
        CancellationToken cancellationToken = default)
    {
        _context.Bookmarks.Update(bookmark);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(
        Bookmark bookmark,
        CancellationToken cancellationToken = default)
    {
        bookmark.IsDeleted = true;
        bookmark.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}