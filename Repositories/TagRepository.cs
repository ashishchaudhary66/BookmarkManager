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
    public async Task<IEnumerable<Tag>> GetTagsAsync(
    CancellationToken cancellationToken)
    {
        return await _context.Tags
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}
