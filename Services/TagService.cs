using BookmarkManager.Interfaces;
using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Mapper;

namespace BookmarkManager.Servicess;

public class TagService : ITagService
{
    private readonly ITagRepository _tagRepository;

    public TagService(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<ServiceResult<IEnumerable<TagResponseDto>>> GetTagsAsync(
        CancellationToken cancellationToken)
    {
        var tags = await _tagRepository.GetTagsAsync(cancellationToken);
        var dto = tags.EntityToDtoMap();
        return ServiceResult<IEnumerable<TagResponseDto>>.Success(dto);
    }
}
