using BookmarkManager.Interfaces;
using BookmarkManager.Models;
using BookmarkManager.Models.Dtos;
using BookmarkManager.Models.Entities;
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

    public async Task<ServiceResult<TagResponseDto>> CreateAsync(
        TagCreateDto request,
        CancellationToken cancellationToken = default)
    {
        var name = request.Name?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            return ServiceResult<TagResponseDto>.ValidationFailure(
                "Name",
                "Tag name is required.");
        }

        var colorCode = request.ColorCode?.Trim();

        if (string.IsNullOrWhiteSpace(colorCode))
        {
            return ServiceResult<TagResponseDto>.ValidationFailure(
                "ColorCode",
                "Color code is required.");
        }

        var existingTag = await _tagRepository.GetByNameAsync(
            name,
            cancellationToken);

        if (existingTag != null)
        {
            return ServiceResult<TagResponseDto>.Conflict(
                "A tag with this name already exists.");
        }

        var tag = new Tag
        {
            Name = name,
            ColorCode = colorCode,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsDeleted = false
        };

        await _tagRepository.AddAsync(
            tag,
            cancellationToken);

        return ServiceResult<TagResponseDto>.Created(tag.ToDto());
    }
}
