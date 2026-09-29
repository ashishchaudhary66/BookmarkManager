namespace BookmarkManager.Models.Dtos;

public record TagCreateDto
{
    public required string Name { get; init; }
    public required string ColorCode { get; init; }
}
