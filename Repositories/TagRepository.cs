using BookmarkManager.Data;
using BookmarkManager.Interfaces;
using BookmarkManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManager.Repositories;

public class TagRepository(AppDbContext context) : ITagRepository
{
    private readonly AppDbContext _context = context;

    /// <summary>
    /// Get all tags
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<IReadOnlyList<Tag>> GetAllAsync(
    CancellationToken cancellationToken)
    {
        return await _context.Tags
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Tag>> GetActiveByIdsAsync(
    IEnumerable<int> ids,
    CancellationToken cancellationToken = default)
    {
        var tagIds = ids
            .Distinct()
            .ToList();

        if (tagIds.Count == 0)
        {
            return [];
        }

        return await _context.Tags
            .AsNoTracking()
            .Where(t =>
                !t.IsDeleted &&
                tagIds.Contains(t.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<Tag?> GetByNameAsync(
        string name,
        CancellationToken cancellationToken = default)
    {
        var normalizedName = name.Trim().ToLower();

        return await _context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(
                t => t.Name.ToLower() == normalizedName,
                cancellationToken);
    }

    public async Task AddAsync(
        Tag tag,
        CancellationToken cancellationToken = default)
    {
        await _context.Tags.AddAsync(
            tag,
            cancellationToken);

        await _context.SaveChangesAsync(
            cancellationToken);
    }
}
