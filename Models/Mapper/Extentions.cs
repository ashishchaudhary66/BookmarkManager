using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Entities;

namespace BookmarkManager.Models.Mapper;

public static class Extensions
{
    public static TagResponseDto ToDto(this Tag tag)
    {
        return new TagResponseDto
        {
            Id = tag.Id,
            Name = tag.Name,
            ColorCode = tag.ColorCode
        };
    }

    public static IEnumerable<TagResponseDto> ToDto(
        this IEnumerable<Tag> tags)
    {
        return tags.Select(tag => tag.ToDto());
    }

    public static BookmarkResponseDto ToDto(this Bookmark bookmark)
    {
        return new BookmarkResponseDto
        {
            Id = bookmark.Id,
            Title = bookmark.Title,
            Url = bookmark.Url,
            Tags = bookmark.BookmarkTags?
                .Select(bt => bt.Tag.ToDto())
                .ToList()
                ?? []
        };
    }

    public static IEnumerable<BookmarkResponseDto> ToDto(
        this IEnumerable<Bookmark> bookmarks)
    {
        return bookmarks.Select(bookmark => bookmark.ToDto());
    }
}