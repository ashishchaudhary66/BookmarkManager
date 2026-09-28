using BookmarkManager.Models.Entities;

namespace BookmarkManager.Interfaces;

public interface ITagRepository
{
    Task<IEnumerable<Tag>> GetTagsAsync(CancellationToken cancellationToken);
}
