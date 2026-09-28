namespace BookmarkManager.Models.Dtos;

public record TagResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string ColorCode { get; set; }
}
