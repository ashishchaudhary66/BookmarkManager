using BookmarkManager.Interfaces;
using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Mapper;

namespace BookmarkManager.Services;

public class TagService(ITagRepository tagRepository) : ITagService
{
    private readonly ITagRepository _tagRepository = tagRepository;

    public async Task<ServiceResult<IReadOnlyList<TagResponseDto>>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var tags = await _tagRepository.GetAllAsync(cancellationToken);
        var dto = tags.ToDto();
        return ServiceResult<IReadOnlyList<TagResponseDto>>.Success(dto);
    }
}
