using Domain.Users;

namespace API.Dtos;

public record UserDto(
    Guid? Id,
    string? FirstName,
    string? MiddleName,
    string? LastName,
    string? Email,
    string? ProfilePicture,
    DateTime? CreatedAt,
    DateTime? UpdatedAt)
{
    public static UserDto FromDomainModel(User user)
        => new UserDto(
            Id: user.Id.Value,
            FirstName: user.FirstName,
            MiddleName: user.MiddleName,
            LastName: user.LastName,
            Email: user.Email,
            ProfilePicture: user.ProfilePicture,
            CreatedAt: user.CreatedAt,
            UpdatedAt: user.UpdatedAt);
}
