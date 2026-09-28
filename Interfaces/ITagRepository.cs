using BookmarkManager.Models.Entities;

namespace BookmarkManager.Interfaces;

public interface ITagRepository
{
    /// <summary>
    /// Get all tags
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<IEnumerable<Tag>> GetTagsAsync(CancellationToken cancellationToken);
}
