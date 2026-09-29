using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Entities;
using System.Linq;

namespace BookmarkManager.Models.Mapper;

public static class Extentions
{
    /// <summary>
    /// Entity list to Dto list 
    /// </summary>
    /// <param name="tags"></param>
    /// <returns></returns>
    public static IEnumerable<TagResponseDto> EntityToDtoMap(this IEnumerable<Tag> tags)
    {
        return tags.Select(tag => new TagResponseDto
        {
            Id = tag.Id,
            Name = tag.Name,
            ColorCode = tag.ColorCode
        });
    }

        public static TagResponseDto ToDto(this Tag tag)
        {
            return new TagResponseDto
            {
                Id = tag.Id,
                Name = tag.Name,
                ColorCode = tag.ColorCode
            };
        }

        public static BookmarkResponseDto ToDto(this Bookmark bookmark)
        {
            return new BookmarkResponseDto
            {
                Id = bookmark.Id,
                Title = bookmark.Title,
                Url = bookmark.Url,
                Tags = bookmark.BookmarkTags?.Select(bt => bt.Tag.ToDto()).ToList() ?? new List<TagResponseDto>()
            };
        }

        public static IEnumerable<BookmarkResponseDto> EntityToDtoMap(this IEnumerable<Bookmark> bookmarks)
        {
            return bookmarks.Select(b => b.ToDto());
        }
}

