namespace BookmarkManager.Models.Entities;

public class Tag
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string ColorCode { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public ICollection<BookmarkTag> BookmarkTags { get; set; }
        = new List<BookmarkTag>();
}
