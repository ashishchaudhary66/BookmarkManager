using BookmarkManager.Data;
using BookmarkManager.Interfaces;
using BookmarkManager.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManager.Repositories;

public class TagRepository(AppDbContext context) : ITagRepository
{
    // Implement methods for tag management (e.g., GetTags, AddTag, UpdateTag, DeleteTag)
    // inject the AppDbContext here for database operations
    private readonly AppDbContext _context = context;

    // Example method to get all tags
    public async Task<IEnumerable<Tag>> GetTagsAsync(
    CancellationToken cancellationToken)
    {
        return await _context.Tags
            .Where(x => !x.IsDeleted)
            .OrderBy(x => x.Name)
            .ToListAsync(cancellationToken);
    }
}
