using Domain.Section_Items;

namespace API.Dtos;

public record SectionItemDto(
    Guid? Id,
    string? Title,
    string? Content,
    Guid? SectionId,
    SectionDto? Section,
    DateTime? CreatedAt)
{
    public static SectionItemDto FromDomainModel(SectionItem sectionItem)
        => new(
            Id: sectionItem.Id.Value,
            Title: sectionItem.Title,
            Content: sectionItem.Content,
            SectionId: sectionItem.SectionId.Value,
            Section: sectionItem.Section is null ? null : SectionDto.FromDomainModel(sectionItem.Section),
            CreatedAt: sectionItem.CreatedAt);
}