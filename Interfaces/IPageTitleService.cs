namespace BookmarkManager.Interfaces
{
    public interface IPageTitleService
    {
        Task<string?> TryGetTitleAsync(
        Uri uri,
        CancellationToken cancellationToken = default);
    }
}
