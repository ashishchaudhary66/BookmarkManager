using System.Collections.Generic;

namespace BookmarkManager.Models.Dtos;

public record BookmarkUpdateDto
{
    public string? Title { get; init; }
    public required string Url { get; init; }
    public IEnumerable<int>? TagIds { get; init; }
}
