using BookmarkManager.Interfaces;
using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Mapper;

namespace BookmarkManager.Services;

public class TagService(ITagRepository tagRepository) : ITagService
{
    private readonly ITagRepository _tagRepository = tagRepository;

    /// <summary>
    ///  Get all tags
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ServiceResult<IEnumerable<TagResponseDto>>> GetTagsAsync(
        CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetTagsAsync(cancellationToken);
        var dto = tags.EntityToDtoMap();
        return ServiceResult<IEnumerable<TagResponseDto>>.Success(dto);
    }
}
