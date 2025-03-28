using Domain;

namespace API.Dtos;

public record UserDto()
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? ProfilePicture { get; set; }
    public List<Section> Sections { get; set; } = new();    
}

public class CreateUserDto
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? ProfilePicture { get; set; }
}

public class UpdateUserDto
{
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? ProfilePicture { get; set; }
}