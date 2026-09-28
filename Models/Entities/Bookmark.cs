namespace BookmarkManager.Models.Entities;

public class Bookmark
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string Url { get; set; } = string.Empty;

    public string NormalizedUrl { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<BookmarkTag> BookmarkTags { get; set; }
        = new List<BookmarkTag>();
}
