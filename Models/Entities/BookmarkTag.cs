namespace BookmarkManager.Models.Entities;

public class BookmarkTag
{
    public int Id { get; set; }

    public int BookmarkId { get; set; }

    public int TagId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public Bookmark Bookmark { get; set; } = null!;

    public Tag Tag { get; set; } = null!;
}
