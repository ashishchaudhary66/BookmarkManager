namespace BookmarkManager.Models.Dtos;

public record BookmarkResponseDto
{
    public int Id { get; init; }
    public string? Title { get; init; }
    public required string Url { get; init; }
    public IEnumerable<TagResponseDto> Tags { get; init; } = new List<TagResponseDto>();
}
