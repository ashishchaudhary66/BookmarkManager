namespace BookmarkManager.Models.Dtos;

public record TagUpdateDto
{
    public required string Name { get; init; }
    public required string ColorCode { get; init; }
}
