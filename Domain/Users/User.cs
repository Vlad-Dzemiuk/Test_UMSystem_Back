using Domain.Sections;

namespace Domain.Users;

public class User
{
    public UserId Id { get; }
    public string FirstName { get; private set; }
    public string MiddleName { get; private set; }
    public string LastName { get; private set; }
    public string Email { get; private set; }
    public string ProfilePicture { get; private set; }
    public List<Section> Sections { get; }

    public DateTime UpdatedAt { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public static User New(
        UserId id,
        string firstName,
        string middleName,
        string lastName,
        string email,
        string password,
        string profilePicture)
        => new(id,
            firstName,
            middleName,
            lastName,
            email,
            password,
            DateTime.UtcNow,
            profilePicture);

    private User(
        UserId id,
        string firstName,
        string middleName,
        string lastName,
        string email,
        string password,
        DateTime createdAt,
        string profilePicture)
    {
        Id = id;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Email = email;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        ProfilePicture = profilePicture;
    }

    public void UpdateDetails(string firstName, string middleName, string lastName)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        UpdatedAt = DateTime.UtcNow;
    }

    public void UpdateProfilePicture(string profilePicture)
    {
        ProfilePicture = profilePicture;
        UpdatedAt = DateTime.UtcNow;
    }
}