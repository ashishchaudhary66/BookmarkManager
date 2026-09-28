using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Entities;

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
}
