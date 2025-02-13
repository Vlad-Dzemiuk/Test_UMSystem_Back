using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain;

[BsonIgnoreExtraElements]
public class User
{
    [BsonId]
    [BsonElement("user_id"), BsonRepresentation(BsonType.ObjectId)]
    public string? UserId { get; set; }

    [BsonElement("first_name"), BsonRepresentation(BsonType.String)]
    public string FirstName { get; set; }

    [BsonElement("middle_name"), BsonRepresentation(BsonType.String)]
    public string MiddleName { get; set; }

    [BsonElement("last_name"), BsonRepresentation(BsonType.String)]
    public string LastName { get; set; }

    [BsonElement("email"), BsonRepresentation(BsonType.String)]
    public string Email { get; set; }

    [BsonElement("profile_picture"), BsonRepresentation(BsonType.String)]
    public string ProfilePicture { get; set; }
    
    [BsonElement("updated_at"), BsonRepresentation(BsonType.DateTime)]
    public DateTime UpdatedAt { get; set; }

    [BsonElement("created_at"), BsonRepresentation(BsonType.DateTime)]
    public DateTime CreatedAt { get; set; }
    
    [BsonIgnoreIfNull]
    public List<Section>? Sections { get; set; } = new List<Section>();

    public User(string firstName, string middleName, string lastName, string email, string profilePicture)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Email = email;
        ProfilePicture = profilePicture;

        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
    }

    public User()
    {
    }
}