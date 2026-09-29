using BookmarkManager.Models.Entities;

namespace BookmarkManager.Interfaces;

public interface ITagRepository
{
    Task<IReadOnlyList<Tag>> GetActiveByIdsAsync(
        IEnumerable<int> ids,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Tag>> GetAllAsync(
        CancellationToken cancellationToken = default);
}