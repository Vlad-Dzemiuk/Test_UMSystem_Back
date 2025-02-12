using Domain;

namespace API.Dtos;

public record SectionDto(
    Guid? Id,
    string? Name,
    Guid? UserId,
    UserDto? User,
    DateTime? CreatedAt)
{
    /*public static SectionDto FromDomainModel(Section section)
        => new(
            Id: section.Id.Value,
            Name: section.Name,
            UserId: section.UserId.Value,
            User: section.User is null ? null : UserDto.FromDomainModel(section.User),
            CreatedAt: section.CreatedAt);*/
}